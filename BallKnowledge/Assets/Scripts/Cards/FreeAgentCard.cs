using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeAgentCard : EmployeeCard
{
    [Header("Contract Negotiation's Visuals")]
    [SerializeField] TMP_Text contractYearsText;
    [SerializeField] Image interestBar;
    [SerializeField] Button signButton;

    private int contractYears = 1;
    // For interest in signing, we must destroy the object of the employees we resgined and not clear the entire list and reinstantiate
    // We also must keep in mind, for restricted free agents, we should probably take the overall BEFORE they removed from the current roster
    private int interestInSigning;
    private bool willSign;

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

    private void Start()
    {
        SetInterestInSigning();
    }

    #region Signing Functionality
    private void GrabEmployee(Employee employee)
    {
        freeAgent = employee;
    }

    public void SetInterestInSigning()
    {
        // The employee's interest in signing is based off the current roster's overall, the higher the overall, the higher their interest can be
        var maxInterestInSigning = employeeLists.GetRosterOverall();

        interestInSigning = Random.Range(20, maxInterestInSigning);

        uiManager.LoadFreeAgentInterestBar(interestBar, (float)interestInSigning);

        var randomNumber = Random.Range(1, 101);
        if (randomNumber > interestInSigning)  
            willSign = false;
        else
            willSign = true;
    }

    public void SignPlayer(FreeAgentCard freeAgentCard)
    {
        Employee freeAgentToSign = freeAgentCard.freeAgent;

        if (!willSign)
        {
            uiManager.NotInteretedInSigning(freeAgentToSign);
            signButton.interactable = false;
            return;
        }    
        else
        {
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
