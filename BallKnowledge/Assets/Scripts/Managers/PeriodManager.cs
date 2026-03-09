using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PeriodManager : MonoBehaviour
{
    [Header("Current Period")]
    public Period currentPeriod;
    public int currentPeriodIndex = 0;
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

    [Header("List Sizes")]
    [SerializeField] int rosterCount;
    [SerializeField] int draftClassSize;
    [SerializeField] int freeAgencyClassSize;

    private EmployeeCard employeeCardObject;
    private ProspectCard prospectCardObject;
    private FreeAgentCard freeAgentCardObject;
    private RetirementCard retirementCardObject;

    private EmployeeLists employeeLists;
    private UIManager uiManager;
    private GeneralManager generalManager;
    private EmployeeFactory employeeFactory;

    EmployeeRNG employeeRNG = new EmployeeRNG();
    EmployeeArrays employeeArrays = new EmployeeArrays();

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        uiManager = GetComponent<UIManager>();
        generalManager = GetComponent<GeneralManager>();

        employeeFactory = new EmployeeFactory();

        employeeArrays.JsonToList();

        employeeCardObject = uiManager.employeeCard.GetComponent<EmployeeCard>();
        prospectCardObject = uiManager.prospectCard.GetComponent<ProspectCard>();
        freeAgentCardObject = uiManager.freeAgentCard.GetComponent<FreeAgentCard>();
        retirementCardObject = uiManager.retiringEmployeeCard.GetComponent<RetirementCard>();

        CreateAnEmployee(rosterCount, employeeFactory, employeeLists, employeeArrays, employeeLists.currentRoster, employeeCardObject, uiManager.rosterGridStorage);
        uiManager.RefreshUI();

        UpdatePeriod();
    }

    #region Changing Period
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
                uiManager.ChangeUI(uiManager.rosterScreen);
                break;

            case Period.Retirements:
                CheckforRetirement();
                uiManager.ChangeUI(uiManager.retiringEmployeeLayout);
                break;

            case Period.ExpiringContracts:
                employeeLists.ClearList(employeeLists.retiringEmployees);
                CheckExpiringContracts();
                uiManager.ChangeUI(uiManager.expiringContractsLayout);
                break; 

            case Period.FreeAgency:
                CreateAnEmployee(freeAgencyClassSize, employeeFactory, employeeLists, employeeArrays, employeeLists.freeAgentClass, freeAgentCardObject, uiManager.freeAgencyLayout);
                LetExpiringContractsWalk();
                uiManager.ChangeUI(uiManager.freeAgencyLayout);
                break;

            case Period.Trading:
                employeeLists.ClearList(employeeLists.freeAgentClass);
                break;

            case Period.EmployeeEvents:
                CheckForEmployeeEvent();
                uiManager.ChangeUI(uiManager.disgruntledEmployeeLayout);
                // Load those employees in cards and assign each a random event
                // Allow user to deal with these situations
                break;

            case Period.Draft:
                employeeLists.ClearList(employeeLists.disgruntledEmployees);
                CreateAnEmployee(draftClassSize, employeeFactory, employeeLists, employeeArrays, employeeLists.draftClass, prospectCardObject, uiManager.prospectLayout);
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
    #endregion

    private void CreateAnEmployee(int employeeCount, EmployeeFactory employeeFactory, EmployeeLists employeeLists, EmployeeArrays employeeArrays, List<Employee> listToAddTo, EmployeeCard employeeCard, Transform layout)
    {
        for (int i = 0; i < employeeCount; i++)
        {
            employeeFactory.CreateEmployee(employeeLists, employeeArrays, listToAddTo);
        }

        foreach (var employee in listToAddTo)
        {
            GameObject cardObject = Instantiate(employeeCard.gameObject, layout);
            EmployeeCard cardInstance = cardObject.GetComponent<EmployeeCard>();

            cardInstance.GetEmployeeStats(employee);
            //employeeFactory.PrintStats(employee);
        }
    }

    #region Period Functionality/Checks
    private void NewLeagueYear()
    {
        generalManager.currentYear++;
        generalManager.seasonsElapsed++;

        generalManager.firstRoundPicks += 1;
        generalManager.secondRoundPicks += 1;
        generalManager.thirdRoundPicks += 1;
        uiManager.UpdateDraftPicks();

        foreach (var employee in employeeLists.currentRoster)
        {
            employee.age++;
            employee.yearsUnderContract--;

            if (employee.isRookie)
                employee.isRookie = false;

            UpdateEmployeeOverall(employee);
        }
    }

    private void UpdateEmployeeOverall(Employee employee)
    {
        List<int> statIncreases = new List<int>();
        int minStatsIncrease = 0;
        int maxStatsIncrease = 0;

        switch (employee.workEthic)
        {
            case EmployeeEnumerators.WorkEthic.Bum:
                minStatsIncrease = 0;
                maxStatsIncrease = 1;
                break;

            case EmployeeEnumerators.WorkEthic.Lazy:
                minStatsIncrease = 0;
                maxStatsIncrease = 2;
                break;

            case EmployeeEnumerators.WorkEthic.Paycheck_Collector:
                minStatsIncrease = 0;
                maxStatsIncrease = 3;
                break;

            case EmployeeEnumerators.WorkEthic.Gets_The_Job_Done:
                minStatsIncrease = 1;
                maxStatsIncrease = 3;
                break;

            case EmployeeEnumerators.WorkEthic.Motivated:
                minStatsIncrease = 2;
                maxStatsIncrease = 3;
                break;

            case EmployeeEnumerators.WorkEthic.Grinder:
                minStatsIncrease = 2;
                maxStatsIncrease = 4;
                break;

            case EmployeeEnumerators.WorkEthic.X_Factor:
                minStatsIncrease = 3;
                maxStatsIncrease = 5;
                break;
        }

        
        if (employee.age <= 31)
        {
            for (int i = 0; i < 5; i++)
            {
                int randomStatIncrease = Random.Range(minStatsIncrease, maxStatsIncrease);
                statIncreases.Add(randomStatIncrease);
            }
            
            // Maybe make it so if its greater than 100, set it to 100
            if (employee.efficiency <= (100 - statIncreases[0]))
                employee.efficiency += statIncreases[0];

            if (employee.customerService <= (100 - statIncreases[1]))
                employee.customerService += statIncreases[1];

            if (employee.communication <= (100 - statIncreases[2]))
                employee.communication += statIncreases[2];

            if (employee.teamwork <= (100 - statIncreases[3]))
                employee.teamwork += statIncreases[3];

            if (employee.iq <= (100 - statIncreases[4]))
                employee.iq += statIncreases[4];

            statIncreases.Clear();
        }
        else
        {
            employee.efficiency -= 2;
            employee.customerService -= 2;
            employee.communication -= 2;
            employee.teamwork -= 2;
            employee.iq -= 2;

            if (employee.efficiency < 0)
                employee.efficiency = 0;

            if (employee.customerService < 0)
                employee.customerService = 0;

            if (employee.communication < 0)
                employee.communication = 0;

            if (employee.teamwork < 0)
                employee.teamwork = 0;

            if (employee.iq < 0)
                employee.iq = 0;
        }
        
        employee.overall = (employee.efficiency + 
                            employee.customerService + 
                            employee.communication +
                            employee.teamwork +
                            employee.iq) 
                            / 5;
    }

    private void CheckforRetirement()
    {
        int randomNumber = Random.Range(1, 101); 

        foreach (var employee in employeeLists.currentRoster.ToList())
        {
            switch (employee.age)
            {
                case 35:
                    if (randomNumber > 0 && randomNumber < 21) { AddToRetirement(employee); } // 20% Chance
                    break;

                case 36:
                    if (randomNumber > 0 && randomNumber < 31) { AddToRetirement(employee); } // 30% Chance
                    break;

                case 37:
                    if (randomNumber > 0 && randomNumber < 41) { AddToRetirement(employee); } // 40% Chance
                    break;

                case 38:
                    if (randomNumber > 0 && randomNumber < 61) { AddToRetirement(employee); } // 60% Chance
                    break;

                case 39:
                    if (randomNumber > 0 && randomNumber < 81) { AddToRetirement(employee); } // 80% Chance
                    break;

                case 40:
                    AddToRetirement(employee); // 100% Chance
                    break;
            }
        }
    }

    private void AddToRetirement(Employee employee)
    {
        employeeLists.AddEmployee(employee, employeeLists.retiringEmployees);
        employeeLists.RemoveEmployee(employee, employeeLists.currentRoster);
    }

    private void CheckExpiringContracts()
    {
        foreach (var employee in employeeLists.currentRoster.ToList())
        {
            if (employee.yearsUnderContract == 0)
            {
                employee.value = employeeFactory.EmployeeValueCalucator(employee);
                employee.hourlyWage = employeeRNG.GetRandomWage(employee);

                employeeLists.AddEmployee(employee, employeeLists.pendingFreeAgents);
                employeeLists.RemoveEmployee(employee, employeeLists.currentRoster);
            }
        }
    }

    private void LetExpiringContractsWalk()
    {
        foreach (var expiringContract in employeeLists.pendingFreeAgents.ToList())
        {
            // Recalcutes/randomizes the asking price once they hit free agency so you might be able to get them cheaper or more expensive
            expiringContract.hourlyWage = employeeRNG.GetRandomWage(expiringContract);

            employeeLists.AddEmployee(expiringContract, employeeLists.freeAgentClass);
            employeeLists.RemoveEmployee(expiringContract, employeeLists.pendingFreeAgents);
        }
    }

    private void CheckForEmployeeEvent()
    {
        foreach (var employee in employeeLists.currentRoster.ToList())
        {
            int randomNumber = Random.Range(1, 101);

            switch (employee.personalityTrait)
            {
                case EmployeeEnumerators.PersonalityTrait.Toxic:
                    if (randomNumber > 0 && randomNumber < 81) { EmployeeIsDisgruntled(employee); } // 80% Chance
                    break;

                case EmployeeEnumerators.PersonalityTrait.Selfish:
                    if (randomNumber > 0 && randomNumber < 61) { EmployeeIsDisgruntled(employee); } // 60% Chance
                    break;

                case EmployeeEnumerators.PersonalityTrait.Difficult:
                    if (randomNumber > 0 && randomNumber < 46) { EmployeeIsDisgruntled(employee); } // 45% Chance
                    break;

                case EmployeeEnumerators.PersonalityTrait.Team_Player:
                    if (randomNumber > 0 && randomNumber < 26) { EmployeeIsDisgruntled(employee); } // 25% Chance
                    break;

                case EmployeeEnumerators.PersonalityTrait.Saint:
                    if (randomNumber > 0 && randomNumber < 11) { EmployeeIsDisgruntled(employee); } // 10% Chance
                    break;

                case EmployeeEnumerators.PersonalityTrait.Perfectionist:
                    // 0% Chance
                    break;
            }
        } 
    }

    private void EmployeeIsDisgruntled(Employee employee)
    {
        employeeLists.AddEmployee(employee, employeeLists.disgruntledEmployees);
        employeeLists.RemoveEmployee(employee, employeeLists.currentRoster);
    }
    #endregion
}
