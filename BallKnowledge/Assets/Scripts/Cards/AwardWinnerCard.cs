using UnityEngine;
using TMPro;

public class AwardWinnerCard : EmployeeCard
{
    [Header("Award Winner Visuals")]
    public TMP_Text awardWonText;
    public TMP_Text prizeWonText;

    [Header("Nomination Visuals")]
    public GameObject nominationButton;

    [Header("Award Winner Configuration")]
    [SerializeField] bool isAwardWinner;
    
    private Employee awardWinner;

    private void Start()
    {
        if (!isAwardWinner) nominationButton.SetActive(true);
        else nominationButton.SetActive(false);
    }

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
        isRookieText.text = employeeIsRookie ? "Rookie" : "Veteran";
        jobPositionText.text = employeeJobPosition.ToString();
        personalityText.text = employeePersonalityTrait.ToString();
        methodOfAcquirementText.text = employeeMethodOfAcquirement;
        overallText.text = $"Overall: {employeeOverall}";
        ageText.text = $"Age: {employeeAge}";
        hourlyWageText.text = $"Wage: {employeeHourlyWage}/hr";
        yearsUnderContractText.text = $"{employeeYearsUnderContract} Year(s) Remaining";

        awardWonText.text = string.Empty;
        prizeWonText.text = string.Empty;
    }

    #region Nomination Functionality
    private void GrabEmployee(Employee employee)
    {
        awardWinner = employee;
    }

    public void NominateEmployeeForTeamAward()
    {
        if (awardWinner.efficiency < 100) awardWinner.efficiency += awardManager.ovrUpgradeAmountTeamAward;
        if (awardWinner.efficiency > 100) awardWinner.efficiency = 100;

        if (awardWinner.customerService < 100) awardWinner.customerService += awardManager.ovrUpgradeAmountTeamAward;
        if (awardWinner.customerService > 100) awardWinner.customerService = 100;

        if (awardWinner.communication < 100) awardWinner.communication += awardManager.ovrUpgradeAmountTeamAward;
        if (awardWinner.communication > 100) awardWinner.communication = 100;
        
        if (awardWinner.teamwork < 100) awardWinner.teamwork += awardManager.ovrUpgradeAmountTeamAward;
        if (awardWinner.teamwork > 100) awardWinner.teamwork = 100;

        if (awardWinner.iq < 100) awardWinner.iq += awardManager.ovrUpgradeAmountTeamAward;
        if (awardWinner.iq > 100) awardWinner.iq = 100;

        awardWonText.text = $"{manager.currentYear} Management Thank You Award";
        prizeWonText.text = $"+{awardManager.ovrUpgradeAmountTeamAward} Overall";

        uiManager.showEmployeesToNominateButton.interactable = false;
        Instantiate(this.gameObject, uiManager.awardWinnersContent);

        uiManager.employeesToNominateContent.gameObject.SetActive(false);

        uiManager.ClearContent(uiManager.employeesToNominateContent);
    }
    #endregion
}
