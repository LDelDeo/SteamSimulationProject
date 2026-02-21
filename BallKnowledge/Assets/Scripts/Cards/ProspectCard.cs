using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProspectCard : EmployeeCard
{
    #region Prospect Visuals
    [Header("Prospect Card Visuals")]
    [SerializeField] TMP_Text statOne;
    [SerializeField] TMP_Text statTwo;
    [SerializeField] TMP_Text statThree;
    [SerializeField] TMP_Text statFour;
    [SerializeField] TMP_Text statFive;
    [SerializeField] TMP_Text workEthic;
    #endregion

    private int statOneValue;
    private int statTwoValue;
    private int statThreeValue;
    private int statFourValue;
    private int statFiveValue;

    private EmployeeEnumerators.WorkEthic employeeWorkEthic;

    private int amountOfVisibleStats;

    private Employee prospect;

    public override void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        employeePosition = employee.jobPosition.ToString();
        employeeOverall = employee.overall.ToString();

        statOneValue = employee.efficiency;
        statTwoValue = employee.customerService;
        statThreeValue = employee.communication;
        statFourValue = employee.teamwork;
        statFiveValue = employee.iq;

        employeeWorkEthic = employee.workEthic;
        
        SetStats();
        GrabEmployee(employee);
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        positionText.text = employeePosition;

        amountOfVisibleStats = 0;

        IsStatVisible(statOne, statOneValue);
        IsStatVisible(statTwo, statTwoValue);
        IsStatVisible(statThree, statThreeValue);
        IsStatVisible(statFour, statFourValue);
        IsStatVisible(statFive, statFiveValue);

        RevealDevelopmentTrait();
        RevealOverall();     
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
            workEthic.text = $"Work Ethic: {employeeWorkEthic}";
        }
        else
        {
            workEthic.text = "Work Ethic: ?";
        }
    }

    private void RevealOverall()
    {
        if (amountOfVisibleStats == 5)
        {
            overallText.text = $"Overall: {employeeOverall}";
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

        if (manager.draftPicks > 0 && employeeLists.HasRosterSpace(prospectToDraft))
        {
            employeeLists.AddEmployee(prospectToDraft, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(prospectToDraft, employeeLists.draftClass);

            manager.draftPicks--;
            manager.playersDrafted++;

            uiManager.RefreshUI();
        }
    }
    #endregion
}
