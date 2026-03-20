using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class RetirementCard : EmployeeCard
{
    [Header("Retirement Card Visuals")]  
    [SerializeField] Button convinceToStayButton;
    [SerializeField] Button ackowledgeRetirementButton;
    [SerializeField] Button hallOfFameButton;

    private Employee retiredEmployee;

    private void Start()
    {
        ButtonEnabler(true, true, false);
    }

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
        jobPositionText.text = employeeJobPosition.ToString();
        overallText.text = employeeOverall.ToString();

        ageText.text = $"Age: {employeeAge}";
        hourlyWageText.text = $"${employeeHourlyWage}/hr";
    }

    #region Retirement Functionality
    private void GrabEmployee(Employee employee)
    {
        retiredEmployee = employee;
    }

    public void ConvinceToStay()
    {
        Employee employeeRetiring = this.gameObject.GetComponent<RetirementCard>().retiredEmployee;

        if (employeeRetiring.age >= 40) { ButtonEnabler(false, true, true); } // If employee is 40 or older, 100% chance they retire
        else
        {
            int randomNumber = Random.Range(1, 11);

            if (randomNumber >= 8) // 80% chance employee stays another year if asked
            {
                uiManager.EmployeeRetiring(employeeRetiring, false);

                employeeLists.AddEmployee(employeeRetiring, employeeLists.currentRoster);
                employeeLists.RemoveEmployee(employeeRetiring, employeeLists.retiringEmployees);

                ButtonEnabler(false, false, false);
            }
            else // 20% chance employee retires for good if asked to stay
            {
                uiManager.EmployeeRetiring(employeeRetiring, true);

                ButtonEnabler(false, false, true);
            }
        }
    }

    public void AckowledgeDecsion()
    {
        ButtonEnabler(false, false, true);
    }

    public void AddToHallOfFame()
    {
        Employee employeeRetiring = this.gameObject.GetComponent<RetirementCard>().retiredEmployee;

        uiManager.NameGenericText(employeeRetiring, $"is honored to be added to the {generalManager.franchiseName} hall of fame");

        employeeLists.AddEmployee(employeeRetiring, employeeLists.employeeHallOfFame);

        ButtonEnabler(false, false, false);
    }

    public void ButtonEnabler(bool stayBTN, bool ackowledgeBTN, bool hallOfFameBTN)
    {
        convinceToStayButton.interactable = stayBTN;
        ackowledgeRetirementButton.interactable = ackowledgeBTN;
        hallOfFameButton.interactable = hallOfFameBTN;
    }
    #endregion
}