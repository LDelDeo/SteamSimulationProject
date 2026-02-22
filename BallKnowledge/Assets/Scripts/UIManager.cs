using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform rosterGrid;
    public GameObject employeeCard;

    public Transform prospectLayout;
    public GameObject prospectCard;

    public Transform freeAgencyLayout;
    public GameObject freeAgentCard;

    public Transform expiringContractsLayout;
    public GameObject expiringContractCard;

    public Transform[] layouts;

    [Header("HUD Stats")]
    [SerializeField] TMP_Text capSpaceText;
    [SerializeField] TMP_Text draftPicksText;
    [SerializeField] TMP_Text leagueYearText;

    [Header("Script References")]
    private EmployeeLists employeeLists;
    private GeneralManager generalManager;
    private UIManager uiManager;
    private TestGeneration testGeneration;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        generalManager = GetComponent<GeneralManager>();
        uiManager = GetComponent<UIManager>();
        testGeneration = GetComponent<TestGeneration>();

        
    }

    public void RefreshUI()
    {
        RefreshRosterUI();
        RefreshDraftUI();
        RefreshFreeAgentUI();
        RefreshExpiringContractsUI();

        UpdateCapSpace();
        UpdateDraftPicks();
        UpdateLeagueYear();
    }

    private void RefreshRosterUI()
    {
        ClearGrid(rosterGrid);

        foreach (var employee in employeeLists.currentRoster)
        {
            GameObject cardObject = Instantiate(employeeCard, rosterGrid);
            EmployeeCard card = cardObject.GetComponent<EmployeeCard>();
            card.GetEmployeeStats(employee);
        }
    }

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

    private void UpdateCapSpace()
    {
        generalManager.currentUsedCapSpace = 0;

        foreach (var employee in employeeLists.currentRoster)
        {
            generalManager.currentUsedCapSpace += employee.hourlyWage;
        }

        capSpaceText.text = $"${generalManager.currentUsedCapSpace} / $275";
    }

    public void UpdateDraftPicks()
    {
        draftPicksText.text = $"{generalManager.currentYear} Draft Picks: {generalManager.draftPicks}";
    }

    public void UpdateLeagueYear()
    {
        leagueYearText.text = generalManager.currentYear.ToString();
    }

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
}