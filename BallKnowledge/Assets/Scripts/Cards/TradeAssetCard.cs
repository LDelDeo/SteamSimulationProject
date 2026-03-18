using Unity.VisualScripting;
using UnityEngine;

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

    public void GetOffersForEmployee(TradeAssetCard tradeAssetCard)
    {
        Employee employeeToTradeAway = tradeAssetCard.employeeToBeOffered;

        tradeManager.TradeEmployeeForPicks(employeeToTradeAway);
    }

    public void SelectEmployeeToTradeFor(TradeAssetCard tradeBlockCard)
    {
        if (tradeManager.employeeToBeAcquired != null)
        {
            uiManager.GenericText("You can only trade for one employee at a time! Remove the current selected employee before adding this one");
            return;
        }

        Employee employeeToAcquire = tradeBlockCard.employeeToBeOffered;
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

    public void AddEmployeeToTradePackage(TradeAssetCard tradeAssetCard)
    {
        if (tradeManager.TradePackageIsFull())
        {
            uiManager.GenericText("Your trade package is full! Remove an asset before adding this one");
            return;
        }

        Employee employeeToTrade = tradeAssetCard.employeeToBeOffered;

        tradeManager.outgoingTradePackageValue.Add(employeeToTrade.value - tradeManager.outgoingEmployeeValueNerf);

        employeeLists.AddEmployee(employeeToTrade, tradeManager.outgoingEmployees);
        employeeLists.RemoveEmployee(employeeToTrade, employeeLists.currentRoster);

        addButton.SetActive(false);
        removeButton.SetActive(true);

        uiManager.RefreshUI();
    }

    public void RemoveEmployeeFromTradePackage(TradeAssetCard tradeAssetCard)
    {
        Employee employeeNotToTrade = tradeAssetCard.employeeToBeOffered;

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
