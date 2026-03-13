using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DisgruntledCard : EmployeeCard
{
    [Header("Disgrunted Visuals")]
    [SerializeField] TMP_Text scenarioText;
    [SerializeField] GameObject[] buttons; // 0 = Cut, 1 = Trade, 2 = Raise, 3 = Extend, 4 = Convince to Stay

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
        buttons[0].SetActive(true); // Cut
        buttons[1].SetActive(true); // Trade
    }

    private void ScenarioTwo()
    {
        scenarioText.text = $"{employeeFirstName} has become increasinly unhappy with their role, they're demanding a trade from the franchise";

        buttons[1].SetActive(true); // Trade
        buttons[4].SetActive(true); // Convince to Stay
    }

    private void ScenarioThree()
    {
        int randomNumber = Random.Range(1, 4);
        yearExtensionAmount = randomNumber;

        scenarioText.text = $"{employeeFirstName} will not work until they're given a {yearExtensionAmount} year contract extentsion";

        buttons[3].SetActive(true); // Extend
        buttons[0].SetActive(true); // Cut
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

        buttons[0].SetActive(true); // Cut
    }

    public void CutEmployee(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        manager.playersCut++;

        employeeLists.AddEmployee(disgruntledEmployee, employeeLists.freeAgentClass);
        employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

        uiManager.EmployeeCut(disgruntledEmployee);
        SettlementClosed();

        disgruntledEmployee.hourlyWage = employeeRNG.GetRandomWage(disgruntledEmployee);
    }

    public void TradeEmployee(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        if (tradeManager.EmployeeValueInPicks(disgruntledEmployee) == TradeManager.TradePackages.NoTradeInterest)
        {
            uiManager.NoTradeInterest(disgruntledEmployee);
            buttons[1].SetActive(false);
        }
        else
        {
            tradeManager.TradeEmployeeForPicks(disgruntledEmployee);
            SettlementClosed();
        }
    }

    public void RaiseEmployee(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        if (manager.currentUsedCapSpace + disgruntledEmployee.hourlyWage + raiseAmount <= manager.maxCapSpace)
        {
            if (employeeLists.HasRosterSpace(disgruntledEmployee))
            {
                disgruntledEmployee.hourlyWage += raiseAmount;

                employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
                employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

                uiManager.EmployeeRaise(disgruntledEmployee, raiseAmount, true);
                SettlementClosed();
            }
        }
        else
        {
            uiManager.EmployeeRaise(disgruntledEmployee, raiseAmount, false);
        }
    }

    public void ExtendEmployee(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        if (employeeLists.HasRosterSpace(disgruntledEmployee))
        {
            disgruntledEmployee.yearsUnderContract += yearExtensionAmount;

            employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

            uiManager.EmployeeExtention(disgruntledEmployee, yearExtensionAmount);
            SettlementClosed();
        }
    }

    public void ConvinceEmployeeToStay(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

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
            uiManager.EmployeeStaying(disgruntledEmployee);
            SettlementClosed();

            if (employeeLists.HasRosterSpace(disgruntledEmployee))
            {
                employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
                employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);
            }
        }
        else
        {
            uiManager.EmployeeWantsOut(disgruntledEmployee);
            buttons[4].SetActive(false);
        }
    }

    private void SettlementClosed()
    {
        foreach (var button in buttons)
            button.SetActive(false);

        scenarioText.color = Color.green;
        scenarioText.text = "SETTLED";

        uiManager.UpdateCapSpace();
        uiManager.UpdateDraftPicks();
    }
    #endregion
}
