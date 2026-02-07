using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static EmployeeEnumerators;

public class EmployeeFactory
{
    public void CreateEmployee()
    {
        Employee employee = new Employee();

        EmployeeRNG employeeRNG = new EmployeeRNG();
        EmployeeArrays employeeArrays = new EmployeeArrays();

        employee.gender = employeeRNG.GetGender();

        if (employee.gender == EmployeeGender.Male) { employee.firstName = employeeRNG.GetRandomStringFromArray(employeeArrays.maleNames); }
        else if (employee.gender == EmployeeGender.Female) { employee.firstName = employeeRNG.GetRandomStringFromArray(employeeArrays.femaleNames); }

        employee.lastName = employeeRNG.GetRandomStringFromArray(employeeArrays.lastNames);

        employee.jobPosition = EmployeeRNG.GetRandomEnumValue<JobType>();
        employee.workEthic = EmployeeRNG.GetRandomEnumValue<WorkEthic>();

        employee.efficiency = employeeRNG.GetRandomStat();
        employee.customerService = employeeRNG.GetRandomStat();
        employee.communication = employeeRNG.GetRandomStat();
        employee.teamwork = employeeRNG.GetRandomStat();
        employee.iq = employeeRNG.GetRandomStat();
        employee.employeeStats = new int[] { employee.efficiency, employee.customerService, employee.communication, employee.teamwork, employee.iq };

        employee.overall = (employee.efficiency + employee.customerService + employee.communication + employee.teamwork + employee.iq) / 5;

        PrintStats(employee);
    }

    private void PrintStats(Employee employee)
    {
        Debug.Log("Employee Stats");
        Debug.Log("==============");
        Debug.Log($"First Name: {employee.firstName}");
        Debug.Log($"Last Name: {employee.lastName}");
        Debug.Log($"Sex: {employee.gender}");
        Debug.Log($"Position: {employee.jobPosition}");
        Debug.Log($"Development Trait: {employee.workEthic}");
        Debug.Log($"Efficiency: {employee.efficiency}");
        Debug.Log($"Customer Service: {employee.customerService}");
        Debug.Log($"Communication: {employee.communication}");
        Debug.Log($"Teamwork: {employee.teamwork}");
        Debug.Log($"IQ: {employee.iq}");
        Debug.Log($"Overall/Rating: {employee.overall}");
    }
}

public class EmployeeRNG
{
    public EmployeeGender GetGender()
    {
        var randomNumber = UnityEngine.Random.Range(0, 2);

        switch (randomNumber)
        {
            case 0:
                return EmployeeGender.Male;

            case 1:
                return EmployeeGender.Female;
        }

        return EmployeeGender.None;
    }

    public string GetRandomStringFromArray(string[] array)
    {
        var randomNumber = UnityEngine.Random.Range(0, array.Length);
        return array[randomNumber];
    }

    public static T GetRandomEnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int randomNumber = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(randomNumber);
    }

    public int GetRandomStat()
    {
        int minStat = 0;
        int maxStat = 101;

        int randomStatValue = UnityEngine.Random.Range(minStat, maxStat);
        return randomStatValue;
    }
}

public class EmployeeEnumerators
{
    public enum EmployeeGender
    {
        None,
        Male,
        Female
    }
    public enum JobType
    {
        Prep_Cook,
        Line_Cook,
        Fryer,
        Patty_Flipper,
        Manager,
        Busser,
        Janitor,
        Cashier,
        Drive_Thru_Attendee,
        Media_Manager,
        Expiditer,
        Shift_Manager
    }

    public enum WorkEthic
    {
        Bum,
        Lazy,
        Paycheck_Collector,
        Gets_The_Job_Done,
        Motivated,
        Grinder,
        X_Factor
    }
}

public class EmployeeArrays
{
    // We could Import from JSON instead
    public string[] maleNames = { "Luke", "John", "Josh", "Tom", "Mike", "Owen", "Jared", "Aaron", "Noah" };
    public string[] femaleNames = { "Fallon", "Jenna", "Eden", "Alane", "Carly", "Halie" };
    public string[] lastNames = { "Placeholder" };
}
