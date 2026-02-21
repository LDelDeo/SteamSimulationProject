using UnityEngine;
using System.Collections.Generic;

public class TestGeneration : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] int rosterCount;
    [SerializeField] int draftClassSize;
    [SerializeField] int freeAgencyClassSize;

    [Header("Prefabs")]
    [SerializeField] GameObject RosterScreen;
    [SerializeField] GameObject employeeCard;
    [SerializeField] Transform rosterGrid;

    [SerializeField] GameObject DraftBoardScreen;
    [SerializeField] GameObject prospectCard;
    [SerializeField] Transform prospectLayout;

    [SerializeField] GameObject FreeAgencyScreen;
    [SerializeField] GameObject freeAgentCard;
    [SerializeField] Transform freeAgencyLayout;    

    [Header("Script References")]
    private EmployeeLists employeeLists;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        EmployeeFactory employeeFactory = new EmployeeFactory();

        var employeeCardObject = employeeCard.GetComponent<EmployeeCard>();
        var prospectCardObject = prospectCard.GetComponent<ProspectCard>();
        var freeAgentCardObject = freeAgentCard.GetComponent<FreeAgentCard>();

        // Create Employees for Roster (We can use this for when you start the game)
        CreateAnEmployee(rosterCount, employeeFactory, employeeLists, employeeLists.currentRoster, employeeCardObject, rosterGrid);

        // Create Employees for Draft Class (Create this list for the annual draft in the offseason)
        CreateAnEmployee(draftClassSize, employeeFactory, employeeLists, employeeLists.draftClass, prospectCardObject, prospectLayout);

        // Create Employees for Free Agency Class (Create this list for the annual free agency period)
        // We could add the players that are cut by the user to this list as well for realism
        CreateAnEmployee(freeAgencyClassSize, employeeFactory, employeeLists, employeeLists.freeAgentClass, freeAgentCardObject, freeAgencyLayout);
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

    public void ShowRoster()
    {
        RosterScreen.SetActive(true);
        DraftBoardScreen.SetActive(false);
        FreeAgencyScreen.SetActive(false);
    }

    public void ShowDraftBoard()
    {
        RosterScreen.SetActive(false);
        DraftBoardScreen.SetActive(true);
        FreeAgencyScreen.SetActive(false);
    }

    public void ShowFreeAgents()
    {
        RosterScreen.SetActive(false);
        DraftBoardScreen.SetActive(false);
        FreeAgencyScreen.SetActive(true);
    }
}
