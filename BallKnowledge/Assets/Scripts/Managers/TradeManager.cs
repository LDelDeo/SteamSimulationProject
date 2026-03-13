using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    [Header("Trading Configuration")]
    [SerializeField] public int tradeBlockSize;
    [SerializeField] public int firstRoundPickValue;
    [SerializeField] public int secondRoundPickValue;
    [SerializeField] public int thirdRoundPickValue;

    [Header("Trading Layout")]
    [SerializeField] GameObject[] displays;

    public Employee employeeToBeAcquired;

    public List<int> outgoingTradePackageValue = new List<int>();
    private int totalTradePackageValue;

    public List<Employee> outgoingEmployees = new List<Employee>();
    public List<int> outgoingDraftPicks = new List<int>();

    private UIManager uiManager;
    private GeneralManager generalManager;
    private EmployeeLists employeeLists;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
        generalManager = GetComponent<GeneralManager>();
        employeeLists = GetComponent<EmployeeLists>();
    }

    #region Trading Assets for an Employee
    public bool TradePackageIsFull() // Trade package can have a max of three assets
    {
        if (outgoingTradePackageValue.Count == 3) return true;
        else return false;
    }

    public bool TradePutsUserOverTheCap()
    {
        int outgoingCapSpace = 0;
        int incomingCapSpace = 0;

        foreach (var employee in outgoingEmployees)
            outgoingCapSpace += employee.hourlyWage; 

        incomingCapSpace = employeeToBeAcquired.hourlyWage;

        if (generalManager.currentUsedCapSpace + incomingCapSpace - outgoingCapSpace <= generalManager.maxCapSpace)
            return false;
        else
            return true;
    }

    public void SubmitTradePackage()
    {
        if (employeeToBeAcquired == null)
        {
            uiManager.NoEmployeeToAcquireSelected();
            return;
        }

        if (!TradePutsUserOverTheCap())
        {
            totalTradePackageValue = 0;

            for (int i = 0; i < outgoingTradePackageValue.Count; i++)
                totalTradePackageValue += outgoingTradePackageValue[i];

            if (totalTradePackageValue >= employeeToBeAcquired.value)
                TradeAccepted();
            else
                TradeDeclined();
        }
        else if (TradePutsUserOverTheCap())
            uiManager.InsufficientCapRoom(employeeToBeAcquired);
    }

    private void TradeAccepted()
    {
        if (employeeLists.HasRosterSpace(employeeToBeAcquired))
        {
            uiManager.TradeAccepted();

            for (int i = 0; i < outgoingDraftPicks.Count; i++)
            {
                if (outgoingDraftPicks[i] == 1) { generalManager.firstRoundPicks--; }
                else if (outgoingDraftPicks[i] == 2) { generalManager.secondRoundPicks--; }
                else if (outgoingDraftPicks[i] == 3) { generalManager.thirdRoundPicks--; }
            }

            uiManager.RefreshTradeInterestBar((float)totalTradePackageValue, (float)employeeToBeAcquired.value);

            outgoingEmployees.Clear();
            outgoingDraftPicks.Clear();
            outgoingTradePackageValue.Clear();

            employeeLists.AddEmployee(employeeToBeAcquired, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(employeeToBeAcquired, employeeLists.tradeBlock);
            employeeToBeAcquired = null;

            uiManager.RefreshUI();
            uiManager.RefreshUserAssetsUI();
        }
        else if (!employeeLists.HasRosterSpace(employeeToBeAcquired))
            uiManager.InsufficientRosterSpace(employeeToBeAcquired);
    }

    private void TradeDeclined()
    {
        uiManager.TradeDeclined();
        uiManager.RefreshTradeInterestBar((float)totalTradePackageValue, (float)employeeToBeAcquired.value);
    }
    #endregion

    #region Trading an Employee for Draft Picks
    public void TradeEmployeeForPicks(Employee employee)
    {
        switch(EmployeeValueInPicks(employee))
        {
            case TradePackages.NoTradeInterest:
                uiManager.NoTradeInterest(employee);
                break;
            case TradePackages.ThirdRoundPick:
                uiManager.TradeEmployeeForPicks(employee, "a third round pick");
                generalManager.thirdRoundPicks += 1;
                break;
            case TradePackages.MultipleThirdRoundPicks:
                uiManager.TradeEmployeeForPicks(employee, "two third round picks");
                generalManager.thirdRoundPicks += 2;
                break;
            case TradePackages.SecondRoundPick:
                uiManager.TradeEmployeeForPicks(employee, "a second round pick");
                generalManager.secondRoundPicks += 1;
                break;
            case TradePackages.MultipleSecondRoundPicks:
                uiManager.TradeEmployeeForPicks(employee, "two second round picks");
                generalManager.secondRoundPicks += 2;
                break;
            case TradePackages.FirstRoundPick:
                uiManager.TradeEmployeeForPicks(employee, "a FIRST round pick");
                generalManager.firstRoundPicks += 1;
                break;
            case TradePackages.MultipleFirstRoundPicks:
                uiManager.TradeEmployeeForPicks(employee, "TWO FIRST ROUND PICKS");
                generalManager.firstRoundPicks += 2;
                break;
        }

        if (employeeLists.currentRoster.Contains(employee) && EmployeeValueInPicks(employee) != TradePackages.NoTradeInterest)
        {
            employeeLists.RemoveEmployee(employee, employeeLists.currentRoster);
            uiManager.RefreshUI();
            uiManager.RefreshUserAssetsUI();
        }  

        generalManager.tradesCompleted++;
    }

    public enum TradePackages
    {
        NoTradeInterest,
        ThirdRoundPick,
        MultipleThirdRoundPicks,
        SecondRoundPick,
        MultipleSecondRoundPicks,
        FirstRoundPick,
        MultipleFirstRoundPicks
    }

    public TradePackages EmployeeValueInPicks(Employee employee)
    {
        if (employee.value >= 4 && employee.value <= 8) return TradePackages.NoTradeInterest;
        else if (employee.value >= 9 && employee.value <= 12) return TradePackages.ThirdRoundPick;
        else if (employee.value >= 13 && employee.value <= 16) return TradePackages.MultipleThirdRoundPicks;
        else if (employee.value >= 17 && employee.value <= 19) return TradePackages.SecondRoundPick;
        else if (employee.value >= 20 && employee.value <= 24) return TradePackages.MultipleSecondRoundPicks;
        else if (employee.value >= 25 && employee.value <= 28) return TradePackages.FirstRoundPick;
        else if (employee.value >= 29) return TradePackages.MultipleFirstRoundPicks;

        return TradePackages.NoTradeInterest;
    }
    #endregion

    #region Trading Layout/Displays
    public void Display(GameObject displayToShow)
    {
        foreach (GameObject display in displays)
            display.SetActive(false);

        displayToShow.SetActive(true);

        uiManager.RefreshCurrentTradePackageUI();
        uiManager.RefreshEmployeesForPicksUI();
        uiManager.tradingScreen.GetComponent<ScrollRect>().content = displayToShow.GetComponent<RectTransform>();
    }
    #endregion
}
