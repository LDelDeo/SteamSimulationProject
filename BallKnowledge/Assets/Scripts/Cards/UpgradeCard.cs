using UnityEngine;
using TMPro;

public class UpgradeCard : EmployeeCard
{
    [Header("Upgrade Visuals")]
    public TMP_Text statOneChangeAmount;
    public TMP_Text statTwoChangeAmount;
    public TMP_Text statThreeChangeAmount;
    public TMP_Text statFourChangeAmount;
    public TMP_Text statFiveChangeAmount;

    public Employee upgradedEmployee;

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
        ageText.text = $"Age: {employeeAge}";
        jobPositionText.text = employeeJobPosition.ToString();
        personalityText.text = employeePersonalityTrait.ToString();

        efficiencyText.text = employeeEfficiency.ToString();
        customerServiceText.text = employeeCustomerService.ToString();
        communicationText.text = employeeCommunication.ToString();
        teamworkText.text = employeeTeamwork.ToString();
        iqText.text = employeeIq.ToString();
    }

    #region Upgraded Stats Functionality
    private void GrabEmployee(Employee employee)
    {
        upgradedEmployee = employee;
    }

    public void SetEmployeeUpgrades(int changeOne, int changeTwo, int changeThree, int changeFour, int changeFive)
    {
        statOneChangeAmount.text = changeOne.ToString();
        statTwoChangeAmount.text = changeTwo.ToString();
        statThreeChangeAmount.text = changeThree.ToString();
        statFourChangeAmount.text = changeFour.ToString();
        statFiveChangeAmount.text = changeFive.ToString();

        // Get Overall as well
        // Change Text +/- and color as well
    }
    #endregion
}
