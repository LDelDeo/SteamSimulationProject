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
    [SerializeField] int freeAgencyClassSize;

    [Header("Annual Configuration")]
    [SerializeField] int ageOfRegression;
    [SerializeField] int amountOfRegressionPerStat;

    #region Card Scripts
    private EmployeeCard employeeCardObject;
    private ProspectCard prospectCardObject;
    private FreeAgentCard freeAgentCardObject;
    private RetirementCard retirementCardObject;
    private TradeAssetCard tradeAssetCardObject;
    #endregion

    #region Required Scripts/Classes
    private EmployeeLists employeeLists;
    private UIManager uiManager;
    private GeneralManager generalManager;
    private EmployeeFactory employeeFactory;
    private DraftManager draftManager;
    private TradeManager tradeManager;
    private FaceManager faceManager;

    EmployeeRNG employeeRNG = new EmployeeRNG();
    EmployeeArrays employeeArrays = new EmployeeArrays();
    #endregion

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        uiManager = GetComponent<UIManager>();
        generalManager = GetComponent<GeneralManager>();
        draftManager = GetComponent<DraftManager>();
        tradeManager = GetComponent<TradeManager>();
        faceManager = GetComponent<FaceManager>();

        employeeFactory = new EmployeeFactory();

        employeeArrays.JsonToList();

        employeeCardObject = uiManager.employeeCardPrefab.GetComponent<EmployeeCard>();
        prospectCardObject = uiManager.prospectCardPrefab.GetComponent<ProspectCard>();
        freeAgentCardObject = uiManager.freeAgentCardPrefab.GetComponent<FreeAgentCard>();
        retirementCardObject = uiManager.retiringEmployeeCardPrefab.GetComponent<RetirementCard>();
        tradeAssetCardObject = uiManager.tradeAssetForEmployeeCardPrefab.GetComponent<TradeAssetCard>();

        CreateAnEmployee(rosterCount, employeeFactory, employeeLists, employeeArrays, employeeLists.currentRoster, employeeCardObject, uiManager.rosterGridStorage.transform);

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

        uiManager.emptyListText.text = string.Empty;

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
                CheckforEmptyList(employeeLists.retiringEmployees, $"No Retirements in {generalManager.currentYear}");

                uiManager.ChangeUI(uiManager.retirementScreen);
                break;

            case Period.ExpiringContracts:
                employeeLists.retiringEmployees.Clear();

                CheckExpiringContracts();
                CheckforEmptyList(employeeLists.pendingFreeAgents, $"No Expiring Contracts in {generalManager.currentYear}");

                uiManager.ChangeUI(uiManager.expiringContractsScreen);
                break; 

            case Period.FreeAgency:
                CreateAnEmployee(freeAgencyClassSize, employeeFactory, employeeLists, employeeArrays, employeeLists.freeAgentClass, freeAgentCardObject, uiManager.freeAgencyContent);
                
                LetExpiringContractsWalk();

                uiManager.ChangeUI(uiManager.freeAgencyScreen);
                break;

            case Period.Trading:
                employeeLists.freeAgentClass.Clear();

                CreateAnEmployee(tradeManager.tradeBlockSize, employeeFactory, employeeLists, employeeArrays, employeeLists.tradeBlock, tradeAssetCardObject, uiManager.tradeBlockContent);
                
                uiManager.ChangeUI(uiManager.tradingScreen);
                break;

            case Period.EmployeeEvents:
                employeeLists.tradeBlock.Clear();
                ReturnTradeAssets();                
                
                CheckForEmployeeEvent();
                CheckforEmptyList(employeeLists.disgruntledEmployees, $"No Disgruntled Employees in {generalManager.currentYear}");
                
                uiManager.ChangeUI(uiManager.disgruntlementsScreen);
                break;

            case Period.Draft:
                employeeLists.disgruntledEmployees.Clear();

                draftManager.currentRound = 1;
                uiManager.nextPeriodButton.SetActive(false);
                CreateAnEmployee(draftManager.draftClassSize, employeeFactory, employeeLists, employeeArrays, employeeLists.draftClass, prospectCardObject, uiManager.prospectContent);

                uiManager.ChangeUI(uiManager.draftScreen);
                break;

            case Period.SeasonSimulation:
                draftManager.latestDraftClass.Clear();
                draftManager.latestDraftClassRoundSelected.Clear();

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
            employeeFactory.CreateEmployee(employeeLists, employeeArrays, faceManager, listToAddTo);
        }

        foreach (var employee in listToAddTo)
        {
            GameObject cardObject = Instantiate(employeeCard.gameObject, layout);
            EmployeeCard cardInstance = cardObject.GetComponent<EmployeeCard>();

            cardInstance.GetEmployeeStats(employee);
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

    // We could make this stat increases serialized fields if needed
    // Possibly with a Season Reflection Manager?
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

        if (employee.age <= ageOfRegression)
        {
            for (int i = 0; i < 5; i++)
            {
                int randomStatIncrease = Random.Range(minStatsIncrease, maxStatsIncrease);
                statIncreases.Add(randomStatIncrease);
            }

            if (employee.efficiency + statIncreases[0] > 100) employee.efficiency = 100;
            else employee.efficiency += statIncreases[0];

            if (employee.customerService + statIncreases[1] > 100) employee.customerService = 100;
            else employee.customerService += statIncreases[1];

            if (employee.communication + statIncreases[2] > 100) employee.communication = 100;
            else employee.communication += statIncreases[2];

            if (employee.teamwork + statIncreases[3] > 100) employee.teamwork = 100;
            else employee.teamwork += statIncreases[3];

            if (employee.iq + statIncreases[4] > 100) employee.iq = 100;
            else employee.iq += statIncreases[4];

            statIncreases.Clear();
        }
        else
        {
            employee.efficiency -= amountOfRegressionPerStat;
            employee.customerService -= amountOfRegressionPerStat;
            employee.communication -= amountOfRegressionPerStat;
            employee.teamwork -= amountOfRegressionPerStat;
            employee.iq -= amountOfRegressionPerStat;

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

    public void CheckforEmptyList(List<Employee> listToCheck, string emptyAction)
    {
        if (listToCheck.Count == 0)
            uiManager.emptyListText.text = emptyAction;
    }

    private void CheckforRetirement()
    {
        int percentChanceOfRetirement = 0;

        foreach (var employee in employeeLists.currentRoster.ToList())
        {
            int randomNumber = Random.Range(1, 101);

            switch (employee.age)
            {
                case 35: percentChanceOfRetirement = 21; break; // 20% Chance
                case 36: percentChanceOfRetirement = 31; break; // 30% Chance
                case 37: percentChanceOfRetirement = 41; break; // 40% Chance
                case 38: percentChanceOfRetirement = 61; break; // 60% Chance
                case 39: percentChanceOfRetirement = 81; break; // 80% Chance
                case 40: percentChanceOfRetirement = 101; break; // 100% Chance
            }

            if (randomNumber < percentChanceOfRetirement)
            {
                employeeLists.AddEmployee(employee, employeeLists.retiringEmployees);
                employeeLists.RemoveEmployee(employee, employeeLists.currentRoster);
            }
        }
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

    private void ReturnTradeAssets()
    {
        // If you advance a period and there is employees left in the transaction screen, they will be returned to the user's current roster
        foreach (var Employee in tradeManager.outgoingEmployees.ToList())
        {
            employeeLists.AddEmployee(Employee, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(Employee, tradeManager.outgoingEmployees);
        }

        employeeLists.tradeBlock.Clear();
        tradeManager.outgoingDraftPicks.Clear();
        tradeManager.outgoingTradePackageValue.Clear();
        tradeManager.employeeToBeAcquired = null;
    }

    private void CheckForEmployeeEvent()
    {
        int percentChanceOfDisgruntlement = 0;

        foreach (var employee in employeeLists.currentRoster.ToList())
        {
            int randomNumber = Random.Range(1, 101);

            switch (employee.personalityTrait)
            {
                case EmployeeEnumerators.PersonalityTrait.Toxic: percentChanceOfDisgruntlement = 76; break; // 75% Chance
                case EmployeeEnumerators.PersonalityTrait.Diva: percentChanceOfDisgruntlement = 41; break; // 40% Chance
                case EmployeeEnumerators.PersonalityTrait.Difficult: percentChanceOfDisgruntlement = 26; break; // 25% Chance
                case EmployeeEnumerators.PersonalityTrait.Team_Player: percentChanceOfDisgruntlement = 16; break; // 15% Chance
                case EmployeeEnumerators.PersonalityTrait.Saint: percentChanceOfDisgruntlement = 6; break; // 5% Chance
                case EmployeeEnumerators.PersonalityTrait.Perfectionist: percentChanceOfDisgruntlement = 1; break; // 0% Chance 
            }

            if (randomNumber < percentChanceOfDisgruntlement)
            {
                employeeLists.AddEmployee(employee, employeeLists.disgruntledEmployees);
                employeeLists.RemoveEmployee(employee, employeeLists.currentRoster);
            }
        } 
    }
    #endregion
}
