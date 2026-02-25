using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RetirementCard : EmployeeCard
{
    #region Free Agent Visuals
    [Header("Retirement Card Visuals")]
    [SerializeField] TMP_Text ageText;
    [SerializeField] TMP_Text currentWageText;

    [SerializeField] GameObject actionCanvas;
    [SerializeField] TMP_Text outcomeText;

    [SerializeField] Button convinceToStayButton;
    [SerializeField] Button ackowledgeRetirementButton;
    [SerializeField] Button hallOfFameButton;
    #endregion

    private string ageValue;
    private int upcomingCapHit;

    private Employee retiredEmployee;

    private void Start()
    {
        ButtonEnabler(true, true, false);
    }

    public override void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        employeePosition = employee.jobPosition.ToString();
        employeeOverall = employee.overall.ToString();

        ageValue = employee.age.ToString();
        upcomingCapHit = employee.hourlyWage;

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
        currentWageText.text = $"${upcomingCapHit}/hr";
    }

    #region Retirement Functionality
    private void GrabEmployee(Employee employee)
    {
        retiredEmployee = employee;
    }

    public void ConvinceToStay(RetirementCard retirementCard)
    {
        Employee employeeRetiring = retirementCard.retiredEmployee;

        if (employeeRetiring.age >= 40) { ButtonEnabler(false, true, true); }
        else
        {
            int randomNumber = Random.Range(1, 11);

            if (randomNumber >= 8)
            {
                actionCanvas.SetActive(true);
                outcomeText.text = $"Welcome {employeeRetiring.firstName} {employeeRetiring.lastName} back for at least another year!";

                employeeLists.AddEmployee(employeeRetiring, employeeLists.currentRoster);
                employeeLists.RemoveEmployee(employeeRetiring, employeeLists.retiringEmployees);

                ButtonEnabler(false, false, false);
            }
            else
            {
                actionCanvas.SetActive(true);
                outcomeText.text = $"{employeeRetiring.firstName} {employeeRetiring.lastName} is retiring for good, but appreciates the offer to come back";

                ButtonEnabler(false, false, true);
            }
        }
    }

    public void AckowledgeDecsion()
    {
        ButtonEnabler(false, false, true);
    }

    public void AddToHallOfFame(RetirementCard retirementCard)
    {
        Employee employeeRetiring = retirementCard.retiredEmployee;

        actionCanvas.SetActive(true);
        outcomeText.text = $"{employeeRetiring.firstName} {employeeRetiring.lastName} is honored to be added to (team name)'s hall of fame";

        employeeLists.AddEmployee(employeeRetiring, employeeLists.employeeHallOfFame);

        ButtonEnabler(false, false, false);
    }

    public void ButtonEnabler(bool stayBTN, bool ackowledgeBTN, bool hallOfFameBTN)
    {
        convinceToStayButton.interactable = stayBTN;
        ackowledgeRetirementButton.interactable = ackowledgeBTN;
        hallOfFameButton.interactable = hallOfFameBTN;
    }

    public void CloseActionCanvas()
    {
        actionCanvas.SetActive(false);
        outcomeText.text = string.Empty;
    }
    #endregion
}