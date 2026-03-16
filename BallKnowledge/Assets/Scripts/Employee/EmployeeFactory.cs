using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EmployeeFactory
{
    public void CreateEmployee(EmployeeLists employeeLists, EmployeeArrays employeeArrays, FaceManager faceManager, List<Employee> listToAddTo)
    {
        Employee employee = new Employee();
        EmployeeRNG employeeRNG = new EmployeeRNG();

        employee.gender = employeeRNG.GetGender();

        if (employee.gender == EmployeeEnumerators.EmployeeGender.Male) 
        { 
            employee.firstName = employeeRNG.GetRandomStringFromList(employeeArrays.maleNamesList);
            // Uncomment when the Face Manager Lists have been populated
            //employeeRNG.GetMaleApperance(employee, faceManager); 
        }
        else if (employee.gender == EmployeeEnumerators.EmployeeGender.Female) 
        { 
            employee.firstName = employeeRNG.GetRandomStringFromList(employeeArrays.femaleNamesList);
            // Uncomment when the Face Manager Lists have been populated
            //employeeRNG.GetFemaleApperance(employee, faceManager);
        }

        employee.lastName = employeeRNG.GetRandomStringFromList(employeeArrays.lastNamesList);

        if (listToAddTo == employeeLists.draftClass) { employee.isRookie = true; }    
        else { employee.isRookie = false; }

        if (employee.isRookie) 
        { 
            employee.age = employeeRNG.GetRandomAge(21, 25);
            employee.hourlyWage = 21;
            employee.yearsUnderContract = 3;
        }
        else if (!employee.isRookie && listToAddTo != employeeLists.tradeBlock)
        { 
            employee.age = employeeRNG.GetRandomAge(25, 34);
            employee.yearsUnderContract = 0;
        }
        else if (!employee.isRookie && listToAddTo == employeeLists.tradeBlock)
        {
            employee.age = employeeRNG.GetRandomAge(25, 34);
            employee.yearsUnderContract = employeeRNG.GetRandomContractLength(1, 6);
        }

        employee.jobPosition = EmployeeRNG.GetRandomEnumValue<EmployeeEnumerators.JobType>();
        employee.workEthic = EmployeeRNG.GetWorkEthic();
        employee.personalityTrait = EmployeeRNG.GetRandomEnumValue<EmployeeEnumerators.PersonalityTrait>();

        employee.methodOfAcquirement = "Previous regime";

        if (listToAddTo == employeeLists.tradeBlock)
        {
            employee.efficiency = employeeRNG.GetJuicedStat();
            employee.customerService = employeeRNG.GetJuicedStat();
            employee.communication = employeeRNG.GetJuicedStat();
            employee.teamwork = employeeRNG.GetJuicedStat();
            employee.iq = employeeRNG.GetJuicedStat();
        }
        else
        {
            employee.efficiency = employeeRNG.GetRandomStat();
            employee.customerService = employeeRNG.GetRandomStat();
            employee.communication = employeeRNG.GetRandomStat();
            employee.teamwork = employeeRNG.GetRandomStat();
            employee.iq = employeeRNG.GetRandomStat();
        }

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
        // Max Value = 30, Min Value = 3
        // Max Contract = 120/hr, Min Contract = $6/hr

        // These values have an affect on the amout of requested wage they'd like in negotiations
        // Combination of age, work ethic, individual awards, championships and overall
        switch (employee.workEthic)
        {
            case EmployeeEnumerators.WorkEthic.Bum: employeeValue += 1; break;
            case EmployeeEnumerators.WorkEthic.Lazy: employeeValue += 2; break;
            case EmployeeEnumerators.WorkEthic.Paycheck_Collector: employeeValue += 3; break;
            case EmployeeEnumerators.WorkEthic.Gets_The_Job_Done: employeeValue += 4; break;
            case EmployeeEnumerators.WorkEthic.Motivated: employeeValue += 5; break;
            case EmployeeEnumerators.WorkEthic.Grinder: employeeValue += 6; break;
            case EmployeeEnumerators.WorkEthic.X_Factor: employeeValue += 7; break;
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

        int accolades = employee.rookieOfTheYear + employee.employeeOfTheYear + employee.mostValuableEmployee + employee.championships;
        switch (accolades)
        {
            case 0: employeeValue += 0; break;
            case 1: employeeValue += 1; break;
            case 2: employeeValue += 2; break;
            case 3: employeeValue += 3; break;
            case 4: employeeValue += 4; break;
            case 5: employeeValue += 5; break;
            default: employeeValue += 6; break;
        }

        return employeeValue;
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
            case 0: return EmployeeEnumerators.EmployeeGender.Male;
            case 1: return EmployeeEnumerators.EmployeeGender.Female;
        }

        return EmployeeEnumerators.EmployeeGender.None;
    }

    public int GetRandomAge(int minAge, int maxAge)
    {
        var ageOutput = UnityEngine.Random.Range(minAge, maxAge);
        return ageOutput;
    }

    public int GetRandomContractLength(int minLength, int maxLength)
    {
        var lengthOutput = UnityEngine.Random.Range(minLength, maxLength);
        return lengthOutput;
    }

    public string GetRandomStringFromList(List<string> list)
    {
        var randomNumber = UnityEngine.Random.Range(0, list.Count);
        return list[randomNumber];
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

    public int GetJuicedStat()
    {
        int minStat = 50;
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

    public void GetMaleApperance(Employee employee, FaceManager faceManager)
    {
        employee.head = faceManager.heads[GetFacialFeature(faceManager.heads)];
        employee.eyes = faceManager.eyes[GetFacialFeature(faceManager.eyes)];
        employee.mouth = faceManager.mouths[GetFacialFeature(faceManager.mouths)];
        employee.ears = faceManager.ears[GetFacialFeature(faceManager.ears)];
        employee.nose = faceManager.noses[GetFacialFeature(faceManager.noses)];
        employee.glasses = faceManager.noses[GetFacialFeature(faceManager.glasses)];
        employee.hair = faceManager.maleHair[GetFacialFeature(faceManager.maleHair)];
        employee.facialHair = faceManager.facialHair[GetFacialFeature(faceManager.facialHair)];

        employee.skinTone = faceManager.skinTones[GetColor(faceManager.skinTones)]; // This gets applied to head, ears, nose
        employee.hairColor = faceManager.hairColor[GetColor(faceManager.hairColor)]; // This gets applied to hair, facial hair, eyebrows
    }

    public void GetFemaleApperance(Employee employee, FaceManager faceManager)
    {
        employee.head = faceManager.heads[GetFacialFeature(faceManager.heads)];
        employee.eyes = faceManager.eyes[GetFacialFeature(faceManager.eyes)];
        employee.mouth = faceManager.mouths[GetFacialFeature(faceManager.mouths)];
        employee.ears = faceManager.ears[GetFacialFeature(faceManager.ears)];
        employee.nose = faceManager.noses[GetFacialFeature(faceManager.noses)];
        employee.glasses = faceManager.noses[GetFacialFeature(faceManager.glasses)];
        employee.hair = faceManager.femaleHair[GetFacialFeature(faceManager.femaleHair)];
        employee.facialHair = faceManager.facialHair[0]; // 0 is none

        employee.skinTone = faceManager.skinTones[GetColor(faceManager.skinTones)]; // This gets applied to head, ears, nose
        employee.hairColor = faceManager.hairColor[GetColor(faceManager.hairColor)]; // This gets applied to hair, eyebrows
    }

    public int GetFacialFeature(Sprite[] facialFeature)
    {
        int facialFeatureIndex = UnityEngine.Random.Range(0, facialFeature.Length);
        return facialFeatureIndex;
    }

    public int GetColor(Color32[] color)
    {
        int colorIndex = UnityEngine.Random.Range(0, color.Length);
        return colorIndex;
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
        Expediter,
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

    public enum PersonalityTrait
    {
        Toxic,
        Diva,
        Difficult,
        Team_Player,
        Saint,
        Perfectionist
    }
}

public class EmployeeArrays
{
    public List<string> maleNamesList = new List<string>();
    public List<string> femaleNamesList = new List<string>();
    public List<string> lastNamesList = new List<string>();

    public List<string> LoadNamesFromJson(string fileName)
    {   
        TextAsset jsonText = Resources.Load<TextAsset>(fileName);
        NamesList data = JsonUtility.FromJson<NamesList>(jsonText.text);

        return data.names;
    }

    public void JsonToList()
    {
        maleNamesList.AddRange(LoadNamesFromJson("MaleNames"));
        femaleNamesList.AddRange(LoadNamesFromJson("FemaleNames"));
        lastNamesList.AddRange(LoadNamesFromJson("LastNames"));
    }
}

[System.Serializable]
public class NamesList
{
    public List<string> names;
}