using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeFactory
{
    public void CreateEmployee(EmployeeLists employeeLists, List<Employee> listToAddTo)
    {
        Employee employee = new Employee();

        EmployeeRNG employeeRNG = new EmployeeRNG();
        EmployeeArrays employeeArrays = new EmployeeArrays();

        employee.gender = employeeRNG.GetGender();

        if (employee.gender == EmployeeEnumerators.EmployeeGender.Male) { employee.firstName = employeeRNG.GetRandomStringFromArray(employeeArrays.maleNames); }
        else if (employee.gender == EmployeeEnumerators.EmployeeGender.Female) { employee.firstName = employeeRNG.GetRandomStringFromArray(employeeArrays.femaleNames); }

        employee.lastName = employeeRNG.GetRandomStringFromArray(employeeArrays.lastNames);

        if (listToAddTo == employeeLists.draftClass) { employee.isRookie = true; }    
        else { employee.isRookie = false; }

        if (employee.isRookie) { employee.age = employeeRNG.GetRandomAge(20, 25); }
        else { employee.age = employeeRNG.GetRandomAge(25, 34); }

        employee.jobPosition = EmployeeRNG.GetRandomEnumValue<EmployeeEnumerators.JobType>();
        employee.workEthic = EmployeeRNG.GetRandomEnumValue<EmployeeEnumerators.WorkEthic>();

        employee.efficiency = employeeRNG.GetRandomStat();
        employee.customerService = employeeRNG.GetRandomStat();
        employee.communication = employeeRNG.GetRandomStat();
        employee.teamwork = employeeRNG.GetRandomStat();
        employee.iq = employeeRNG.GetRandomStat();
        employee.employeeStats = new int[] { employee.efficiency, employee.customerService, employee.communication, employee.teamwork, employee.iq };

        employee.overall = (employee.efficiency + employee.customerService + employee.communication + employee.teamwork + employee.iq) / 5;

        employeeLists.AddEmployee(employee, listToAddTo);
    }

    public void PrintStats(Employee employee)
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
    EmployeeEnumerators employeeEnumerators = new EmployeeEnumerators();
    public EmployeeEnumerators.EmployeeGender GetGender()
    {
        var randomNumber = UnityEngine.Random.Range(0, 2);

        switch (randomNumber)
        {
            case 0:
                return EmployeeEnumerators.EmployeeGender.Male;

            case 1:
                return EmployeeEnumerators.EmployeeGender.Female;
        }

        return EmployeeEnumerators.EmployeeGender.None;
    }

    public int GetRandomAge(int minAge, int maxAge)
    {
        var ageOutput = UnityEngine.Random.Range(minAge, maxAge);
        return ageOutput;
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
        Busser,
        Drive_Thru_Attendee,
        Janitor,
        Cashier,
        Media_Manager,
        Prep_Cook,
        Line_Cook,
        Fry_Cook,
        Patty_Flipper,
        Expiditer,
        Shift_Manager,
        Manager
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
    public string[] lastNames = { "Placeholder1", "Placeholder2" };
}
