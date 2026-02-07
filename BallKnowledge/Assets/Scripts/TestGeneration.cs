using UnityEngine;
using System.Collections.Generic;

public class TestGeneration : MonoBehaviour
{
    [SerializeField] int employeeCount;
    private void Start()
    {
        EmployeeFactory employeeFactory = new EmployeeFactory();
        EmployeeLists employeeLists = new EmployeeLists();

        for (int i = 0; i < employeeCount; i++)
        {
            employeeFactory.CreateEmployee(employeeLists, employeeLists.currentRoster);
        }

        foreach (var employee in employeeLists.currentRoster)
            employeeFactory.PrintStats(employee);

        Debug.Log(employeeLists.currentRoster.Count);
    }
}
