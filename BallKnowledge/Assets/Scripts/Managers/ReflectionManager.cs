using System.Collections.Generic;
using UnityEngine;

public class ReflectionManager : MonoBehaviour
{
    #region Stat Configuaration
    [Header("Bum Progression")]
    [SerializeField] int minBumProgression;
    [SerializeField] int maxBumProgression;

    [Header("Lazy Progression")]
    [SerializeField] int minLazyProgression;
    [SerializeField] int maxLazyProgression;

    [Header("Paycheck Collector Progression")]
    [SerializeField] int minPaycheckCollectorProgression;
    [SerializeField] int maxPayCheckCollectorProgression;

    [Header("Gets The Job Done Progression")]
    [SerializeField] int minGetsTheJobDoneProgression;
    [SerializeField] int maxGetsTheJobDoneProgression;

    [Header("Motivated Progression")]
    [SerializeField] int minMotivatedProgression;
    [SerializeField] int maxMotivatedProgression;

    [Header("Grinder Progression")]
    [SerializeField] int minGrinderProgression;
    [SerializeField] int maxGrinderProgression;

    [Header("X-Factor Progression")]
    [SerializeField] int minXFactorProgression;
    [SerializeField] int maxXFactorProgression;

    [Header("Employee Regression")]
    [SerializeField] int statRegression;
    #endregion

    #region Script References
    private EmployeeLists employeeLists;
    private PeriodManager periodManager;
    private UIManager uiManager;
    #endregion

    private void Awake()
    {
        employeeLists = GetComponent<EmployeeLists>();
        periodManager = GetComponent<PeriodManager>();
        uiManager = GetComponent<UIManager>();
    }

    public void NaturalEmployeeStatChange()
    {
        foreach (var employee in employeeLists.currentRoster)
            UpdateEmployeeStats(employee);
    }

    private void UpdateEmployeeStats(Employee employee)
    {
        List<int> statIncreases = new List<int>();
        int minStatsIncrease = 0;
        int maxStatsIncrease = 0;

        switch (employee.workEthic)
        {
            case EmployeeEnumerators.WorkEthic.Bum:
                minStatsIncrease = minBumProgression;
                maxStatsIncrease = maxBumProgression;
                break;

            case EmployeeEnumerators.WorkEthic.Lazy:
                minStatsIncrease = minLazyProgression;
                maxStatsIncrease = maxLazyProgression;
                break;

            case EmployeeEnumerators.WorkEthic.Paycheck_Collector:
                minStatsIncrease = minPaycheckCollectorProgression;
                maxStatsIncrease = maxPayCheckCollectorProgression;
                break;

            case EmployeeEnumerators.WorkEthic.Gets_The_Job_Done:
                minStatsIncrease = minGetsTheJobDoneProgression;
                maxStatsIncrease = maxGetsTheJobDoneProgression;
                break;

            case EmployeeEnumerators.WorkEthic.Motivated:
                minStatsIncrease = minMotivatedProgression;
                maxStatsIncrease = maxMotivatedProgression;
                break;

            case EmployeeEnumerators.WorkEthic.Grinder:
                minStatsIncrease = minGrinderProgression;
                maxStatsIncrease = maxGrinderProgression;
                break;

            case EmployeeEnumerators.WorkEthic.X_Factor:
                minStatsIncrease = minXFactorProgression;
                maxStatsIncrease = maxXFactorProgression;
                break;
        }

        if (employee.age <= periodManager.ageOfRegression)
        {
            for (int i = 0; i < 5; i++)
            {
                int randomStatIncrease = Random.Range(minStatsIncrease, maxStatsIncrease);
                statIncreases.Add(randomStatIncrease);
            }

            if (employee.efficiency + statIncreases[0] > employeeLists.maxEmployeeStat) employee.efficiency = employeeLists.maxEmployeeStat;
            else employee.efficiency += statIncreases[0];

            if (employee.customerService + statIncreases[1] > employeeLists.maxEmployeeStat) employee.customerService = employeeLists.maxEmployeeStat;
            else employee.customerService += statIncreases[1];

            if (employee.communication + statIncreases[2] > employeeLists.maxEmployeeStat) employee.communication = employeeLists.maxEmployeeStat;
            else employee.communication += statIncreases[2];

            if (employee.teamwork + statIncreases[3] > employeeLists.maxEmployeeStat) employee.teamwork = employeeLists.maxEmployeeStat;
            else employee.teamwork += statIncreases[3];

            if (employee.iq + statIncreases[4] > employeeLists.maxEmployeeStat) employee.iq = employeeLists.maxEmployeeStat;
            else employee.iq += statIncreases[4];

            statIncreases.Clear();
        }
        else
        {
            employee.efficiency -= statRegression;
            employee.customerService -= statRegression;
            employee.communication -= statRegression;
            employee.teamwork -= statRegression;
            employee.iq -= statRegression;

            if (employee.efficiency < employeeLists.minEmployeeStat)
                employee.efficiency = employeeLists.minEmployeeStat;

            if (employee.customerService < employeeLists.minEmployeeStat)
                employee.customerService = employeeLists.minEmployeeStat;

            if (employee.communication < employeeLists.minEmployeeStat)
                employee.communication = employeeLists.minEmployeeStat;

            if (employee.teamwork < employeeLists.minEmployeeStat)
                employee.teamwork = employeeLists.minEmployeeStat;

            if (employee.iq < employeeLists.minEmployeeStat)
                employee.iq = employeeLists.minEmployeeStat;
        }

        employee.overall = (employee.efficiency +
                            employee.customerService +
                            employee.communication +
                            employee.teamwork +
                            employee.iq)
                            / 5;

        foreach (GameObject upgradeCard in uiManager.employeeUpgradesContent)
        {
            var employeeToUpdate = upgradeCard.GetComponent<UpgradeCard>().upgradedEmployee;

            if (employeeToUpdate == employee)
            {
                if (employeeToUpdate.age >= periodManager.ageOfRegression)
                    upgradeCard.GetComponent<UpgradeCard>().SetEmployeeUpgrades(statRegression, statRegression, statRegression, statRegression, statRegression);
                else
                    upgradeCard.GetComponent<UpgradeCard>().SetEmployeeUpgrades(statIncreases[0], statIncreases[1], statIncreases[2], statIncreases[3], statIncreases[4]);
            }
                
        }
            
    }
}
