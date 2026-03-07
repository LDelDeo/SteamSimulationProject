using UnityEngine;
using TMPro;

public class EmployeeProfile : EmployeeCard
{
    #region Visuals
    [Header("Employee Stat Visuals")]
    [SerializeField] TMP_Text genderText;
    [SerializeField] TMP_Text ageText;
    [SerializeField] TMP_Text isRookieText;
    [SerializeField] TMP_Text hourlyWageText;
    [SerializeField] TMP_Text yearsUnderContractText;
    [SerializeField] TMP_Text jobPositionText;
    [SerializeField] TMP_Text workEthicText;
    [SerializeField] TMP_Text personalityText;
    [SerializeField] TMP_Text efficiencyText;
    [SerializeField] TMP_Text customerServiceText;
    [SerializeField] TMP_Text communicationText;
    [SerializeField] TMP_Text teamworkText;
    [SerializeField] TMP_Text iqText;
    [SerializeField] TMP_Text mvpsText;
    [SerializeField] TMP_Text eoftyText;
    [SerializeField] TMP_Text roftyText;
    [SerializeField] TMP_Text championshipsText;
    #endregion

    private string genderValue;
    private string ageValue;
    private string isRookieValue;
    private string hourlyWageValue;
    private string yearsUnderContractValue;
    private string jobPositionValue;
    private string workEthicValue;
    private string personalityValue;
    private string efficiencyValue;
    private string customerServiceValue;
    private string communicationValue;
    private string teamworkValue;
    private string iqValue;
    private string mvpsValue;
    private string eoftyValue;
    private string roftyValue;
    private string championshipsValue;

    private Employee thisEmployee;

    private PeriodManager periodManager;
    EmployeeRNG employeeRNG = new EmployeeRNG();

    private void Start()
    {
        periodManager = FindFirstObjectByType<PeriodManager>();
    }

    public override void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        genderValue = employee.gender.ToString();
        ageValue = employee.age.ToString();
        isRookieValue = employee.isRookie ? "Rookie" : "Veteran";
        hourlyWageValue = employee.hourlyWage.ToString();
        yearsUnderContractValue = employee.yearsUnderContract.ToString();
        jobPositionValue = employee.jobPosition.ToString();
        workEthicValue = employee.workEthic.ToString();
        personalityValue = employee.personalityTrait.ToString();
        employeeOverall = employee.overall.ToString();
        efficiencyValue = employee.efficiency.ToString();
        customerServiceValue = employee.customerService.ToString();
        communicationValue = employee.communication.ToString();
        teamworkValue = employee.teamwork.ToString();
        iqValue = employee.iq.ToString();
        mvpsValue = employee.mostValuableEmployee.ToString();
        eoftyValue = employee.employeeOfTheYear.ToString();
        roftyValue = employee.rookieOfTheYear.ToString();
        championshipsValue = employee.championships.ToString();

        SetStats();
        GrabEmployee(employee);
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        genderText.text = genderValue;
        ageText.text = $"Age: {ageValue}";
        isRookieText.text = isRookieValue;
        positionText.text = employeePosition;
        workEthicText.text = $"Work Ethics: {workEthicValue}";
        personalityText.text = $"Personality: {personalityValue}";
        hourlyWageText.text = $"{hourlyWageValue}/hr";
        yearsUnderContractText.text = $"{yearsUnderContractValue} year(s) remaining";
        efficiencyText.text = $"Efficiency: {efficiencyValue}";
        customerServiceText.text = $"Customer Service: {customerServiceValue}";
        communicationText.text = $"Communication: {communicationValue}";
        teamworkText.text = $"Teamwork: {teamworkValue}";
        iqText.text = $"IQ: {iqValue}";
        overallText.text = $"Overall: {employeeOverall}";
        mvpsText.text = $"MVPs: {mvpsValue}";
        eoftyText.text = $"Employee of the Years: {eoftyValue}";
        roftyText.text = $"Rookie of the Years: {roftyValue}";
        championshipsText.text = $"Championships: {championshipsValue}";
    }

    #region Employee Stats Functionality
    private void GrabEmployee(Employee employee)
    {
        thisEmployee = employee;
    }

    public void CutEmployee()
    {
        // add to cut players

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
