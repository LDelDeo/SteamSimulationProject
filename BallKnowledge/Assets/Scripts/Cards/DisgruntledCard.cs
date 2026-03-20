using NUnit.Framework;
using System.Security.Policy;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TradeManager;

public class DisgruntledCard : EmployeeCard
{
    [Header("Disgrunted Visuals")]
    [SerializeField] TMP_Text scenarioText;
    [SerializeField] GameObject[] buttons; // 0 = Release, 1 = Trade, 2 = Raise, 3 = Extend, 4 = Convince to Stay

    private int raiseAmount;
    private int yearExtensionAmount;
    private string reasonOfArrest;

    private Employee disgruntledEmployee;

    public override void GetEmployeeStats(Employee employee)
    {
        base.GetEmployeeStats(employee);

        SetStats();
        GrabEmployee(employee);

        foreach (var button in buttons)
            button.SetActive(false);

        SelectScenario();
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        jobPositionText.text = employeeJobPosition.ToString();
        personalityText.text = employeePersonalityTrait.ToString();
        overallText.text = $"Overall: {employeeOverall}";
        ageText.text = $"Age: {employeeAge}";
        hourlyWageText.text = $"Wage: {employeeHourlyWage}/hr";
        yearsUnderContractText.text = $"{employeeYearsUnderContract} Year(s) Remaining";
    }

    #region Disgruntlement Functionality
    private void GrabEmployee(Employee employee)
    {
        disgruntledEmployee = employee;
    }

    private void SelectScenario()
    {
        int randomNumber = Random.Range(0, 4);
        SetScenario(randomNumber);
    }

    private void SetScenario(int scenarioSelected)
    {
        switch (scenarioSelected)
        {
            case 0: ScenarioOne(); break;
            case 1: ScenarioTwo(); break;
            case 2: ScenarioThree(); break;
            case 3: ScenarioFour(); break;
        }
    }

    private void ScenarioOne()
    {
        int randomNumber = Random.Range(1, 16);
        raiseAmount = randomNumber;

        scenarioText.text = $"{employeeFirstName} will not work until they're given a {raiseAmount}/hr raise";

        buttons[2].SetActive(true); // Raise
        buttons[0].SetActive(true); // Release
        buttons[1].SetActive(true); // Trade
    }

    private void ScenarioTwo()
    {
        scenarioText.text = $"{employeeFirstName} has become increasinly unhappy with their role, they're demanding a trade from the franchise";

        buttons[0].SetActive(true); // Release
        buttons[1].SetActive(true); // Trade
        buttons[4].SetActive(true); // Convince to Stay
    }

    private void ScenarioThree()
    {
        int randomNumber = Random.Range(1, 4);
        yearExtensionAmount = randomNumber;

        scenarioText.text = $"{employeeFirstName} will not work until they're given a {yearExtensionAmount} year contract extentsion";

        buttons[3].SetActive(true); // Extend
        buttons[0].SetActive(true); // Release
        buttons[1].SetActive(true); // Trade
    }

    private void ScenarioFour()
    {
        int randomNumber = Random.Range(0, 4);

        switch (randomNumber)
        {
            case 0: reasonOfArrest = "speeding"; break;
            case 1: reasonOfArrest = "performance enchaning substances"; break;
            case 2: reasonOfArrest = "shop lifting"; break;
            case 3: reasonOfArrest = "assault"; break;
        }

        scenarioText.text = $"{employeeFirstName} has been arrested for {reasonOfArrest}. Your customers are demanding an immediate release from the franchise (cancel culture)";

        buttons[0].SetActive(true); // Release
    }

    public void RequestToReleaseEmployee()
    {
        uiManager.AttemptEmployeeRelease(disgruntledEmployee, ReleaseEmployee);
    }

    private void ReleaseEmployee()
    {
        Employee disgruntledEmployee = this.gameObject.GetComponent<DisgruntledCard>().disgruntledEmployee;

        generalManager.playersCut++;

        employeeLists.AddEmployee(disgruntledEmployee, employeeLists.freeAgentClass);
        employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

        uiManager.EmployeeReleased(disgruntledEmployee);
        SettlementClosed("Employee Released");

        disgruntledEmployee.hourlyWage = employeeRNG.GetRandomWage(disgruntledEmployee);
    }

    public void RequestToTradeEmployee()
    {
        Employee disgruntledEmployee = this.gameObject.GetComponent<DisgruntledCard>().disgruntledEmployee;

        if (tradeManager.EmployeeValueInPicks(disgruntledEmployee) == TradeManager.TradePackages.NoTradeInterest)
        {
            uiManager.NameGenericText(disgruntledEmployee, "has generated no trade interest from other fast food franchises");
            buttons[1].SetActive(false);
        }
        else uiManager.AttemptEmployeeTrade(disgruntledEmployee, TradeEmployee, AttemptTradeEmployeeForPicks());
    }

