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

        if (employee.isRookie) 
        { 
            employee.age = employeeRNG.GetRandomAge(20, 25);
            employee.hourlyWage = 21;
            employee.yearsUnderContract = 3;
        }
        else 
        { 
            employee.age = employeeRNG.GetRandomAge(25, 34);
            employee.yearsUnderContract = 0;
        }

        employee.jobPosition = EmployeeRNG.GetRandomEnumValue<EmployeeEnumerators.JobType>();
        employee.workEthic = EmployeeRNG.GetWorkEthic();

        employee.efficiency = employeeRNG.GetRandomStat();
        employee.customerService = employeeRNG.GetRandomStat();
        employee.communication = employeeRNG.GetRandomStat();
        employee.teamwork = employeeRNG.GetRandomStat();
        employee.iq = employeeRNG.GetRandomStat();

        employee.overall = (employee.efficiency + employee.customerService + employee.communication + employee.teamwork + employee.iq) / 5;

        employee.value = EmployeeValueCalucator(employee);
        if (!employee.isRookie) { employee.hourlyWage = employeeRNG.GetRandomWage(employee); }

        if (listToAddTo == employeeLists.currentRoster && employeeLists.HasRosterSpace(employee))
            { employeeLists.AddEmployee(employee, listToAddTo); }
        else if ( listToAddTo != employeeLists.currentRoster )
        { employeeLists.AddEmployee(employee, listToAddTo); }
        
    }

    public int EmployeeValueCalucator(Employee employee)
    {
        var employeeValue = 0;
        // Max Value = 24, Min Value = 3
        // These values have an affect on the amout of requested wage they'd like in negotiations
        // Combination of age, work ethic and overall (We could possibly add positional value & awards won as well)

        switch (employee.workEthic)
        {
            case EmployeeEnumerators.WorkEthic.Bum:
                employeeValue += 1;
            break;

            case EmployeeEnumerators.WorkEthic.Lazy:
                employeeValue += 2;
            break;

            case EmployeeEnumerators.WorkEthic.Paycheck_Collector:
                employeeValue += 3;
            break;

            case EmployeeEnumerators.WorkEthic.Gets_The_Job_Done:
                employeeValue += 4;
            break;

            case EmployeeEnumerators.WorkEthic.Motivated:
                employeeValue += 5;
            break;

            case EmployeeEnumerators.WorkEthic.Grinder:
                employeeValue += 6;
            break;

            case EmployeeEnumerators.WorkEthic.X_Factor:
                employeeValue += 7;
            break;
        }

        if (employee.overall <= 25) { employeeValue += 1; }
        else if (employee.overall >= 25 && employee.overall < 50) { employeeValue += 2; }
        else if (employee.overall >= 50 && employee.overall < 60) { employeeValue += 3; }
        else if (employee.overall >= 60 && employee.overall < 70) { employeeValue += 4; }
        else if (employee.overall >= 70 && employee.overall < 80) { employeeValue += 5; }
        else if (employee.overall >= 80 && employee.overall < 90) { employeeValue += 6; }
        else if (employee.overall >= 91) { employeeValue += 7; }

        if (employee.age > 34) { employeeValue += 1; }
        else if (employee.age == 33) { employeeValue += 2; }
        else if (employee.age == 32) { employeeValue += 3; }
        else if (employee.age == 31) { employeeValue += 4; }
        else if (employee.age == 30) { employeeValue += 5; }
        else if (employee.age == 29) { employeeValue += 6; }
        else if (employee.age == 28) { employeeValue += 7; }
        else if (employee.age == 27) { employeeValue += 8; }
        else if (employee.age == 26) { employeeValue += 9; }
        else if (employee.age <= 25) { employeeValue += 10; }

        return employeeValue;
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
        Debug.Log($"Employee Value: {employee.value}");
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

    public static EmployeeEnumerators.WorkEthic GetWorkEthic()
    {
        int randomNumber = UnityEngine.Random.Range(1, 101);

        if (randomNumber > 0 && randomNumber < 24) { return EmployeeEnumerators.WorkEthic.Bum; } // 23% Chance
        else if (randomNumber > 23 && randomNumber < 44) { return EmployeeEnumerators.WorkEthic.Lazy; } // 20% Chance
        else if (randomNumber > 43 && randomNumber < 62) { return EmployeeEnumerators.WorkEthic.Paycheck_Collector; } // 18% Chance
        else if (randomNumber > 61 && randomNumber < 77) { return EmployeeEnumerators.WorkEthic.Gets_The_Job_Done; } // 15% Chance
        else if (randomNumber > 76 && randomNumber < 88) { return EmployeeEnumerators.WorkEthic.Motivated; } // 11% Chance
        else if (randomNumber > 87 && randomNumber < 96) { return EmployeeEnumerators.WorkEthic.Grinder; } // 8% Chance
        else if (randomNumber > 95 && randomNumber < 101) { return EmployeeEnumerators.WorkEthic.X_Factor; } // 5% Chance

        return EmployeeEnumerators.WorkEthic.Bum;
    }

    public int GetRandomStat()
    {
        int minStat = 0;
        int maxStat = 101;

        int randomStatValue = UnityEngine.Random.Range(minStat, maxStat);
        return randomStatValue;
    }

    public int GetRandomWage(Employee employee)
    {
        int minWage = employee.value * 2;
        int maxWage = employee.value * 4;

        int randomWageValue = UnityEngine.Random.Range(minWage, maxWage);
        return randomWageValue;
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
