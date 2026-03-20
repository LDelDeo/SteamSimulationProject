using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System;

public class EmployeeCard : MonoBehaviour
{
    #region Required Classes
    protected GeneralManager generalManager;
    protected EmployeeLists employeeLists;
    protected UIManager uiManager;
    protected PeriodManager periodManager;
    protected DraftManager draftManager;
    protected TradeManager tradeManager;
    protected AwardManager awardManager;
    protected EmployeeRNG employeeRNG = new EmployeeRNG();
    #endregion

    private Employee thisEmployee;

    #region Visuals
    [Header("Generic Card Text")]
    [SerializeField] protected TMP_Text firstNameText;
    [SerializeField] protected TMP_Text lastNameText;
    [SerializeField] protected TMP_Text isRookieText;
    [SerializeField] protected TMP_Text genderText;
    [SerializeField] protected TMP_Text jobPositionText;
    [SerializeField] protected TMP_Text workEthicText;
    [SerializeField] protected TMP_Text personalityText;
    [SerializeField] protected TMP_Text methodOfAcquirementText;
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

    [Header("Generic Card Face")]
    [SerializeField] protected SpriteRenderer headRenderer;
    [SerializeField] protected SpriteRenderer eyesRenderer;
    [SerializeField] protected SpriteRenderer mouthRenderer;
    [SerializeField] protected SpriteRenderer earsRenderer;
    [SerializeField] protected SpriteRenderer eyebrowsRenderer;
    [SerializeField] protected SpriteRenderer noseRenderer;
    [SerializeField] protected SpriteRenderer glassesRenderer;
    [SerializeField] protected SpriteRenderer hairRenderer;
    [SerializeField] protected SpriteRenderer facialHairRenderer;

    [Header("Generic Card Background")]
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

    protected string employeeMethodOfAcquirement;

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

    protected Sprite employeeHead;
    protected Sprite employeeEyes;
    protected Sprite employeeMouth;
    protected Sprite employeeEars;
    protected Sprite employeeEyebrows;
    protected Sprite employeeNose;
    protected Sprite employeeGlasses;
    protected Sprite employeeHair;
    protected Sprite employeeFacialHair;

    protected Color32 employeeSkinTone;
    protected Color32 employeeHairColor;
    #endregion

    private void Awake()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
        employeeLists = FindFirstObjectByType<EmployeeLists>();
        uiManager = FindFirstObjectByType<UIManager>();
        periodManager = FindFirstObjectByType<PeriodManager>();
        draftManager = FindFirstObjectByType<DraftManager>();
        tradeManager = FindFirstObjectByType<TradeManager>();
        awardManager = FindFirstObjectByType<AwardManager>();  
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

        employeeMethodOfAcquirement = employee.methodOfAcquirement;

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

        employeeHead = employee.head;
        employeeEyes = employee.eyes;
        employeeMouth = employee.mouth;
        employeeEars = employee.ears;
        employeeEyebrows = employee.eyebrows;
        employeeNose = employee.nose;
        employeeGlasses = employee.glasses;
        employeeHair = employee.hair;
        employeeFacialHair = employee.facialHair;

        employeeSkinTone = employee.skinTone;
        employeeHairColor = employee.hairColor;

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

        headRenderer.sprite = employeeHead;
        eyesRenderer.sprite = employeeEyes;
        mouthRenderer.sprite = employeeMouth;
        earsRenderer.sprite = employeeEars;
        eyebrowsRenderer.sprite = employeeEyebrows;
        noseRenderer.sprite = employeeNose;
        glassesRenderer.sprite = employeeGlasses;
        hairRenderer.sprite = employeeHair;
        facialHairRenderer.sprite = employeeFacialHair;

        headRenderer.color = employeeSkinTone;
        earsRenderer.color = employeeSkinTone;
        noseRenderer.color = employeeSkinTone;

        hairRenderer.color = employeeHairColor;
        facialHairRenderer.color = employeeHairColor;
        eyebrowsRenderer.color = employeeHairColor;
    }

    public void SetEmployeeCardBackground(Employee employee)
    {
        switch (employee.workEthic)
        {
            case EmployeeEnumerators.WorkEthic.Bum: employeeCardBackground.color = new Color32(115, 59, 3, 255); break; // Brown
            case EmployeeEnumerators.WorkEthic.Lazy:employeeCardBackground.color = new Color32(115, 114, 112, 255); break; // Light Grey
            case EmployeeEnumerators.WorkEthic.Paycheck_Collector: employeeCardBackground.color = new Color32(11, 189, 103, 255); break; // Green
            case EmployeeEnumerators.WorkEthic.Gets_The_Job_Done: employeeCardBackground.color = new Color32(11, 150, 189, 255); break; // Sky Blue
            case EmployeeEnumerators.WorkEthic.Motivated: employeeCardBackground.color = new Color32(189, 11, 183, 255); break; // Magenta
            case EmployeeEnumerators.WorkEthic.Grinder: employeeCardBackground.color = new Color32(196, 167, 47, 255); break; // Gold
            case EmployeeEnumerators.WorkEthic.X_Factor: employeeCardBackground.color = new Color32(189, 23, 11, 255); break; // Red
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
        foreach(Transform child in uiManager.employeeStatsContent)
            Destroy(child.gameObject);

        GameObject employeeStatsObject = Instantiate(uiManager.employeeProfileCardPrefab, uiManager.employeeStatsContent);

        EmployeeProfile employeeStats = employeeStatsObject.GetComponent<EmployeeProfile>();
        employeeStats.GetEmployeeStats(thisEmployee);
    }
    #endregion
}