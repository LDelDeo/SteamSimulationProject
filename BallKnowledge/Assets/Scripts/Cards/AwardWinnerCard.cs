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
        Employee awardWinner = this.gameObject.GetComponent<AwardWinnerCard>().awardWinner;

        employeeLists.UpgradeEmployeeOverall(awardWinner, awardManager.ovrUpgradeAmountTeamAward);

        GameObject cardObject = Instantiate(uiManager.awardWinnerCardPrefab, uiManager.awardWinnersContent);
        AwardWinnerCard card = cardObject.GetComponent<AwardWinnerCard>();

        card.GetEmployeeStats(awardWinner);
        card.SetEmployeeCardBackground(awardWinner);
        card.awardWonText.text = $"{generalManager.currentYear} Management Thank You Award";
        card.prizeWonText.text = $"+{awardManager.ovrUpgradeAmountTeamAward} Overall & {GetCornyGift()}";

        uiManager.showEmployeesToNominateButton.interactable = false;

        uiManager.SetScrollContent(uiManager.awardsScreen, uiManager.awardWinnersContent);
        uiManager.ClearContent(uiManager.employeesToNominateContent);

        uiManager.employeesToNominateContent.gameObject.SetActive(false);
        uiManager.awardWinnersContent.gameObject.SetActive(true);
    }

    private string GetCornyGift()
    {
        var randomNumber = Random.Range(0, 9);
        string cornyGift = string.Empty;

        switch (randomNumber)
        {
            case 0: cornyGift = "a box of pens"; break;
            case 1: cornyGift = "a thank you sticker"; break;
            case 2: cornyGift = "a company logo pin"; break;
            case 3: cornyGift = "a paid day off"; break;
            case 4: cornyGift = "a company themed plastic water bottle"; break;
            case 5: cornyGift = "a fidget toy"; break;
            case 6: cornyGift = "a company themed lanyard"; break;
            case 7: cornyGift = "a company themed keychain"; break;
            case 8: cornyGift = "a couple breath mints"; break;
        }

        return cornyGift;
    }
    #endregion
}
