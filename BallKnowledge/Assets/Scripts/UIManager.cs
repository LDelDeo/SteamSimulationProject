using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Transform rosterGrid;
    [SerializeField] GameObject employeeCard;

    [SerializeField] Transform prospectLayout;
    [SerializeField] GameObject prospectCard;

    [SerializeField] Transform freeAgencyLayout;
    [SerializeField] GameObject freeAgentCard;

    [SerializeField] public TMP_Text capSpaceText;

    [Header("Script References")]
    private EmployeeLists employeeLists;
    private GeneralManager generalManager;
    private UIManager uiManager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        generalManager = GetComponent<GeneralManager>();
        uiManager = GetComponent<UIManager>();
    }

    public void RefreshUI()
    {
        RefreshRosterUI();
        RefreshDraftUI();
        RefreshFreeAgentUI();

        UpdateCapSpace();
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

    private void UpdateCapSpace()
    {
        generalManager.currentUsedCapSpace = 0;

        foreach (var employee in employeeLists.currentRoster)
        {
            generalManager.currentUsedCapSpace += employee.hourlyWage;
        }

        uiManager.capSpaceText.text = $"${generalManager.currentUsedCapSpace} / $275";
    }

    private void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }
}