using UnityEngine;

public class Employee
{
    public EmployeeEnumerators.EmployeeGender gender;

    public string firstName;
    public string lastName;

    public bool isRookie;
    public int age;

    public int? hourlyWage;

    public EmployeeEnumerators.JobType jobPosition;
    public EmployeeEnumerators.WorkEthic workEthic;

    public int efficiency;
    public int customerService;
    public int communication;
    public int teamwork;
    public int iq;

    public int[] employeeStats;
    public int overall;

    public int mostValuableEmployee = 0;
    public int employeeOfTheYear = 0;
    public int rookieOfTheYear = 0;

    public int championships = 0;

    // Sprites for each facial feature below
}
