using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EmployeeLists : MonoBehaviour
{
    RosterConstruction rosterConstruction = new RosterConstruction();

    #region Employee Lists
    public List<Employee> currentRoster = new List<Employee>();
    public List<Employee> draftClass = new List<Employee>();
    public List<Employee> pendingFreeAgents = new List<Employee>();
    public List<Employee> freeAgentClass = new List<Employee>();
    #endregion

    #region Adding & Subtracting Employees
    public void AddEmployee(Employee employee, List<Employee> list)
    {
        if (employee == null)
        {
            Debug.LogError("Add Employee called with NULL employee");
            return;
        }

        if (list == currentRoster || list == draftClass || list == freeAgentClass)
            list.Add(employee);
    }

    public void RemoveEmployee(Employee employee, List<Employee> list)
    {
        list.Remove(employee);
    }

    public bool HasRosterSpace(Employee employee)
    {
        switch (employee.jobPosition)
        {
            case EmployeeEnumerators.JobType.Busser:
                if (rosterConstruction.currentBusser < rosterConstruction.maxbusser)
                {
                    rosterConstruction.currentBusser++;
                    return true;
                }   
                else { break; }

            case EmployeeEnumerators.JobType.Janitor:
                if (rosterConstruction.currentJanitor < rosterConstruction.maxJanitor)
                {
                    rosterConstruction.currentJanitor++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Drive_Thru_Attendee:
                if (rosterConstruction.currentDriveThruAttendee < rosterConstruction.maxDriveThruAttendee)
                {
                    rosterConstruction.currentDriveThruAttendee++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Cashier:
                if (rosterConstruction.currentCashier < rosterConstruction.maxCashier)
                {
                    rosterConstruction.currentCashier++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Media_Manager:
                if (rosterConstruction.currentMediaManager < rosterConstruction.maxMediaManager)
                {
                    rosterConstruction.currentMediaManager++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Prep_Cook:
                if (rosterConstruction.currentPrepCook < rosterConstruction.maxPrepCook)
                {
                    rosterConstruction.currentPrepCook++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Line_Cook:
                if (rosterConstruction.currentLineCook < rosterConstruction.maxLineCook)
                {
                    rosterConstruction.currentLineCook++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Fry_Cook:
                if (rosterConstruction.currentFryCook < rosterConstruction.maxFryCook)
                {
                    rosterConstruction.currentFryCook++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Patty_Flipper:
                if (rosterConstruction.currentPattyFlipper < rosterConstruction.maxPattyFlipper)
                {
                    rosterConstruction.currentPattyFlipper++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Expiditer:
                if (rosterConstruction.currentExpiditer< rosterConstruction.maxExpiditer)
                {
                    rosterConstruction.currentExpiditer++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Shift_Manager:
                if (rosterConstruction.currentShiftManager < rosterConstruction.maxShiftManager)
                {
                    rosterConstruction.currentShiftManager++;
                    return true;
                }
                else { break; }

            case EmployeeEnumerators.JobType.Manager:
                if (rosterConstruction.currentManager < rosterConstruction.maxManager)
                {
                    rosterConstruction.currentManager++;
                    return true;
                }
                else { break; }
        }

        return false;
    }
    #endregion
}

public class RosterConstruction
{
    public int maxbusser { get; private set; } = 3;
    public int currentBusser = 0;
    public int maxJanitor { get; private set; } = 2;
    public int currentJanitor = 0;
    public int maxDriveThruAttendee { get; private set; } = 1;
    public int currentDriveThruAttendee = 0;
    public int maxCashier { get; private set; } = 3;
    public int currentCashier = 0;
    public int maxMediaManager { get; private set; } = 1;
    public int currentMediaManager = 0;
    public int maxPrepCook { get; private set; } = 1;
    public int currentPrepCook = 0;
    public int maxLineCook { get; private set; } = 2;
    public int currentLineCook = 0;
    public int maxFryCook { get; private set; } = 1;
    public int currentFryCook = 0;
    public int maxPattyFlipper { get; private set; } = 1;
    public int currentPattyFlipper = 0;
    public int maxExpiditer { get; private set; } = 1;
    public int currentExpiditer = 0;
    public int maxShiftManager { get; private set; } = 1;
    public int currentShiftManager = 0;
    public int maxManager { get; private set; } = 1;
    public int currentManager = 0;
}
