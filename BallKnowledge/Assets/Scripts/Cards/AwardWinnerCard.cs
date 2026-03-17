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

    public void NominateEmployeeForTeamAward(AwardWinnerCard awardWinnerCard)
    {
        Employee awardWinner = awardWinnerCard.awardWinner;

        employeeLists.UpgradeEmployeeOverall(awardWinner, awardManager.ovrUpgradeAmountTeamAward);

        // Maybe make a function in UI manager to create a singluar card
        GameObject cardObject = Instantiate(uiManager.awardWinnerCardPrefab, uiManager.awardWinnersContent);
        AwardWinnerCard card = cardObject.GetComponent<AwardWinnerCard>();

        card.GetEmployeeStats(awardWinner);
        card.SetEmployeeCardBackground(awardWinner);
        card.awardWonText.text = $"{manager.currentYear} Management Thank You Award";
        card.prizeWonText.text = $"+{awardManager.ovrUpgradeAmountTeamAward} Overall & a box of pens"; // maybe add corny gifts here

        uiManager.showEmployeesToNominateButton.interactable = false;

        uiManager.SetScrollContent(uiManager.awardsScreen, uiManager.awardWinnersContent);
        uiManager.ClearContent(uiManager.employeesToNominateContent);

        uiManager.employeesToNominateContent.gameObject.SetActive(false);
        uiManager.awardWinnersContent.gameObject.SetActive(true);
    }
    #endregion
}
