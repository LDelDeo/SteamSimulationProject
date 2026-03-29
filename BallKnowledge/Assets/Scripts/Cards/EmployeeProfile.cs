using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmployeeProfile : EmployeeCard
{
    [Header("Employee Profile Visuals")]
    [SerializeField] Button cutButton;

    private Employee thisEmployee;

    private void Start()
    {
        // You can't cut a rookie employee during the draft they were taken in
        if (thisEmployee.isRookie && periodManager.currentPeriod == PeriodManager.Period.Draft)
            cutButton.interactable = false;
    }

    public override void GetEmployeeStats(Employee employee)
    {
        base.GetEmployeeStats(employee);

        GrabEmployee(employee);
        SetStats();
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        genderText.text = employeeGender.ToString();
        ageText.text = $"{employeeAge} y/o";
        isRookieText.text = employeeIsRookie ? "Rookie" : "Veteran";
        jobPositionText.text = employeeJobPosition.ToString();
        workEthicText.text = employeeWorkEthic.ToString();
        personalityText.text = employeePersonalityTrait.ToString();
        methodOfAcquirementText.text = employeeMethodOfAcquirement.ToString();
        hourlyWageText.text = $"{employeeHourlyWage}/hr";
        yearsUnderContractText.text = $"{employeeYearsUnderContract} year(s) remaining";
        efficiencyText.text = $"Efficiency: {employeeEfficiency}";
        customerServiceText.text = $"Customer Service: {employeeCustomerService}";
        communicationText.text = $"Communication: {employeeCommunication}";
        teamworkText.text = $"Teamwork: {employeeTeamwork}";
        iqText.text = $"IQ: {employeeIq}";
        overallText.text = $"Overall: {employeeOverall}";
        mvpsText.text = $"MVEs: {employeeMVPs}";
        employeeOfTheYearsText.text = $"{employeeLists.FrontOrBackOfHouse(thisEmployee)} of house eoty's: {employeeEmployeeOfTheYears}";
        rookieOfTheYearsText.text = $"Rookie of the Years: {employeeRookieOfTheYears}";
        championshipsText.text = $"Championships: {employeeChampionships}";
    }

    #region Employee Stats Functionality
    private void GrabEmployee(Employee employee)
    {
        thisEmployee = employee;
    }

    public void RequestToCutEmployee()
    {
        uiManager.AttemptEmployeeCut(thisEmployee, CutEmployee);
    }

    private void CutEmployee()
    {
        // Contracts with more than 4 years remaining cannot be cut
        if (thisEmployee.yearsUnderContract > 4)
        {
            uiManager.GenericText("Employees with a contract length of over 4 years cannot be cut");
            return;
        }

        generalManager.playersCut++;

        uiManager.EmployeeCut(thisEmployee);

        employeeLists.AddEmployee(thisEmployee, employeeLists.freeAgentClass);
        employeeLists.RemoveEmployee(thisEmployee, employeeLists.currentRoster);

        // If the current period is free agency and you cut a player, they won't update their requested wage in the current free agency class,
        // this prevents a glitch where you can keep cutting and signing to get the best possible contract value. It stays consistent through out
        if (periodManager.currentPeriod != PeriodManager.Period.FreeAgency)
            thisEmployee.hourlyWage = employeeRNG.GetRandomWage(thisEmployee);

        uiManager.RefreshUI();
    }
    #endregion
}
