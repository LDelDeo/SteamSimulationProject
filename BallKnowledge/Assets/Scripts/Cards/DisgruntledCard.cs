using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DisgruntledCard : EmployeeCard
{
    [Header("Disgrunted Visuals")]
    [SerializeField] GameObject actionCanvasPrefab; // Use an action canvas like retirments to handle settlements
    [SerializeField] TMP_Text scenarioText;
    [SerializeField] TMP_Text conclusionText;
    [SerializeField] GameObject[] buttons; // 0 = Cut, 1 = Trade, 2 = Raise, 3 = Extend, 4 = Convince to Stay

    private int raiseAmount;
    private int yearExtentionAmount;
    private string reasonOfArrest;

    private Employee disgruntledEmployee;

    public override void GetEmployeeStats(Employee employee)
    {
        base.GetEmployeeStats(employee);

        SetStats();
        GrabEmployee(employee);
        
        HideButtons();
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
        hourlyWageText.text = $"{employeeHourlyWage}/hr";
    }

    #region Disgruntlement Functionality
    private void GrabEmployee(Employee employee)
    {
        disgruntledEmployee = employee;
    }

    private void HideButtons()
    {
        foreach (var button in buttons)
            button.SetActive(false);
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
            case 0:
                ScenarioOne();
                break;

            case 1:
                ScenarioTwo();
                break;

            case 2:
                ScenarioThree();
                break;

            case 3:
                ScenarioFour();
                break;
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
        yearExtentionAmount = randomNumber;

        scenarioText.text = $"{employeeFirstName} will not work until they're given a {yearExtentionAmount} year contract extentsion";

        buttons[3].SetActive(true); // Extend
        buttons[0].SetActive(true); // Cut
        buttons[1].SetActive(true); // Trade
    }

    private void ScenarioFour()
    {
        int randomNumber = Random.Range(0, 4);

        switch (randomNumber)
        {
            case 0:
                reasonOfArrest = "speeding";
                break;

            case 1:
                reasonOfArrest = "performance enchaning drugs";
                break;

            case 2:
                reasonOfArrest = "shop lifting";
                break;

            case 3:
                reasonOfArrest = "assault";
                break;

        }

        scenarioText.text = $"{employeeFirstName} has been arrested for {reasonOfArrest}. Your customers are demanding a release from the franchise";

        buttons[0].SetActive(true); // Cut
    }

    public void CutEmployee(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        manager.playersCut++;

        employeeLists.AddEmployee(disgruntledEmployee, employeeLists.freeAgentClass);
        employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

        // If the current period is free agency and you cut a player, they won't update their requested wage in the current free agency class,
        // this prevents a glitch where you can keep cutting and signing to get the best possible contract value. It stays consistent through out
        if (periodManager.currentPeriod != PeriodManager.Period.FreeAgency)
            disgruntledEmployee.hourlyWage = employeeRNG.GetRandomWage(disgruntledEmployee);

        HideButtons();
        conclusionText.text = "Employee Cut";
        conclusionText.color = Color.red;
    }

    public void TradeEmployee()
    {

        // Trading logic here, we need to make a draft pick value calculator similar to the employee value calculator
        HideButtons();
    }

    public void RaiseEmployee(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        if (manager.currentUsedCapSpace + disgruntledEmployee.hourlyWage + raiseAmount <= manager.maxCapSpace)
        {
            disgruntledEmployee.hourlyWage += raiseAmount;

            employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

            HideButtons();
            conclusionText.text = "Contract Raised";
            conclusionText.color = Color.green;
        }
    }

    public void ExtendEmployee(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        disgruntledEmployee.yearsUnderContract += yearExtentionAmount;

        employeeLists.AddEmployee(disgruntledEmployee, employeeLists.currentRoster);
        employeeLists.RemoveEmployee(disgruntledEmployee, employeeLists.disgruntledEmployees);

        HideButtons();
        conclusionText.text = "Contract Extended";
        conclusionText.color = Color.green;
    }

    public void ConvinceEmployeeToStay(DisgruntledCard disgruntledCard)
    {
        Employee disgruntledEmployee = disgruntledCard.disgruntledEmployee;

        // Depending on the personality trait, make it more or less likely they'll stay or leave
        int randomNumber = Random.Range(0, 2);

        if (randomNumber == 0) 
        {
            conclusionText.text = "Employee Staying";
            conclusionText.color = Color.green;
            HideButtons();
        }
        else if (randomNumber == 1)
        {
            conclusionText.text = "Employee wants out";
            conclusionText.color = Color.green;
            buttons[4].SetActive(false);
        }    

        HideButtons();
    }
    #endregion
}
