using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Roster Screen UI")]
    public Transform rosterGridStorage;
    public Transform rosterScreen;
    public Transform employeeStatsLayout;

    public Transform[] rosterSlots;
    public TMP_Text[] rosterSlotText;

    public GameObject OffenseScreen;
    public GameObject DefenseScreen;
    public GameObject employeeCard;

    private bool isRosterShowing;

    [Header("Draft Screen UI")]
    public Transform prospectLayout;
    public GameObject prospectCard;

    [Header("Free Agency Screen UI")]
    public Transform freeAgencyLayout;
    public GameObject freeAgentCard;

    [Header("Restricted FA Screen UI")]
    public Transform expiringContractsLayout;
    public GameObject expiringContractCard;

    [Header("Retirement Screen UI")]
    public Transform retiringEmployeeLayout;
    public GameObject retiringEmployeeCard;

    [Header("Layout Array")]
    public Transform[] layouts;

    [Header("HUD Stats")]
    [SerializeField] TMP_Text capSpaceText;
    [SerializeField] TMP_Text draftPicksText;
    [SerializeField] TMP_Text leagueYearText;

    [Header("Script References")]
    private EmployeeLists employeeLists;
    private GeneralManager generalManager;
    private UIManager uiManager;
    private PeriodManager periodManager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        generalManager = GetComponent<GeneralManager>();
        uiManager = GetComponent<UIManager>();
        periodManager = GetComponent<PeriodManager>();

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

        UpdateCapSpace();
        UpdateDraftPicks();
        UpdateLeagueYear();
    }

    #region Roster UI
    private void RefreshRosterUI()
    {
        ClearGrid(rosterGridStorage);

        foreach (Transform slot in rosterSlots)
        {
            ClearGrid(slot);
        }

        foreach (var employee in employeeLists.currentRoster)
        {
            GameObject cardObject = Instantiate(employeeCard, CheckEmployeeRosterSlot(employee));
            EmployeeCard card = cardObject.GetComponent<EmployeeCard>();
            card.GetEmployeeStats(employee);
        }

        foreach (Transform child in employeeStatsLayout.transform)
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
        ClearGrid(prospectLayout);

        foreach (var prospect in employeeLists.draftClass)
        {
            GameObject cardObject = Instantiate(prospectCard, prospectLayout);
            ProspectCard card = cardObject.GetComponent<ProspectCard>();
            card.GetEmployeeStats(prospect);
        }
    }

    private void RefreshFreeAgentUI()
    {
        ClearGrid(freeAgencyLayout);

        foreach (var freeAgent in employeeLists.freeAgentClass)
        {
            GameObject cardObject = Instantiate(freeAgentCard, freeAgencyLayout);
            FreeAgentCard card = cardObject.GetComponent<FreeAgentCard>();
            card.GetEmployeeStats(freeAgent);
        }
    }

    private void RefreshExpiringContractsUI()
    {
        ClearGrid(expiringContractsLayout);

        foreach (var expiringContract in employeeLists.pendingFreeAgents)
        {
            GameObject cardObject = Instantiate(expiringContractCard, expiringContractsLayout);
            FreeAgentCard card = cardObject.GetComponent<FreeAgentCard>();
            card.GetEmployeeStats(expiringContract);
        }
    }

    private void RefreshRetirementsUI()
    {
        ClearGrid(retiringEmployeeLayout);

        foreach (var retirement in employeeLists.retiringEmployees)
        {
            GameObject cardObject = Instantiate(retiringEmployeeCard, retiringEmployeeLayout);
            RetirementCard card = cardObject.GetComponent<RetirementCard>();
            card.GetEmployeeStats(retirement);
        }
    }

    #region HUD
    private void UpdateCapSpace()
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
        draftPicksText.text = $"{generalManager.currentYear} Draft Picks: {generalManager.draftPicks}";
    }

    public void UpdateLeagueYear()
    {
        leagueYearText.text = generalManager.currentYear.ToString();
    }
    #endregion

    #region Clearing and Changing UI
    private void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangeUI(Transform screenToShow)
    {
        foreach (Transform screen in layouts)
        {
            screen.gameObject.SetActive(false);
        }

        screenToShow.gameObject.SetActive(true);
        RefreshUI();
    }

    public void ShowRosterScreen()
    {
        isRosterShowing = !isRosterShowing;

        if (isRosterShowing) { rosterScreen.gameObject.SetActive(true); }
        else if (!isRosterShowing && periodManager.currentPeriod != PeriodManager.Period.StartOfYear) { rosterScreen.gameObject.SetActive(false); }

        RefreshRosterUI();
    }
    #endregion
}