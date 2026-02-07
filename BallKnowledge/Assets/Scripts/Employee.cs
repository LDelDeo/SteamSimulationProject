using UnityEngine;

public class Employee
{
    string firstName;
    string lastName;

    EmployeeEnumerators.Gender gender;

    EmployeeEnumerators.JobTypes role;

    int efficiency;
    int customerService;
    int communication;
    int teamwork;
    int workEthic;
    int overall;

    Sprite skinTone;
    Sprite hair;
    Sprite mouth;
    Sprite eyes;
    Sprite nose;
    Sprite mustache;
    Sprite beard;
}

public class EmployeeEnumerators
{
    public enum Gender
    {
        Male,
        Female
    }
    public enum JobTypes
    {
        Prep_Cook,
        Line_Cook,
        Fryer,
        Patty_Flipper,
        Manager,
        Busser,
        Janitor,
        Cashier,
        Drive_Thru_Attendee
    }

}
