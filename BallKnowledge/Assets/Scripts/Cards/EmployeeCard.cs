using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System;

public class EmployeeCard : MonoBehaviour
{
    protected GeneralManager manager;
    protected EmployeeLists employeeLists;
    protected UIManager uiManager;
    protected PeriodManager periodManager;
    protected DraftManager draftManager;
    protected EmployeeRNG employeeRNG = new EmployeeRNG();

    [SerializeField] private GameObject employeeStatsPrefab;
    private GameObject employeeStatsTransform;

    private Employee thisEmployee;

    #region Visuals
    [Header("Generic Card Visuals")]
    [SerializeField] protected TMP_Text firstNameText;
    [SerializeField] protected TMP_Text lastNameText;
    [SerializeField] protected TMP_Text isRookieText;
    [SerializeField] protected TMP_Text genderText;
    [SerializeField] protected TMP_Text jobPositionText;
    [SerializeField] protected TMP_Text workEthicText;
    [SerializeField] protected TMP_Text personalityText;
    [SerializeField] protected TMP_Text ageText;
    [SerializeField] protected TMP_Text hourlyWageText;
    [SerializeField] protected TMP_Text yearsUnderContractText;
    [SerializeField] protected TMP_Text efficiencyText;
    [SerializeField] protected TMP_Text customerServiceText;
    [SerializeField] protected TMP_Text communicationText;
    [SerializeField] protected TMP_Text teamworkText;
    [SerializeField] protected TMP_Text iqText;
    [SerializeField] protected TMP_Text overallText;
    [SerializeField] protected TMP_Text valueText;
    [SerializeField] protected TMP_Text mvpsText;
    [SerializeField] protected TMP_Text employeeOfTheYearsText;
    [SerializeField] protected TMP_Text rookieOfTheYearsText;
    [SerializeField] protected TMP_Text championshipsText;
    [SerializeField] protected Image employeeCardBackground;
    #endregion

    #region Temporary Values
    protected string employeeFirstName;
    protected string employeeLastName;

    protected bool employeeIsRookie;

    protected EmployeeEnumerators.EmployeeGender employeeGender;
    protected EmployeeEnumerators.JobType employeeJobPosition;
    protected EmployeeEnumerators.WorkEthic employeeWorkEthic;
    protected EmployeeEnumerators.PersonalityTrait employeePersonalityTrait;

    protected int employeeAge;
    protected int employeeHourlyWage;
    protected int employeeYearsUnderContract;
    protected int employeeEfficiency;
    protected int employeeCustomerService;
    protected int employeeCommunication;
    protected int employeeTeamwork;
    protected int employeeIq;
    protected int employeeOverall;
    protected int employeeValue;
    protected int employeeMVPs;
    protected int employeeEmployeeOfTheYears;
    protected int employeeRookieOfTheYears;
    protected int employeeChampionships;
    #endregion

    private void Awake()
    {
        manager = FindFirstObjectByType<GeneralManager>();
        employeeLists = FindFirstObjectByType<EmployeeLists>();
        uiManager = FindFirstObjectByType<UIManager>();
        employeeStatsTransform =  GameObject.Find("Single Layout (Employee Profile)");
        periodManager = FindFirstObjectByType<PeriodManager>();
        draftManager = FindFirstObjectByType<DraftManager>();
    }

    public virtual void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;

        employeeIsRookie = employee.isRookie;

        employeeGender = employee.gender;
        employeeJobPosition = employee.jobPosition;
        employeeWorkEthic = employee.workEthic;
        employeePersonalityTrait = employee.personalityTrait;

        employeeAge = employee.age;
        employeeHourlyWage = employee.hourlyWage;
        employeeYearsUnderContract = employee.yearsUnderContract;
        employeeEfficiency = employee.efficiency;
        employeeCustomerService = employee.customerService;
        employeeCommunication = employee.communication;
        employeeTeamwork = employee.teamwork;
        employeeIq = employee.iq;
        employeeOverall = employee.overall;
        employeeValue = employee.value;
        employeeMVPs = employee.mostValuableEmployee;
        employeeEmployeeOfTheYears = employee.employeeOfTheYear;
        employeeRookieOfTheYears = employee.rookieOfTheYear;
        employeeChampionships = employee.championships;

        SetStats();
        GrabEmployee(employee);
    }

    #region Setting Values
    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        jobPositionText.text = employeeJobPosition.ToString();
        overallText.text = employeeOverall.ToString();
    }

    public void SetEmployeeCardBackground(Employee employee)
    {
        switch (employee.workEthic)
        {
            case EmployeeEnumerators.WorkEthic.Bum:
                employeeCardBackground.color = new Color32(115, 59, 3, 255); // Brown
                break;

            case EmployeeEnumerators.WorkEthic.Lazy:
                employeeCardBackground.color = new Color32(115, 114, 112, 255); // Light Grey
                break;

            case EmployeeEnumerators.WorkEthic.Paycheck_Collector:
                employeeCardBackground.color = new Color32(11, 189, 103, 255); // Green
                break;

            case EmployeeEnumerators.WorkEthic.Gets_The_Job_Done:
                employeeCardBackground.color = new Color32(11, 150, 189, 255); // Sky Blue
                break;

            case EmployeeEnumerators.WorkEthic.Motivated:
                employeeCardBackground.color = new Color32(189, 11, 183, 255); // Magenta
                break;

            case EmployeeEnumerators.WorkEthic.Grinder:
                employeeCardBackground.color = new Color32(196, 167, 47, 255); // Gold
                break;

            case EmployeeEnumerators.WorkEthic.X_Factor:
                employeeCardBackground.color = new Color32(189, 23, 11, 255); // Red
                break;
        }
    }
    #endregion

    #region Roster Functionality
    private void GrabEmployee(Employee employee)
    {
        thisEmployee = employee;
    }

    public void ShowEmployeeStats()
    {
        foreach(Transform child in employeeStatsTransform.transform)
            Destroy(child.gameObject);

        GameObject employeeStatsObject = Instantiate(employeeStatsPrefab, employeeStatsTransform.transform);

        EmployeeProfile employeeStats = employeeStatsObject.GetComponent<EmployeeProfile>();
        employeeStats.GetEmployeeStats(thisEmployee);
    }
    #endregion
}