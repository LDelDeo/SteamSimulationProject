using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region UI Elements
    [Header("Action Canvas")]
    public GameObject actionCanvasPrefab;
    public Transform actionCanvasInstantiatePoint;
    public GameObject currentActionCanvas;
    public GameObject actionCanvasText;
    private GameObject closeActionCanvasButton;

    [Header("Roster Screen UI")]
    public GameObject rosterScreen;
    public GameObject OffenseScreen;
    public GameObject DefenseScreen;

    public GameObject rosterGridStorage;
    public Transform employeeStatsContent;

    public Transform[] rosterSlots;
    public TMP_Text[] rosterSlotText;

    public GameObject employeeCardPrefab;
    public GameObject employeeProfileCardPrefab;

    private bool isRosterShowing;

    [Header("Draft Screen UI")]
    public GameObject draftScreen;
    public Transform prospectContent;

    public GameObject prospectCardPrefab;

    public TMP_Text currentRoundText;
    public TMP_Text draftPicksByRoundText;

    [Header("Free Agency Screen UI")]
    public GameObject freeAgencyScreen;
    public Transform freeAgencyContent;

    public GameObject freeAgentCardPrefab;

    [Header("Restricted FA Screen UI")]
    public GameObject expiringContractsScreen;
    public Transform expiringContractsContent;

    public GameObject expiringContractCardPrefab;

    [Header("Retirement Screen UI")]
    public GameObject retirementScreen;
    public Transform retiringEmployeeContent;
    public GameObject retiringEmployeeCardPrefab;

    [Header("Employee Events Screen UI")]
    public GameObject disgruntlementsScreen;
    public Transform disgruntledEmployeeContent;
    public GameObject disgruntledCardPrefab;

    [Header("Layout Array")]
    public GameObject[] screens;

    [Header("General HUD")]
    [SerializeField] TMP_Text capSpaceText;
    [SerializeField] TMP_Text draftPicksText;
    [SerializeField] TMP_Text leagueYearText;
    public TMP_Text emptyListText;
    public GameObject nextPeriodButton;
    #endregion

    [Header("Script References")]
    private EmployeeLists employeeLists;
    private GeneralManager generalManager;
    private UIManager uiManager;
    private PeriodManager periodManager;
    private DraftManager draftManager;
    private GeneralManager manager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        generalManager = GetComponent<GeneralManager>();
        uiManager = GetComponent<UIManager>();
        periodManager = GetComponent<PeriodManager>();
        draftManager = GetComponent<DraftManager>();
        manager = GetComponent<GeneralManager>();

        OffenseScreen.SetActive(true);
        DefenseScreen.SetActive(false);
    }

    public void RefreshUI()
    {
        RefreshRosterUI();
        RefreshDraftUI();
        RefreshFreeAgentUI();
        RefreshExpiringContractsUI();
        RefreshRetirementsUI();
        RefreshDisgruntledEmployeesUI();

        UpdateCapSpace();
        UpdateDraftPicks();
        UpdateLeagueYear();
    }

    #region Action Canvas Functionality
    public void FindActionCanvas()
    {
        currentActionCanvas = GameObject.Find("Action Canvas(Clone)");
        actionCanvasText = GameObject.Find("Outcome Text");

        closeActionCanvasButton = GameObject.Find("Acknowledge Button");
        closeActionCanvasButton.GetComponent<Button>().onClick.AddListener(CloseActionCanvas);
    }

    public void OpenActionCanvas()
    {
        Instantiate(actionCanvasPrefab, actionCanvasInstantiatePoint);
        FindActionCanvas();
    }

    public void CloseActionCanvas()
    {
        FindActionCanvas();
        Destroy(currentActionCanvas);
    }
    #endregion

    #region Action Canvas Messages
    public void InsufficientCapRoom(Employee employee)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = "Not enough cap space to complete deal\n" +
                                                                   $"You must free up ${(manager.currentUsedCapSpace + employee.hourlyWage) - manager.maxCapSpace}";
    }

    public void InsufficientRosterSpace(Employee employee)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"You're currently at the maximum amount of {employee.jobPosition}s\n" +
                                                                   $" Cut an existing {employee.jobPosition} to complete the deal";
    }

    public void InsufficientDraftPicks(string currentRound)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"Not enough {currentRound} round picks to make selection";
    }

    public void UncuttableContract()
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = "Employees with a contract length of over 4 years cannot be cut";
    }

    public void EmployeeCut(Employee employee)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"Employee Cut. Cap Space Savings: ${employee.hourlyWage}/hr";
    }

    public void EmployeeStaying(Employee employee)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"{employee.firstName} has had a change of heart and has decided to stay";
    }

    public void EmployeeWantsOut(Employee employee)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"{employee.firstName} still wants out of your franchise";
    }

    public void EmployeeExtention(Employee employee, int yearExtensionAmount)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"Contract extended by {yearExtensionAmount} year(s)" +
                                                    $" {employee.firstName} now has a total of {employee.yearsUnderContract} year(s) remaining on their deal" +
                                                    $" and earns {employee.hourlyWage}/hr during that time";
    }

    public void EmployeeRaise(Employee employee, int raiseAmount, bool hasEnoughCapSpace)
    {
        if (hasEnoughCapSpace)
        {
            OpenActionCanvas();
            actionCanvasText.GetComponent<TMP_Text>().text = $"Contract raised by ${raiseAmount}/hr\n" +
                                                        $" {employee.firstName} now makes a total of " +
                                                        $" ${employee.hourlyWage}/hr\n" +
                                                        $" and has {employee.yearsUnderContract} year(s) remaining on their deal";
        }
        else
        {
            OpenActionCanvas();
            actionCanvasText.GetComponent<TMP_Text>().text = "You do not have the required cap room to reconstructure the deal" +
                                                        $" You must free up ${(manager.currentUsedCapSpace + raiseAmount) - manager.maxCapSpace}";
        }
    }

    public void EmployeeRetiring(Employee employee, bool isRetiringForGood)
    {
        if (isRetiringForGood)
        {
            OpenActionCanvas();
            actionCanvasText.GetComponent<TMP_Text>().text = $"{employee.firstName} {employee.lastName} is retiring for good, but appreciates the offer to come back";
        }
        else
        {
            OpenActionCanvas();
            actionCanvasText.GetComponent<TMP_Text>().text = $"Welcome {employee.firstName} {employee.lastName} back for at least another year!";
        }
    }

    public void EmployeeIsJoiningHallOfFame(Employee employee)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"{employee.firstName} {employee.lastName} is honored to be added to the {manager.franchiseName} hall of fame";
    }

    public void DraftPicksForCapSpace(int amountOfPicks, string pickType, int additionalCapSpace)
    {
        OpenActionCanvas();
        actionCanvasText.GetComponent<TMP_Text>().text = $"You will be trading {amountOfPicks} {pickType} round pick(s) for an increase of ${additionalCapSpace} of cap space";
    }
    #endregion

    #region Roster UI
    private void RefreshRosterUI()
    {
        ClearContent(rosterGridStorage.transform);

        foreach (Transform slot in rosterSlots)
        {
            ClearContent(slot);
        }

        foreach (var employee in employeeLists.currentRoster)
        {
            GameObject cardObject = Instantiate(employeeCardPrefab, CheckEmployeeRosterSlot(employee));
            EmployeeCard card = cardObject.GetComponent<EmployeeCard>();
            card.GetEmployeeStats(employee);
            card.SetEmployeeCardBackground(employee);
        }

        foreach (Transform child in employeeStatsContent)
            Destroy(child.gameObject);

        UpdateRosterSlotText();
    }

    public void UpdateRosterSlotText()
    {
        rosterSlotText[0].text = $"Busser {employeeLists.rosterConstruction.currentBusser}/{employeeLists.rosterConstruction.maxbusser}";
        rosterSlotText[1].text = $"Janitor {employeeLists.rosterConstruction.currentJanitor}/{employeeLists.rosterConstruction.maxJanitor}";
        rosterSlotText[2].text = $"Media Manager {employeeLists.rosterConstruction.currentMediaManager}/{employeeLists.rosterConstruction.maxMediaManager}";
        rosterSlotText[3].text = $"Expediter {employeeLists.rosterConstruction.currentExpediter}/{employeeLists.rosterConstruction.maxExpediter}";
        rosterSlotText[4].text = $"Cashier {employeeLists.rosterConstruction.currentCashier}/{employeeLists.rosterConstruction.maxCashier}";
        rosterSlotText[5].text = $"Drive Thru Attendee {employeeLists.rosterConstruction.currentDriveThruAttendee}/{employeeLists.rosterConstruction.maxDriveThruAttendee}";
        rosterSlotText[6].text = $"Patty Flipper {employeeLists.rosterConstruction.currentPattyFlipper}/{employeeLists.rosterConstruction.maxPattyFlipper}";
        rosterSlotText[7].text = $"Fry Cook {employeeLists.rosterConstruction.currentFryCook}/{employeeLists.rosterConstruction.maxFryCook}";
        rosterSlotText[8].text = $"Prep Cook {employeeLists.rosterConstruction.currentPrepCook}/{employeeLists.rosterConstruction.maxPrepCook}";
        rosterSlotText[9].text = $"Line Cook {employeeLists.rosterConstruction.currentLineCook}/{employeeLists.rosterConstruction.maxLineCook}";
        rosterSlotText[10].text = $"Shift Manager {employeeLists.rosterConstruction.currentShiftManager}/{employeeLists.rosterConstruction.maxShiftManager}";
        rosterSlotText[11].text = $"Manager {employeeLists.rosterConstruction.currentManager}/{employeeLists.rosterConstruction.maxManager}";
    }

    public Transform CheckEmployeeRosterSlot(Employee employee)
    {
        switch (employee.jobPosition)
        {
            case EmployeeEnumerators.JobType.Busser:
                return rosterSlots[0];

            case EmployeeEnumerators.JobType.Janitor:
                return rosterSlots[1];

            case EmployeeEnumerators.JobType.Media_Manager:
                return rosterSlots[2];

            case EmployeeEnumerators.JobType.Expediter:
                return rosterSlots[3];

            case EmployeeEnumerators.JobType.Cashier:
                return rosterSlots[4];

            case EmployeeEnumerators.JobType.Drive_Thru_Attendee:
                return rosterSlots[5];

            case EmployeeEnumerators.JobType.Patty_Flipper:
                return rosterSlots[6];

            case EmployeeEnumerators.JobType.Fry_Cook:
                return rosterSlots[7];

            case EmployeeEnumerators.JobType.Prep_Cook:
                return rosterSlots[8];

            case EmployeeEnumerators.JobType.Line_Cook:
                return rosterSlots[9];

            case EmployeeEnumerators.JobType.Shift_Manager:
                return rosterSlots[10];

            case EmployeeEnumerators.JobType.Manager:
                return rosterSlots[11];
        }

        return null;
    }

    public void SwitchRosterLayoutScreen(bool showingOffense)
    {
        if (showingOffense) { OffenseScreen.SetActive(true); DefenseScreen.SetActive(false); }
        else if (!showingOffense) { OffenseScreen.SetActive(false); DefenseScreen.SetActive(true); }
    }
    #endregion

    private void RefreshDraftUI()
    {
        ClearContent(prospectContent);

        foreach (var prospect in employeeLists.draftClass)
        {
            GameObject cardObject = Instantiate(prospectCardPrefab, prospectContent);
            ProspectCard card = cardObject.GetComponent<ProspectCard>();
            card.GetEmployeeStats(prospect);
        }
    }

    private void RefreshFreeAgentUI()
    {
        ClearContent(freeAgencyContent);

        foreach (var freeAgent in employeeLists.freeAgentClass)
        {
            GameObject cardObject = Instantiate(freeAgentCardPrefab, freeAgencyContent);
            FreeAgentCard card = cardObject.GetComponent<FreeAgentCard>();
            card.GetEmployeeStats(freeAgent);
            card.SetEmployeeCardBackground(freeAgent);
        }
    }

    private void RefreshExpiringContractsUI()
    {
        ClearContent(expiringContractsContent);

        foreach (var expiringContract in employeeLists.pendingFreeAgents)
        {
            GameObject cardObject = Instantiate(expiringContractCardPrefab, expiringContractsContent);
            FreeAgentCard card = cardObject.GetComponent<FreeAgentCard>();
            card.GetEmployeeStats(expiringContract);
            card.SetEmployeeCardBackground(expiringContract);
        }
    }

    private void RefreshRetirementsUI()
    {
        ClearContent(retiringEmployeeContent);

        foreach (var retirement in employeeLists.retiringEmployees)
        {
            GameObject cardObject = Instantiate(retiringEmployeeCardPrefab, retiringEmployeeContent);
            RetirementCard card = cardObject.GetComponent<RetirementCard>();
            card.GetEmployeeStats(retirement);
            card.SetEmployeeCardBackground(retirement);
        }
    }

    private void RefreshDisgruntledEmployeesUI()
    {
        ClearContent(disgruntledEmployeeContent);

        foreach (var disgruntledEmployee in employeeLists.disgruntledEmployees)
        {
            GameObject cardObject = Instantiate(disgruntledCardPrefab, disgruntledEmployeeContent);
            DisgruntledCard card = cardObject.GetComponent<DisgruntledCard>();
            card.GetEmployeeStats(disgruntledEmployee);
            card.SetEmployeeCardBackground(disgruntledEmployee);
        }
    }

    #region HUD
    public void UpdateCapSpace()
    {
        generalManager.currentUsedCapSpace = 0;

        foreach (var employee in employeeLists.currentRoster)
        {
            generalManager.currentUsedCapSpace += employee.hourlyWage;
        }

        capSpaceText.text = $"${generalManager.currentUsedCapSpace} / ${generalManager.maxCapSpace}";
    }

    public void UpdateDraftPicks()
    {
        generalManager.totalDraftPicks = (generalManager.firstRoundPicks + generalManager.secondRoundPicks + generalManager.thirdRoundPicks);
        draftPicksText.text = $"{generalManager.currentYear} Draft Picks: {generalManager.totalDraftPicks}";
        draftPicksByRoundText.text = $"Round 1 Picks: {generalManager.firstRoundPicks} | Round 2 Picks: {generalManager.secondRoundPicks} | Round 3 Picks: {generalManager.thirdRoundPicks}";
    }

    public void UpdateLeagueYear()
    {
        leagueYearText.text = generalManager.currentYear.ToString();
    }
    #endregion

    #region Clearing and Changing UI
    private void ClearContent(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangeUI(GameObject screenToShow)
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }

        screenToShow.SetActive(true);
        RefreshUI();
    }

    public void ShowRosterScreen()
    {
        isRosterShowing = !isRosterShowing;

        if (isRosterShowing) { rosterScreen.gameObject.SetActive(true); }
        else if (!isRosterShowing && periodManager.currentPeriod != PeriodManager.Period.StartOfYear) { rosterScreen.SetActive(false); }

        RefreshRosterUI();
    }
    #endregion
}