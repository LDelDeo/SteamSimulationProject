using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmployeeLists : MonoBehaviour
{
    public RosterConstruction rosterConstruction = new RosterConstruction();

    #region Employee Lists
    public List<Employee> currentRoster = new List<Employee>();
    public List<Employee> draftClass = new List<Employee>();
    public List<Employee> pendingFreeAgents = new List<Employee>();
    public List<Employee> freeAgentClass = new List<Employee>();
    public List<Employee> retiringEmployees = new List<Employee>();
    public List<Employee> disgruntledEmployees = new List<Employee>();
    public List<Employee> tradeBlock = new List<Employee>();
    public List<Employee> employeeHallOfFame = new List<Employee>();
    #endregion

    private GeneralManager manager;

    private void Start()
    {
        manager = GetComponent<GeneralManager>();
    }

    #region Adding & Subtracting Employees
    public void AddEmployee(Employee employee, List<Employee> list)
    {
        if (employee == null)
        {
            Debug.LogError("Add Employee called with NULL employee");
            return;
        }

        list.Add(employee);
    }

    public void RemoveEmployee(Employee employee, List<Employee> list)
    {
        if (list == currentRoster)
        {
            switch (employee.jobPosition)
            {
                case EmployeeEnumerators.JobType.Busser: rosterConstruction.currentBusser--; break;
                case EmployeeEnumerators.JobType.Janitor: rosterConstruction.currentJanitor--; break;
                case EmployeeEnumerators.JobType.Drive_Thru_Attendee: rosterConstruction.currentDriveThruAttendee--; break;
                case EmployeeEnumerators.JobType.Cashier: rosterConstruction.currentCashier--; break;
                case EmployeeEnumerators.JobType.Media_Manager: rosterConstruction.currentMediaManager--; break;
                case EmployeeEnumerators.JobType.Prep_Cook: rosterConstruction.currentPrepCook--; break;
                case EmployeeEnumerators.JobType.Line_Cook: rosterConstruction.currentLineCook--; break;
                case EmployeeEnumerators.JobType.Fry_Cook: rosterConstruction.currentFryCook--; break;
                case EmployeeEnumerators.JobType.Patty_Flipper: rosterConstruction.currentPattyFlipper--; break;
                case EmployeeEnumerators.JobType.Expediter: rosterConstruction.currentExpediter--; break;
                case EmployeeEnumerators.JobType.Shift_Manager: rosterConstruction.currentShiftManager--; break;
                case EmployeeEnumerators.JobType.Manager: rosterConstruction.currentManager--; break;
            }
        }

        list.Remove(employee);
    }
    #endregion

    #region Pre Roster Addition Checks
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

            case EmployeeEnumerators.JobType.Expediter:
                if (rosterConstruction.currentExpediter< rosterConstruction.maxExpediter)
                {
                    rosterConstruction.currentExpediter++;
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

    public bool HasCapSpaceToCompleteTransaction(Employee employee)
    {
        if ((manager.currentUsedCapSpace + employee.hourlyWage) <= manager.maxCapSpace)
            return true;
        else
            return false;
    }

    public string FrontOrBackOfHouse(Employee employee)
    {
        bool isFrontOfHouse = false;

        switch (employee.jobPosition)
        {
            case EmployeeEnumerators.JobType.Busser: isFrontOfHouse = true; break;
            case EmployeeEnumerators.JobType.Janitor: isFrontOfHouse = true; break;
            case EmployeeEnumerators.JobType.Drive_Thru_Attendee: isFrontOfHouse = false; break;
            case EmployeeEnumerators.JobType.Cashier: isFrontOfHouse = true; break;
            case EmployeeEnumerators.JobType.Media_Manager: isFrontOfHouse = true; break;
            case EmployeeEnumerators.JobType.Prep_Cook: isFrontOfHouse = false; break;
            case EmployeeEnumerators.JobType.Line_Cook: isFrontOfHouse = false; break;
            case EmployeeEnumerators.JobType.Fry_Cook: isFrontOfHouse = false; break;
            case EmployeeEnumerators.JobType.Patty_Flipper: isFrontOfHouse = false; break;
            case EmployeeEnumerators.JobType.Expediter: isFrontOfHouse = true; break;
            case EmployeeEnumerators.JobType.Shift_Manager: isFrontOfHouse = false; break;
            case EmployeeEnumerators.JobType.Manager: isFrontOfHouse = false; break;
        }

        if (isFrontOfHouse) return "Front";
        else return "Back";
    }

    public int GetRosterOverall()
    {
        var rosterOverall = 0;

        foreach (Employee employee in currentRoster)
            rosterOverall += (employee.overall / rosterConstruction.GetMaxEmployees());

        return rosterOverall;
    }
    #endregion

    #region Update Functions

    // We should make a max stat field instead of hardcoding 100 in all places this happens
    public void UpgradeEmployeeOverall(Employee employee, int overallToUpgrade)
    {
        if (employee.efficiency < 100) employee.efficiency += overallToUpgrade;
        if (employee.efficiency > 100) employee.efficiency = 100;

        if (employee.customerService < 100) employee.customerService += overallToUpgrade;
        if (employee.customerService > 100) employee.customerService = 100;

        if (employee.communication < 100) employee.communication += overallToUpgrade;
        if (employee.communication > 100) employee.communication = 100;

        if (employee.teamwork < 100) employee.teamwork += overallToUpgrade;
        if (employee.teamwork > 100) employee.teamwork = 100;

        if (employee.iq < 100) employee.iq += overallToUpgrade;
        if (employee.iq > 100) employee.iq = 100;

        employee.overall = (employee.efficiency +
                            employee.customerService +
                            employee.communication +
                            employee.teamwork +
                            employee.iq)
                            / 5;
    }
    #endregion
}

public class RosterConstruction
{
    public int GetMaxEmployees()
    {
        var maxEmployees = 
            maxbusser +
            maxJanitor +
            maxCashier +
            maxMediaManager +
            maxPrepCook +
            maxLineCook +
            maxFryCook +
            maxPattyFlipper +
            maxExpediter +
            maxShiftManager +
            maxManager;

        return maxEmployees;
    }

    public int maxbusser { get; private set; } = 3;
    public int currentBusser = 0;
    public int maxJanitor { get; private set; } = 2;
    public int currentJanitor = 0;
    public int maxDriveThruAttendee { get; private set; } = 2;
    public int currentDriveThruAttendee = 0;
    public int maxCashier { get; private set; } = 3;
    public int currentCashier = 0;
    public int maxMediaManager { get; private set; } = 1;
    public int currentMediaManager = 0;
    public int maxPrepCook { get; private set; } = 1;
    public int currentPrepCook = 0;
    public int maxLineCook { get; private set; } = 2;
    public int currentLineCook = 0;
    public int maxFryCook { get; private set; } = 2;
    public int currentFryCook = 0;
    public int maxPattyFlipper { get; private set; } = 1;
    public int currentPattyFlipper = 0;
    public int maxExpediter { get; private set; } = 1;
    public int currentExpediter = 0;
    public int maxShiftManager { get; private set; } = 1;
    public int currentShiftManager = 0;
    public int maxManager { get; private set; } = 1;
    public int currentManager = 0;
}
