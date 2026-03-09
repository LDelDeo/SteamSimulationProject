using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeAgentCard : EmployeeCard
{
    [Header("Contract Negotiation's Visuals")]
    [SerializeField] TMP_Text contractYearsText;
    private int contractYears = 1;

    private Employee freeAgent;

    public override void GetEmployeeStats(Employee employee)
    {
        base.GetEmployeeStats(employee);

        SetStats();
        GrabEmployee(employee);
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        jobPositionText.text = employeeJobPosition.ToString();
        overallText.text = employeeOverall.ToString();
        ageText.text = $"Age: {employeeAge}";
        personalityText.text = $"Personality: {employeePersonalityTrait}";
        hourlyWageText.text = $"${employeeHourlyWage}/hr";
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
