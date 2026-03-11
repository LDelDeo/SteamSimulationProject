using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TradeManager : MonoBehaviour
{
    [SerializeField] public int tradeBlockSize;
    [SerializeField] public int firstRoundPickValue;
    [SerializeField] public int secondRoundPickValue;
    [SerializeField] public int thirdRoundPickValue;

    private List<int> OutgoingTradePackage = new List<int>();
    private int totalTradePackageValue;

    private UIManager uiManager;
    private GeneralManager generalManager;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
        generalManager = GetComponent<GeneralManager>();
    }

    #region Trading assets for an Employee
    public void AddEmployeeToTradePackage()
    {
        // Get employee value and add that int to the trade package
    }

    public void AddDraftPickToTradePackage()
    {
        // Get draft pick round and add that value to the trade package
    }

    public void SubmitTradePackage()
    {
        // Loop through each value in package 1-3
        // Then add all those values together and set it to totalTradePackageValue
        //for (int i = 0; i < OutgoingTradePackage.Count; i++)
            //totalTradePackageValue += assetValue;

        // Then we check to see if that value is >= the value of the employee you're trying to trade for
        // If it is, we accept the trade, add new employee to roster and destroy the other and destroy draft picks

        // We can also us UIManager to create a trade bar which is the tradepackagevalue / employee value(clamped) and then also color the bar based on how close/full the bar is
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
}
