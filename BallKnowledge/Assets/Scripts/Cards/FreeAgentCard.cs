using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeAgentCard : EmployeeCard
{
    [Header("Contract Negotiation's Visuals")]
    [SerializeField] TMP_Text contractYearsText;
    [SerializeField] Image interestBar;
    [SerializeField] Button signButton;

    [Header("Free Agent Configuration")]
    [SerializeField] bool isRestrictedFreeAgent;
    [SerializeField] float maxInterestMultiplier;
    [SerializeField] float minInterestMultiplier;

    private int contractYears = 1;
    private float interestInSigning;
    private int interestStandard;
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
        SetInitialInterestInSigning();
    }

    #region Signing Functionality
    private void GrabEmployee(Employee employee)
    {
        freeAgent = employee;
    }

    public void SetInitialInterestInSigning()
    {
        // The employee's initial interest in signing is based off the current roster's overall. The higher the overall, the higher their interest can be
        var maxInterestInSigning = 0f;
        var minInterestInSigning = 0f;

        // If the employee is an expiring contract, it will grab the overall before all the contracts expired
        // This prevents employees not wanting to resign if the free agent class gets very large
        if (isRestrictedFreeAgent) 
        {
            var preExpiringContractOverall = 0;

            foreach (Employee employee in employeeLists.pendingFreeAgents)
                preExpiringContractOverall += employee.overall;

            maxInterestInSigning = ((preExpiringContractOverall + employeeLists.GetRosterOverall()) * maxInterestMultiplier) / employeeLists.rosterConstruction.GetMaxEmployees();
            minInterestInSigning = ((preExpiringContractOverall + employeeLists.GetRosterOverall()) * minInterestMultiplier) / employeeLists.rosterConstruction.GetMaxEmployees();
        }
        else
        {
            maxInterestInSigning = employeeLists.GetRosterOverall() * maxInterestMultiplier;
            minInterestInSigning = employeeLists.GetRosterOverall() * minInterestMultiplier;
        }

        if (maxInterestInSigning > 100)
            maxInterestInSigning = 100;

        if (minInterestInSigning > 100)
            minInterestInSigning = 100;
        else if (minInterestInSigning < 0)
            minInterestInSigning = 0;

        if (maxInterestInSigning <= minInterestInSigning)
            maxInterestInSigning = minInterestInSigning + 20;

        interestInSigning = Random.Range(minInterestInSigning, maxInterestInSigning);

        uiManager.LoadFreeAgentInterestBar(interestBar, interestInSigning);

        // The free agent's interest must be higher than this standard for them to accept the contract
        interestStandard = Random.Range(1, 101);

        UpdateInterestDecsion();
    }

    private void UpdateInterestDecsion()
    {
        if (interestStandard > interestInSigning)
            willSign = false;
        else
            willSign = true;
    }

    public void SignPlayer(FreeAgentCard freeAgentCard)
    {
        Employee freeAgentToSign = freeAgentCard.freeAgent;

        if (!willSign)
        {
            uiManager.NameGenericText(freeAgentToSign, "is not interested in signing with your franchise");
            signButton.interactable = false;
            return;
        }    
        else
        {
            if (employeeLists.HasCapSpaceToCompleteTransaction(freeAgentToSign))
            {
                if (employeeLists.HasRosterSpace(freeAgentToSign))
                {
                    freeAgentToSign.yearsUnderContract = contractYears;
                    freeAgentToSign.methodOfAcquirement = $"{manager.currentYear} Free Agency Class";

                    employeeLists.AddEmployee(freeAgentToSign, employeeLists.currentRoster);
                    employeeLists.RemoveEmployee(freeAgentToSign, employeeLists.freeAgentClass);

                    uiManager.EmployeeSigningContract(freeAgentToSign);

                    uiManager.RefreshUI();
                }
                else if (!employeeLists.HasRosterSpace(freeAgentToSign)) { uiManager.InsufficientRosterSpace(freeAgentToSign); }
            }
            else if (!employeeLists.HasCapSpaceToCompleteTransaction(freeAgentToSign)) { uiManager.InsufficientCapRoom(freeAgentToSign); }
        }    
    }

    public void ResignPlayer(FreeAgentCard expiringContractCard)
    {
        Employee employeeToResign = expiringContractCard.freeAgent;

        if (!willSign)
        {
            uiManager.NameGenericText(employeeToResign, "is not interested in signing with your franchise");
            signButton.interactable = false;
            return;
        }
        else
        {
            if (employeeLists.HasCapSpaceToCompleteTransaction(employeeToResign))
            {
                if (employeeLists.HasRosterSpace(employeeToResign))
                {
                    employeeToResign.yearsUnderContract = contractYears;

                    employeeLists.AddEmployee(employeeToResign, employeeLists.currentRoster);
                    employeeLists.RemoveEmployee(employeeToResign, employeeLists.pendingFreeAgents);

                    uiManager.EmployeeSigningContract(employeeToResign);

                    uiManager.RefreshUI();
                }
                else if (!employeeLists.HasRosterSpace(employeeToResign)) { uiManager.InsufficientRosterSpace(employeeToResign); }
            }
            else if (!employeeLists.HasCapSpaceToCompleteTransaction(employeeToResign)) { uiManager.InsufficientCapRoom(employeeToResign); }
        } 
    }

    public void AdjustContractWage(bool increase)
    {
        if (increase) // Increasing the money offered to a free agent will make them more interested in signing and more likely they will sign
        {
            freeAgent.hourlyWage++;
            interestInSigning++;
        }
        else if (!increase && freeAgent.hourlyWage > 1) // Decreasing the money offered to a free agent will make them less interested in signing and less likely they will sign
        {
            freeAgent.hourlyWage--;
            interestInSigning--;
        }

        uiManager.LoadFreeAgentInterestBar(interestBar, interestInSigning);
        hourlyWageText.text = $"{freeAgent.hourlyWage}/hr";
        UpdateInterestDecsion();
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

    public void RemoveFreeAgent(FreeAgentCard freeAgentCard)
    {
        if (!employeeLists.freeAgentClass.Contains(freeAgentCard.freeAgent))
            Destroy(freeAgentCard.gameObject);
    }

    public void RemoveUnrestrictedFreeAgent(FreeAgentCard freeAgentCard)
    {
        if (!employeeLists.pendingFreeAgents.Contains(freeAgentCard.freeAgent))
            Destroy(freeAgentCard.gameObject);
    }
    #endregion
}
