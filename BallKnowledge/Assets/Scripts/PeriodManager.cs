using System.Linq;
using UnityEngine;

public class PeriodManager : MonoBehaviour
{
    [SerializeField] Period currentPeriod;
    [SerializeField] int currentPeriodIndex = 0;
    public enum Period
    {
        StartOfYear = 0,
        Retirements = 1,
        ExpiringContracts = 2,
        FreeAgency = 3,
        Trading = 4,
        EmployeeEvents = 5,
        Draft = 6,
        SeasonSimulation = 7,
        Awards = 8,
        SeasonReflection = 9
    }

    private EmployeeLists employeeLists;
    private UIManager uiManager;
    private GeneralManager generalManager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        uiManager = GetComponent<UIManager>();
        generalManager = GetComponent<GeneralManager>();

        UpdatePeriod();
    }

    private void ChangePeriod()
    {
        currentPeriodIndex++;

        if (currentPeriodIndex > 9)
            currentPeriodIndex = 0;

        switch (currentPeriodIndex)
        {
            case 0: currentPeriod = Period.StartOfYear; break;
            case 1: currentPeriod = Period.Retirements; break;
            case 2: currentPeriod = Period.ExpiringContracts; break;
            case 3: currentPeriod = Period.FreeAgency; break;
            case 4: currentPeriod = Period.Trading; break;
            case 5: currentPeriod = Period.EmployeeEvents; break;
            case 6: currentPeriod = Period.Draft; break;
            case 7: currentPeriod = Period.SeasonSimulation; break;
            case 8: currentPeriod = Period.Awards; break;
            case 9: currentPeriod = Period.SeasonReflection; break;
        }

        UpdatePeriod();
    }

    public void UpdatePeriod()
    {
        switch (currentPeriod)
        {
            case Period.StartOfYear:
                uiManager.ChangeUI(uiManager.rosterGrid);
                break;

            case Period.Retirements:
                break;

            case Period.ExpiringContracts:
                CheckExpiringContracts();
                uiManager.ChangeUI(uiManager.expiringContractsLayout);
                break; 

            case Period.FreeAgency:
                //CreateAnEmployee(freeAgencyClassSize, employeeFactory, employeeLists, employeeLists.freeAgentClass, freeAgentCardObject, uiManager.freeAgencyLayout);
                LetExpiringContractsWalk();
                uiManager.ChangeUI(uiManager.freeAgencyLayout);
                break;

            case Period.Trading:
                employeeLists.ClearList(employeeLists.freeAgentClass);
                break;

            case Period.EmployeeEvents:
                break;

            case Period.Draft:
                //CreateAnEmployee(draftClassSize, employeeFactory, employeeLists, employeeLists.draftClass, prospectCardObject, uiManager.prospectLayout);
                uiManager.ChangeUI(uiManager.prospectLayout);
                break;

            case Period.SeasonSimulation:
                employeeLists.ClearList(employeeLists.draftClass);
                break;

            case Period.Awards:
                break;

            case Period.SeasonReflection:
                NewLeagueYear();
                break;
        }
    }

    private void NewLeagueYear()
    {
        generalManager.currentYear++;
        generalManager.draftPicks = 3;

        foreach (var employee in employeeLists.currentRoster)
        {
            employee.age++;
            employee.yearsUnderContract--;

        }
    }

    private void CheckExpiringContracts()
    {
        foreach (var employee in employeeLists.currentRoster.ToList())
        {
            if (employee.yearsUnderContract == 0)
            {
                employeeLists.AddEmployee(employee, employeeLists.pendingFreeAgents);
                employeeLists.RemoveEmployee(employee, employeeLists.currentRoster);
            }
        }
    }

    private void LetExpiringContractsWalk()
    {
        // Maybe here we can recalcute/randomize the asking price once they hit free agency so you might be able to get them cheaper or more expensive
        foreach (var expiringContract in employeeLists.pendingFreeAgents.ToList())
        {
            employeeLists.AddEmployee(expiringContract, employeeLists.freeAgentClass);
            employeeLists.RemoveEmployee(expiringContract, employeeLists.pendingFreeAgents);
        }
    }
}
