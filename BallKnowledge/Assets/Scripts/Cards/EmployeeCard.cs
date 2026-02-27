using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System;

public class EmployeeCard : MonoBehaviour
{
    [Header("Script References")]
    protected GeneralManager manager;
    protected EmployeeLists employeeLists;
    protected UIManager uiManager;

    [SerializeField] private GameObject employeeStatsPrefab;
    private GameObject employeeStatsTransform;

    #region Visuals
    [Header("Employee Card Visuals")]
    [SerializeField] protected TMP_Text firstNameText;
    [SerializeField] protected TMP_Text lastNameText;
    [SerializeField] protected TMP_Text positionText;
    [SerializeField] protected TMP_Text overallText;
    [SerializeField] protected Image employeeCardBackground;
    #endregion

    protected string employeeFirstName;
    protected string employeeLastName;
    protected string employeePosition;
    protected string employeeOverall;

    private Employee thisEmployee;

    private void Awake()
    {
        manager = FindFirstObjectByType<GeneralManager>();
        employeeLists = FindFirstObjectByType<EmployeeLists>();
        uiManager = FindFirstObjectByType<UIManager>();
        employeeStatsTransform =  GameObject.Find("Single Layout (Employee Profile)");
    }

    public virtual void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        employeePosition = employee.jobPosition.ToString();
        employeeOverall = employee.overall.ToString();

        SetStats();
        SetEmployeeCardBackground(employee);
        GrabEmployee(employee);
    }

    #region Setting Values
    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        positionText.text = employeePosition;
        overallText.text = employeeOverall;
    }

    protected void SetEmployeeCardBackground(Employee employee)
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