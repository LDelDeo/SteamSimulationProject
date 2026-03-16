using UnityEngine;

public class Employee
{
    public EmployeeEnumerators.EmployeeGender gender;

    public string firstName;
    public string lastName;

    public bool isRookie;
    public int age;

    public int hourlyWage;
    public int yearsUnderContract;

    public EmployeeEnumerators.JobType jobPosition;
    public EmployeeEnumerators.WorkEthic workEthic;
    public EmployeeEnumerators.PersonalityTrait personalityTrait;

    public string methodOfAcquirement;

    public int efficiency;
    public int customerService;
    public int communication;
    public int teamwork;
    public int iq;

    public int overall;

    public int value;

    public int mostValuableEmployee = 0;
    public int employeeOfTheYear = 0;
    public int rookieOfTheYear = 0;

    public int championships = 0;

    public Sprite head;
    public Sprite eyes;
    public Sprite mouth;
    public Sprite ears;
    public Sprite eyebrows;
    public Sprite nose;
    public Sprite glasses;
    public Sprite hair;
    public Sprite facialHair;
    public Color32 skinTone;
    public Color32 hairColor;
}
