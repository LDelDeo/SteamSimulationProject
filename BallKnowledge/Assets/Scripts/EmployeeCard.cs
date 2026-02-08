using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmployeeCard : MonoBehaviour
{
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

    public virtual void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        employeePosition = employee.jobPosition.ToString();
        employeeOverall = employee.overall.ToString();

        SetStats();
        SetEmployeeCardBackground(employee);
    }

    #region Setting Values
    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        positionText.text = employeePosition;
        overallText.text = employeeOverall;
    }

    private void SetEmployeeCardBackground(Employee employee)
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
}