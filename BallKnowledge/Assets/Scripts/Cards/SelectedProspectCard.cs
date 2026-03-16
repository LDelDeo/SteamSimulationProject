using UnityEngine;

public class SelectedProspectCard : EmployeeCard
{
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
