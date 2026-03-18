using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProspectCard : EmployeeCard
{
    [Header("Prospect Stats")]
    public bool developmentTraitRevealed;
    public bool overallRevealed;
    public EmployeeEnumerators.PersonalityTrait personalityTrait;
    public EmployeeEnumerators.JobType jobType;

    private int amountOfVisibleStats;

    private Employee prospect;

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
        ageText.text = $"Age: {employeeAge}";
        personalityText.text = $"Personality: {employeePersonalityTrait}";

        amountOfVisibleStats = 0;

        IsStatVisible(efficiencyText, employeeEfficiency);
        IsStatVisible(customerServiceText, employeeCustomerService);
        IsStatVisible(communicationText, employeeCommunication);
        IsStatVisible(teamworkText, employeeTeamwork);
        IsStatVisible(iqText, employeeIq);

        RevealDevelopmentTrait();
        RevealOverall();     

        personalityTrait = employeePersonalityTrait;
        jobType = employeeJobPosition;
    }

    #region Draft Stats
    private void IsStatVisible(TMP_Text statText, int statValue)
    {
        int randomNumber = UnityEngine.Random.Range(0, 2);

        if (randomNumber == 0) 
        { 
            statText.text = statValue.ToString();
            amountOfVisibleStats++;
        }
        else if (randomNumber == 1) { statText.text = "?"; }   
    }

    private void RevealDevelopmentTrait()
    {
        if (amountOfVisibleStats == 0) 
        { 
            workEthicText.text = $"Work Ethic: {employeeWorkEthic}";
            developmentTraitRevealed = true;
        }
        else { workEthicText.text = "Work Ethic: ?"; }
    }

    private void RevealOverall()
    {
        if (amountOfVisibleStats == 5)
        {
            overallText.text = $"Overall: {employeeOverall}";
            overallRevealed = true;
            amountOfVisibleStats = 0;
        }
        else
        {
            overallText.text = "Overall: ?";
            amountOfVisibleStats = 0;
        }
    }
    #endregion

    #region Drafting Functionality
    private void GrabEmployee(Employee employee)
    {
        prospect = employee;
    }

    public void DraftPlayer(ProspectCard prospectCard)
    {
        Employee prospectToDraft = prospectCard.prospect;
        
        switch (draftManager.currentRound)
        {
            case 1:
                if (generalManager.firstRoundPicks > 0 && employeeLists.HasRosterSpace(prospectToDraft) && employeeLists.HasCapSpaceToCompleteTransaction(prospectToDraft))
                {
                    employeeLists.AddEmployee(prospectToDraft, draftManager.latestDraftClass);
                    prospectToDraft.methodOfAcquirement = $"{generalManager.currentYear} first round pick";

                    employeeLists.AddEmployee(prospectToDraft, employeeLists.currentRoster);
                    employeeLists.RemoveEmployee(prospectToDraft, employeeLists.draftClass);

                    generalManager.firstRoundPicks--;
                }
                else if (generalManager.firstRoundPicks < 1) { uiManager.InsufficientDraftPicks("first"); }
                else if (!employeeLists.HasRosterSpace(prospectToDraft)) { uiManager.InsufficientRosterSpace(prospectToDraft); }
                else if (!employeeLists.HasCapSpaceToCompleteTransaction(prospectToDraft)) { uiManager.InsufficientCapRoom(prospectToDraft); }
                break;
         
            case 2:
                if (generalManager.secondRoundPicks > 0 && employeeLists.HasRosterSpace(prospectToDraft))
                {
                    employeeLists.AddEmployee(prospectToDraft, draftManager.latestDraftClass);
                    prospectToDraft.methodOfAcquirement = $"{generalManager.currentYear} second round pick";

                    employeeLists.AddEmployee(prospectToDraft, employeeLists.currentRoster);
                    employeeLists.RemoveEmployee(prospectToDraft, employeeLists.draftClass);

                    generalManager.secondRoundPicks--;
                }
                else if (generalManager.secondRoundPicks < 1) { uiManager.InsufficientDraftPicks("second"); }
                else if (!employeeLists.HasRosterSpace(prospectToDraft)) { uiManager.InsufficientRosterSpace(prospectToDraft); }
                else if (!employeeLists.HasCapSpaceToCompleteTransaction(prospectToDraft)) { uiManager.InsufficientCapRoom(prospectToDraft); }
                break;
       
            case 3:
                if (generalManager.thirdRoundPicks > 0 && employeeLists.HasRosterSpace(prospectToDraft))
                {
                    employeeLists.AddEmployee(prospectToDraft, draftManager.latestDraftClass);
                    prospectToDraft.methodOfAcquirement = $"{generalManager.currentYear} third round pick";

                    employeeLists.AddEmployee(prospectToDraft, employeeLists.currentRoster);
                    employeeLists.RemoveEmployee(prospectToDraft, employeeLists.draftClass);

                    generalManager.thirdRoundPicks--;
                }
                else if (generalManager.thirdRoundPicks < 1) { uiManager.InsufficientDraftPicks("third"); }
                else if (!employeeLists.HasRosterSpace(prospectToDraft)) { uiManager.InsufficientRosterSpace(prospectToDraft); }
                else if (!employeeLists.HasCapSpaceToCompleteTransaction(prospectToDraft)) { uiManager.InsufficientCapRoom(prospectToDraft); }
                break;
        }

        generalManager.playersDrafted++;

        uiManager.RefreshUI();
    }

    public void RemoveProspect(ProspectCard prospectCard)
    {
        if (!employeeLists.draftClass.Contains(prospectCard.prospect))
            Destroy(prospectCard.gameObject);
    }
    #endregion
}
