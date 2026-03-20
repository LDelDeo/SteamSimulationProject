using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedProspectCard : EmployeeCard
{
    [Header("Selected Prospect Visual")]
    public TMP_Text selectionText;
    public Image highlightedBackground;

    public override void GetEmployeeStats(Employee employee)
    {
        base.GetEmployeeStats(employee);

        SetStats();
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        ageText.text = $"Age: {employeeAge}";
        jobPositionText.text = employeeJobPosition.ToString();
        personalityText.text = $"Personality: {employeePersonalityTrait}";
        methodOfAcquirementText.text = $"{employeeMethodOfAcquirement}";
        overallText.text = $"Overall: {employeeOverall}";
    }
}
