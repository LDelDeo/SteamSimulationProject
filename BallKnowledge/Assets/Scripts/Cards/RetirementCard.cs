using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class RetirementCard : EmployeeCard
{
    [Header("Retirement Card Visuals")]
    [SerializeField] GameObject actionCanvasPrefab;   
    [SerializeField] Button convinceToStayButton;
    [SerializeField] Button ackowledgeRetirementButton;
    [SerializeField] Button hallOfFameButton;
    private GameObject retirementLayoutTransform;
    private GameObject actionCanvasObject;
    private GameObject outcomeText;
    private GameObject closeCanvasButton;

    private Employee retiredEmployee;

    private void Start()
    {
        retirementLayoutTransform = GameObject.Find("Horizontal Layout (Retirements)");

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

    private void FindActionCanvas()
    {
        actionCanvasObject = GameObject.Find("Action Canvas(Clone)");

        outcomeText = GameObject.Find("Outcome Text");

        closeCanvasButton = GameObject.Find("Acknowledge Button");
        closeCanvasButton.GetComponent<Button>().onClick.AddListener(CloseActionCanvas);
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
                Instantiate(actionCanvasPrefab, retirementLayoutTransform.transform);
                FindActionCanvas();
                outcomeText.GetComponent<TMP_Text>().text = $"Welcome {employeeRetiring.firstName} {employeeRetiring.lastName} back for at least another year!";

                employeeLists.AddEmployee(employeeRetiring, employeeLists.currentRoster);
                employeeLists.RemoveEmployee(employeeRetiring, employeeLists.retiringEmployees);

                ButtonEnabler(false, false, false);
            }
            else
            {
                Instantiate(actionCanvasPrefab, retirementLayoutTransform.transform);
                FindActionCanvas();
                outcomeText.GetComponent<TMP_Text>().text = $"{employeeRetiring.firstName} {employeeRetiring.lastName} is retiring for good, but appreciates the offer to come back";

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

        Instantiate(actionCanvasPrefab, retirementLayoutTransform.transform);
        FindActionCanvas();
        outcomeText.GetComponent<TMP_Text>().text = $"{employeeRetiring.firstName} {employeeRetiring.lastName} is honored to be added to (team name)'s hall of fame";

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
        FindActionCanvas();
        Destroy(actionCanvasObject);
    }
    #endregion
}