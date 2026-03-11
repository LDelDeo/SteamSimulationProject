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

    public void SignPlayer(FreeAgentCard freeAgentCard) // Maybe make it so not every free agent is interested in signing?
    {
        Employee freeAgentToSign = freeAgentCard.freeAgent;

        if (employeeLists.HasCapSpaceToCompleteTransaction(freeAgentToSign) && employeeLists.HasRosterSpace(freeAgentToSign))
        {
            freeAgentToSign.yearsUnderContract = contractYears;

            employeeLists.AddEmployee(freeAgentToSign, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(freeAgentToSign, employeeLists.freeAgentClass);

            uiManager.RefreshUI();
        }
        else if (!employeeLists.HasCapSpaceToCompleteTransaction(freeAgentToSign)) { uiManager.InsufficientCapRoom(freeAgentToSign); }
        else if (!employeeLists.HasRosterSpace(freeAgentToSign)) { uiManager.InsufficientRosterSpace(freeAgentToSign); }
    }

    public void ResignPlayer(FreeAgentCard expiringContractCard) // Maybe make it so not every free agent is interested in returning?
    {
        Employee employeeToResign = expiringContractCard.freeAgent;

        if (employeeLists.HasCapSpaceToCompleteTransaction(employeeToResign) && employeeLists.HasRosterSpace(employeeToResign))
        {
            employeeToResign.yearsUnderContract = contractYears;

            employeeLists.AddEmployee(employeeToResign, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(employeeToResign, employeeLists.pendingFreeAgents);

            uiManager.RefreshUI();
        }
        else if (!employeeLists.HasCapSpaceToCompleteTransaction(employeeToResign)) { uiManager.InsufficientCapRoom(employeeToResign); }
        else if (!employeeLists.HasRosterSpace(employeeToResign)) { uiManager.InsufficientRosterSpace(employeeToResign); }
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
