using Unity.VisualScripting;
using UnityEngine;
using static TradeManager;

public class TradeAssetCard : EmployeeCard
{
    [Header("Trade Asset/Block Card Visuals")]
    public GameObject addButton;
    public GameObject removeButton;

    [SerializeField] bool tradingForPicks;

    private Employee employeeToBeOffered;

    public override void GetEmployeeStats(Employee employee)
    {
        base.GetEmployeeStats(employee);

        SetStats();
        GrabEmployee(employee);

        if (!tradingForPicks)
        {
            addButton.SetActive(true);
            removeButton.SetActive(false);
        }
    }

    private void SetStats()
    {
        firstNameText.text = employeeFirstName;
        lastNameText.text = employeeLastName;
        jobPositionText.text = employeeJobPosition.ToString();
        overallText.text = employeeOverall.ToString();
        ageText.text = $"Age: {employeeAge}";
        personalityText.text = $"Personality: {employeePersonalityTrait}";
        hourlyWageText.text = $"${employeeHourlyWage}/hr";
        yearsUnderContractText.text = $"{employeeYearsUnderContract} years remaining";
    }

    #region Adding to Trade Package & Getting Offers Functionality
    private void GrabEmployee(Employee employee)
    {
        employeeToBeOffered = employee;
    }

    public void RequestToGetOffers()
    {
        if (tradeManager.EmployeeValueInPicks(employeeToBeOffered) == TradeManager.TradePackages.NoTradeInterest)
            uiManager.NameGenericText(employeeToBeOffered, "has generated no trade interest from other fast food franchises");
        else
        uiManager.AttemptEmployeeTrade(employeeToBeOffered, GetOffersForEmployee, AttemptTradeEmployeeForPicks());
    }

    private string AttemptTradeEmployeeForPicks()
    {
        var tradePackage = string.Empty;

        switch (tradeManager.EmployeeValueInPicks(employeeToBeOffered))
        {
            case TradePackages.NoTradeInterest: tradePackage = "has generated no trade interest from other fast food franchises"; break;
            case TradePackages.ThirdRoundPick: tradePackage = "a third round pick"; break;
            case TradePackages.MultipleThirdRoundPicks: tradePackage = "two third round picks"; break;
            case TradePackages.SecondRoundPick: tradePackage = "a second round pick"; break;
            case TradePackages.MultipleSecondRoundPicks: tradePackage = "two second round picks"; break;
            case TradePackages.FirstRoundPick: tradePackage = "a FIRST round pick"; break;
            case TradePackages.MultipleFirstRoundPicks: tradePackage = "TWO FIRST ROUND PICKS"; break;
        }

        return tradePackage;
    }

    private void GetOffersForEmployee()
    {
        tradeManager.TradeEmployeeForPicks(employeeToBeOffered);
    }

    public void SelectEmployeeToTradeFor()
    {
        if (tradeManager.employeeToBeAcquired != null)
        {
            uiManager.GenericText("You can only trade for one employee at a time! Remove the current selected employee before adding this one");
            return;
        }

        Employee employeeToAcquire = this.gameObject.GetComponent<TradeAssetCard>().employeeToBeOffered;
        tradeManager.employeeToBeAcquired = employeeToAcquire;

        addButton.SetActive(false);
        removeButton.SetActive(true);

        uiManager.RefreshUI();
    }

    public void DeselectEmployeeToTradeFor()
    {
        tradeManager.employeeToBeAcquired = null;

        addButton.SetActive(true);
        removeButton.SetActive(false);

        uiManager.RefreshUI();
    }

    public void AddEmployeeToTradePackage()
    {
        if (tradeManager.TradePackageIsFull())
        {
            uiManager.GenericText("Your trade package is full! Remove an asset before adding this one");
            return;
        }

        Employee employeeToTrade = this.gameObject.GetComponent<TradeAssetCard>().employeeToBeOffered;

        tradeManager.outgoingTradePackageValue.Add(employeeToTrade.value - tradeManager.outgoingEmployeeValueNerf);

        employeeLists.AddEmployee(employeeToTrade, tradeManager.outgoingEmployees);
        employeeLists.RemoveEmployee(employeeToTrade, employeeLists.currentRoster);

        addButton.SetActive(false);
        removeButton.SetActive(true);

        uiManager.RefreshUI();
    }

    public void RemoveEmployeeFromTradePackage()
    {
        Employee employeeNotToTrade = this.gameObject.GetComponent<TradeAssetCard>().employeeToBeOffered;

        tradeManager.outgoingTradePackageValue.Remove(employeeNotToTrade.value - tradeManager.outgoingEmployeeValueNerf);

        if (employeeLists.HasRosterSpace(employeeNotToTrade))
        {
            employeeLists.AddEmployee(employeeNotToTrade, employeeLists.currentRoster);
            employeeLists.RemoveEmployee(employeeNotToTrade, tradeManager.outgoingEmployees);
        }

        addButton.SetActive(true);
        removeButton.SetActive(false);

        uiManager.RefreshUI();
    }
    #endregion
}
