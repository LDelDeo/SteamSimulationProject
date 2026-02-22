using UnityEngine;
using System.Collections.Generic;

public class TestGeneration : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] int rosterCount;
    [SerializeField] int draftClassSize;
    [SerializeField] int freeAgencyClassSize;

    [Header("Script References")]
    private EmployeeLists employeeLists;
    private UIManager uiManager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        EmployeeFactory employeeFactory = new EmployeeFactory();

        uiManager = GetComponent<UIManager>();
        var employeeCardObject = uiManager.employeeCard.GetComponent<EmployeeCard>();
        var prospectCardObject = uiManager.prospectCard.GetComponent<ProspectCard>();
        var freeAgentCardObject = uiManager.freeAgentCard.GetComponent<FreeAgentCard>();

        // Create Employees for Roster (We can use this for when you start the game)
        CreateAnEmployee(rosterCount, employeeFactory, employeeLists, employeeLists.currentRoster, employeeCardObject, uiManager.rosterGrid);

        // Create Employees for Draft Class (Create this list for the annual draft in the offseason)
        CreateAnEmployee(draftClassSize, employeeFactory, employeeLists, employeeLists.draftClass, prospectCardObject, uiManager.prospectLayout);

        // Create Employees for Free Agency Class (Create this list for the annual free agency period)
        // We could add the players that are cut by the user to this list as well for realism
        CreateAnEmployee(freeAgencyClassSize, employeeFactory, employeeLists, employeeLists.freeAgentClass, freeAgentCardObject, uiManager.freeAgencyLayout);
    }

    private void CreateAnEmployee(int employeeCount, EmployeeFactory employeeFactory, EmployeeLists employeeLists, List<Employee> listToAddTo, EmployeeCard employeeCard, Transform layout)
    {
        for (int i = 0; i < employeeCount; i++)
        {
            employeeFactory.CreateEmployee(employeeLists, listToAddTo);
        }

        foreach (var employee in listToAddTo)
        {
            GameObject cardObject = Instantiate(employeeCard.gameObject, layout);
            EmployeeCard cardInstance = cardObject.GetComponent<EmployeeCard>();

            cardInstance.GetEmployeeStats(employee);
            employeeFactory.PrintStats(employee);
        }
    }    
}