    private string AttemptTradeEmployeeForPicks()
    {
        var tradePackage = string.Empty;

        switch (tradeManager.EmployeeValueInPicks(disgruntledEmployee))
        {
            case TradePackages.NoTradeInterest: tradePackage = "has generated no trade interest from other fast food franchises"; break;
            case TradePackages.ThirdRoundPick: tradePackage = "a third round pick"; break;
            case TradePackages.MultipleThirdRoundPicks: tradePackage = "two third round picks"; break;
            case TradePackages.SecondRoundPick: tradePackage = "a second round pick"; break;
            case TradePackages.MultipleSecondRoundPicks: tradePackage = "two second round picks"; break;
            case TradePackages.FirstRoundPick: tradePackage = "a FIRST round pick"; break;
            case TradePackages.MultipleFirstRoundPicks: tradePackage = "TWO FIRST ROUND PICKS"; break;
        }

        return tradePackage;
    }

    private void TradeEmployee()
    {
        tradeManager.TradeEmployeeForPicks(disgruntledEmployee);
        SettlementClosed("Employee Traded");
    }

    public void RaiseEmployee()
    {
        Employee disgruntledEmployee = this.gameObject.GetComponent<DisgruntledCard>().disgruntledEmployee;

        if (generalManager.currentUsedCapSpace + disgruntledEmployee.hourlyWage + raiseAmount <= generalManager.maxCapSpace)
        {
            if (employeeLists.HasRosterSpace(disgruntledEmployee))
            {
                disgruntledEmployee.hourlyWage += raiseAmount;

                employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
                employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

                uiManager.EmployeeRaise(disgruntledEmployee, raiseAmount, true);
                SettlementClosed("Employee Wage Raised");
            }
        }
        else
        {
            uiManager.EmployeeRaise(disgruntledEmployee, raiseAmount, false);
        }
    }

    public void RequestToExtendEmployee()
    {
        if (disgruntledEmployee.yearsUnderContract + yearExtensionAmount > 4) uiManager.AttemptToExtendEmployee(disgruntledEmployee, ExtendEmployee, yearExtensionAmount);
        else ExtendEmployee();
    }

    private void ExtendEmployee()
    {
        Employee disgruntledEmployee = this.gameObject.GetComponent<DisgruntledCard>().disgruntledEmployee;

        if (employeeLists.HasRosterSpace(disgruntledEmployee))
        {
            disgruntledEmployee.yearsUnderContract += yearExtensionAmount;

            employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

            uiManager.EmployeeExtention(disgruntledEmployee, yearExtensionAmount);
            SettlementClosed("Employee Contract Extended");
        }
    }

    public void ConvinceEmployeeToStay()
    {
        Employee disgruntledEmployee = this.gameObject.GetComponent<DisgruntledCard>().disgruntledEmployee;

        int randomNumber = Random.Range(1, 101);

        // Depending on the personality trait, it's more or less likely they'll want to stay or leave when asked to stay
        switch (disgruntledEmployee.personalityTrait)
        {
            case EmployeeEnumerators.PersonalityTrait.Toxic:
                if (randomNumber < 81)  EmployeeReconsideration(false, disgruntledEmployee); // 80% chance they will not change their mind
                else EmployeeReconsideration(true, disgruntledEmployee);
                break;
            case EmployeeEnumerators.PersonalityTrait.Diva:
                if (randomNumber < 61) EmployeeReconsideration(false, disgruntledEmployee); // 60% chance they will not change their mind
                else EmployeeReconsideration(true, disgruntledEmployee);
                break;
            case EmployeeEnumerators.PersonalityTrait.Difficult:
                if (randomNumber < 41) EmployeeReconsideration(false, disgruntledEmployee); // 40% chance they will not change their mind
                else EmployeeReconsideration(true, disgruntledEmployee);
                break;
            case EmployeeEnumerators.PersonalityTrait.Team_Player:
                if (randomNumber < 21) EmployeeReconsideration(false, disgruntledEmployee); // 20% chance they will not change their mind
                else EmployeeReconsideration(true, disgruntledEmployee);
                break;
            case EmployeeEnumerators.PersonalityTrait.Saint:
                    EmployeeReconsideration(false, disgruntledEmployee); // 100% chance of changing their mind
                break;
        }   
    }

    private void EmployeeReconsideration(bool changedMind, Employee disgruntledEmployee)
    {
        if (changedMind)
        {
            uiManager.NameGenericText(disgruntledEmployee, "has had a change of heart and has decided to stay");
            SettlementClosed("Employee Returning");

            if (employeeLists.HasRosterSpace(disgruntledEmployee))
            {
                employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
                employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);
            }
        }
        else
        {
            uiManager.NameGenericText(disgruntledEmployee, "still wants out of your franchise");
            buttons[4].SetActive(false);
        }
    }

    private void SettlementClosed(string settlementConclusion)
    {
        foreach (var button in buttons)
            button.SetActive(false);

        scenarioText.color = Color.green;
        scenarioText.text = settlementConclusion;

        uiManager.UpdateCapSpace();
        uiManager.UpdateDraftPicks();
    }
    #endregion
}
