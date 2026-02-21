using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeAgentCard : EmployeeCard
{
    #region Free Agent Visuals
    [Header("Free Agent Card Visuals")]
    [SerializeField] TMP_Text ageText;
    [SerializeField] TMP_Text requestedWageText;
    #endregion

    private string ageValue;
    private int requestedWageValue;

    private Employee freeAgent;

    public override void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        employeePosition = employee.jobPosition.ToString();
        employeeOverall = employee.overall.ToString();

        ageValue = employee.age.ToString();
        requestedWageValue = employee.hourlyWage;

        SetStats();
        SetEmployeeCardBackground(employee);
        GrabEmployee(employee);
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        positionText.text = employeePosition;
        overallText.text = employeeOverall;

        ageText.text = $"Age: {ageValue}";
        requestedWageText.text = $"${requestedWageValue}/hr";
    }

    

    #region Signing Functionality
    private void GrabEmployee(Employee employee)
    {
        freeAgent = employee;
    }

    public void SignPlayer(FreeAgentCard freeAgentCard)
    {
        Employee freeAgentToSign = freeAgentCard.freeAgent;

        if ((manager.currentUsedCapSpace + freeAgentToSign.hourlyWage) < manager.maxCapSpace && employeeLists.HasRosterSpace(freeAgentToSign))
        {
            employeeLists.AddEmployee(freeAgentToSign, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(freeAgentToSign, employeeLists.freeAgentClass);

            uiManager.RefreshUI();
        }
        else
        {
            Debug.Log("Issue");
        }
    }
    #endregion
}
