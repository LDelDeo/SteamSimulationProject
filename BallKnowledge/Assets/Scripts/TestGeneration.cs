using UnityEngine;
using System.Collections.Generic;

public class TestGeneration : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] int rosterCount;
    [SerializeField] int draftClassSize;

    [Header("Prefabs")]
    [SerializeField] GameObject employeeCard;
    [SerializeField] Transform rosterGrid;
    [SerializeField] GameObject prospectCard;
    [SerializeField] Transform prospectLayout;
    

    private void Start()
    {
        EmployeeFactory employeeFactory = new EmployeeFactory();
        EmployeeLists employeeLists = new EmployeeLists();

        var employeeCardObject = employeeCard.GetComponent<EmployeeCard>();
        var prospectCardObject = new ProspectCard();
        prospectCardObject = employeeCard.GetComponent<ProspectCard>();

        // Create Employees for Roster (We can use this for when you start the game)
        CreateAnEmployee(rosterCount, employeeFactory, employeeLists, employeeLists.currentRoster, employeeCardObject, rosterGrid);

        // Create Employees for Draft Class (Create this list for the annual draft in the offseason)
        CreateAnEmployee(draftClassSize, employeeFactory, employeeLists, employeeLists.draftClass, prospectCardObject, prospectLayout);
    }

    private void CreateAnEmployee(int employeeCount, EmployeeFactory employeeFactory, EmployeeLists employeeLists, List<Employee> listToAddTo, EmployeeCard employeeCard, Transform layout)
    {
        for (int i = 0; i < employeeCount; i++)
        {
            employeeFactory.CreateEmployee(employeeLists, listToAddTo);
        }

        foreach (var employee in listToAddTo)
        {
            employeeCard.GetEmployeeStats(employee);
            Instantiate(employeeCard, layout);
        }
    }
}
