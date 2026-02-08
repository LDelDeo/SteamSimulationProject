using TMPro;
using UnityEngine;

public class ProspectCard : EmployeeCard
{
    #region Prospect Visuals
    [Header("Prospect Card Visuals")]
    [SerializeField] TMP_Text statOne;
    [SerializeField] TMP_Text statTwo;
    [SerializeField] TMP_Text statThree;
    [SerializeField] TMP_Text statFour;
    [SerializeField] TMP_Text statFive;
    #endregion

    private int statOneValue;
    private int statTwoValue;
    private int statThreeValue;
    private int statFourValue;
    private int statFiveValue;

    public override void GetEmployeeStats(Employee employee)
    {
        employeeFirstName = employee.firstName;
        employeeLastName = employee.lastName;
        employeePosition = employee.jobPosition.ToString();

        statOneValue = employee.efficiency;
        statTwoValue = employee.customerService;
        statThreeValue = employee.communication;
        statFourValue = employee.teamwork;
        statFiveValue = employee.iq;
        
        SetStats();
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        positionText.text = employeePosition;
        overallText.text = "Overall: ?";

        isStatVisible(statOne, statOneValue);
        isStatVisible(statTwo, statTwoValue);
        isStatVisible(statThree, statThreeValue);
        isStatVisible(statFour, statFourValue);
        isStatVisible(statFive, statFiveValue);
    }

    private void isStatVisible(TMP_Text statText, int statValue)
    {
        int randomNumber = UnityEngine.Random.Range(0, 2);

        if (randomNumber == 0) { statText.text = statValue.ToString(); }
        else if (randomNumber == 1) { statText.text = "?"; }

        // Maybe we can show the overall if all 5 stats are visible
    }
}
