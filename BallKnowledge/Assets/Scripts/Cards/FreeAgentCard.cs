using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeAgentCard : EmployeeCard
{
    #region Free Agent Visuals
    [Header("Free Agent Card Visuals")]
    [SerializeField] TMP_Text ageText;
    [SerializeField] TMP_Text requestedWageText;
    [SerializeField] TMP_Text contractYearsText;
    #endregion

    private string ageValue;
    private int requestedWageValue;
    private int contractYears = 1;

    private Employee freeAgent;

    public override void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        employeePosition = employee.jobPosition.ToString();
        employeeOverall = employee.overall.ToString();

        ageValue = employee.age.ToString();
        requestedWageValue = employee.hourlyWage;

        SetStats();
        SetEmployeeCardBackground(employee);
        GrabEmployee(employee);
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        positionText.text = employeePosition;
        overallText.text = employeeOverall;

        ageText.text = $"Age: {ageValue}";
        requestedWageText.text = $"${requestedWageValue}/hr";
    }

    #region Signing Functionality
    private void GrabEmployee(Employee employee)
    {
        freeAgent = employee;
    }

    public void SignPlayer(FreeAgentCard freeAgentCard)
    {
        Employee freeAgentToSign = freeAgentCard.freeAgent;

        if ((manager.currentUsedCapSpace + freeAgentToSign.hourlyWage) < manager.maxCapSpace && employeeLists.HasRosterSpace(freeAgentToSign))
        {
            freeAgentToSign.yearsUnderContract = contractYears;

            employeeLists.AddEmployee(freeAgentToSign, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(freeAgentToSign, employeeLists.freeAgentClass);

            uiManager.RefreshUI();
        }
    }

    public void ResignPlayer(FreeAgentCard expiringContractCard)
    {
        Employee employeeToResign = expiringContractCard.freeAgent;
        //int minWage = freeAgent.value * 2;
        //int maxWage = freeAgent.value * 4;

        //var requestedWage = Random.Range(minWage, maxWage);
        //employeeToResign.hourlyWage = requestedWage;

        if ((manager.currentUsedCapSpace + employeeToResign.hourlyWage) < manager.maxCapSpace && employeeLists.HasRosterSpace(employeeToResign))
        {
            employeeToResign.yearsUnderContract = contractYears;

            employeeLists.AddEmployee(employeeToResign, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(employeeToResign, employeeLists.pendingFreeAgents);

            uiManager.RefreshUI();
        }
    }

    public void AdjustContractYears(bool increase)
    {
        if (increase) 
        { 
            contractYears++;
            if (contractYears > 7) { contractYears = 1; }
        }
        else if (!increase) 
        { 
            contractYears--;
            if (contractYears < 1) { contractYears = 7; }
        }

        contractYearsText.text = contractYears.ToString();
    }
    #endregion
}
