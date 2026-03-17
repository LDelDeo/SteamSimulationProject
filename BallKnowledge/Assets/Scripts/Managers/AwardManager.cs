using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AwardManager : MonoBehaviour
{
    // We should probably make the prizes different for each level, MVP is + 10, Player of Year is +7, rookie is +5, team player is +3
    [Header("Award Nomination Configuration")]
    [SerializeField] int ovrRequiredToBeNominatedForREOTY;
    [SerializeField] int ovrRequiredToBeNominatedForEOTY;
    [SerializeField] int ovrRequiredToBeNominatedForMVE;

    [Header("Award Prize Configuration")]
    public int ovrUpgradeAmountTeamAward;
    public int ovrUpgradeAmountREOTY;
    public int ovrUpgradeAmountFOHEOTY;
    public int ovrUpgradeAmountBOHEOTY;
    public int ovrUpgradeAmountMVE;

    #region Award Lists
    private List<Employee> rookieEOTYNominations = new List<Employee>();
    private List<Employee> frontOfHouseEOTYNominations = new List<Employee>();
    private List<Employee> backOfHouseEOTYNominations = new List<Employee>();
    private List<Employee> mostValuableEmployeeNominations = new List<Employee>();
    public List<Employee> awardWinners = new List<Employee>();
    #endregion

    #region Award Winners
    public Employee REOTY;
    public Employee FOHEOTY;
    public Employee BOHEOTY;
    public Employee MVE;
    #endregion

    #region Award Winner Prizes
    [Header("Award Winner Prizes")]
    public PrizeTypes rookiePrize;
    public PrizeTypes frontPrize;
    public PrizeTypes backPrize;
    public PrizeTypes mostValuablePrize;
    #endregion

    private EmployeeLists employeeLists;
    private GeneralManager generalManager;
    private UIManager uiManager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();
        generalManager = GetComponent<GeneralManager>();
        uiManager = GetComponent<UIManager>();
    }

    #region Award Enumerators
    public enum PrizeTypes
    {
        none,
        workEthicUpgrade,
        overallUpgrade,
        personalityTraitUpgrade,
        compensatoryFirstRoundPick
    }
    #endregion

    #region Functionality/Checks
    public void GetAwardWinners()
    {
        AwardCriteraCheck();
        uiManager.showEmployeesToNominateButton.interactable = true;
    }

    private void AwardCriteraCheck()
    {
        // Rookie Employee of the Year
        // Must be a rookie, 60 overall or higher, higher than toxic personality
        foreach (var employee in employeeLists.currentRoster)
        {
            if (employee.isRookie &&
                employee.overall >= ovrRequiredToBeNominatedForREOTY &&
                employee.personalityTrait > EmployeeEnumerators.PersonalityTrait.Toxic)
                rookieEOTYNominations.Add(employee);
        }

        // Front of House Employee of the Year
        // Must be working front of house, 70 overall or higher, higher than diva personality, higher than lazy work ethic
        foreach (var employee in employeeLists.currentRoster)
        {
            if (employeeLists.FrontOrBackOfHouse(employee) == "Front" &&
                employee.overall >= ovrRequiredToBeNominatedForEOTY &&
                employee.personalityTrait > EmployeeEnumerators.PersonalityTrait.Diva &&
                employee.workEthic > EmployeeEnumerators.WorkEthic.Lazy)
                frontOfHouseEOTYNominations.Add(employee);
        }

        // Back of House Employee of the Year
        // Must be working back of house, 70 overall or higher, higher than diva personality, higher than lazy work ethic
        foreach (var employee in employeeLists.currentRoster)
        {
            if (employeeLists.FrontOrBackOfHouse(employee) == "Back" &&
                employee.overall >= ovrRequiredToBeNominatedForEOTY &&
                employee.personalityTrait > EmployeeEnumerators.PersonalityTrait.Diva &&
                employee.workEthic > EmployeeEnumerators.WorkEthic.Lazy)
                backOfHouseEOTYNominations.Add(employee);
        }

        // Most Valuable Employee
        // Can't be rookie, 80 overall or higher, higher than team player personality, higher than gets the job done work ethic
        foreach (var employee in employeeLists.currentRoster)
        {
            if (!employee.isRookie &&
                employee.overall >= ovrRequiredToBeNominatedForMVE &&
                employee.personalityTrait > EmployeeEnumerators.PersonalityTrait.Team_Player &&
                employee.workEthic > EmployeeEnumerators.WorkEthic.Gets_The_Job_Done)
                mostValuableEmployeeNominations.Add(employee);
        }

        SelectAwardWinner();
    }

    private void SelectAwardWinner()
    {
        // The more employees you have nominated for a specific award,
        // the more likely you are to have an employee win that specific award
        // (1 nomination = 50%, 2 nominations 66%, 3 nominations 75% etc.)

        if (rookieEOTYNominations.Count > 0)
            SelectREOTY();

        if (frontOfHouseEOTYNominations.Count > 0)
            SelectFOHEOTY();

        if (backOfHouseEOTYNominations.Count > 0)
            SelectBOHEOTY();

        if (mostValuableEmployeeNominations.Count > 0)
            SelectMVE();

        OneAwardPerEmployee();
    }

    private void SelectREOTY()
    {
        var rookieWinner = Random.Range(0, rookieEOTYNominations.Count);
        if (rookieWinner < rookieEOTYNominations.Count) REOTY = rookieEOTYNominations[rookieWinner];
        else REOTY = null;
    }

    private void SelectFOHEOTY()
    {
        var frontWinner = Random.Range(0, frontOfHouseEOTYNominations.Count);
        if (frontWinner < frontOfHouseEOTYNominations.Count) FOHEOTY = frontOfHouseEOTYNominations[frontWinner];
        else FOHEOTY = null;
    }

    private void SelectBOHEOTY()
    {
        var backWinner = Random.Range(0, backOfHouseEOTYNominations.Count);
        if (backWinner < backOfHouseEOTYNominations.Count) BOHEOTY = backOfHouseEOTYNominations[backWinner];
        else BOHEOTY = null;
    }

    private void SelectMVE()
    {
        var mostValuableWinner = Random.Range(0, mostValuableEmployeeNominations.Count);
        if (mostValuableWinner < mostValuableEmployeeNominations.Count) MVE = mostValuableEmployeeNominations[mostValuableWinner];
        else MVE = null;
    }

    private void OneAwardPerEmployee()
    {
        // If the rookie employee of the year wins front/back of house employee of the year,
        // they will just keep the higher award and we will reselect a rookie employee of the year
        if (REOTY == FOHEOTY && REOTY != null && FOHEOTY != null)
        {
            rookieEOTYNominations.Remove(FOHEOTY);
            SelectREOTY();
        }

        if (REOTY == BOHEOTY && REOTY != null && BOHEOTY != null)
        {
            rookieEOTYNominations.Remove(BOHEOTY);
            SelectREOTY();
        }

        // If the front/back of house employee of the year wins most valuable employee,
        // they will just keep the higher award and we will reselect a front/back of house employee of the year
        if (FOHEOTY == MVE && FOHEOTY != null && MVE != null)
        {
            frontOfHouseEOTYNominations.Remove(MVE);
            SelectFOHEOTY();
        }

        if (BOHEOTY == MVE && BOHEOTY != null & MVE != null)
        {
            backOfHouseEOTYNominations.Remove(MVE);
            SelectBOHEOTY();
        }

        SelectAwardWinnersPrize();
    }

    private void SelectAwardWinnersPrize()
    {
        if (REOTY != null)
        {
            PrizeSelector(REOTY, "REOTY");
            REOTY.rookieOfTheYear++;

            awardWinners.Add(REOTY);
        }   

        if (FOHEOTY != null)
        {
            PrizeSelector(FOHEOTY, "FOHEOTY");
            FOHEOTY.employeeOfTheYear++;

            awardWinners.Add(FOHEOTY);
        }
            
        if (BOHEOTY != null)
        {
            PrizeSelector(BOHEOTY, "BOHEOTY");
            BOHEOTY.employeeOfTheYear++;

            awardWinners.Add(BOHEOTY);
        }

        if (MVE != null)
        {
            PrizeSelector(MVE, "MVE");
            MVE.mostValuableEmployee++;

            awardWinners.Add(MVE);
        }       
    }

    // There must be a way to simplify this and clean this up
    private void PrizeSelector(Employee awardWinner, string awardWon)
    {
        // Select 1 of 3 prizes, if the employee doesn't benefit from the prize, it will select one that the employee WILL benefit from
        var randomPrize = Random.Range(0, 3);
        PrizeTypes selectedPrize = PrizeTypes.none;

        switch (randomPrize)
        {
            case 0: selectedPrize = PrizeTypes.workEthicUpgrade; break;
            case 1: selectedPrize = PrizeTypes.overallUpgrade; break;
            case 3: selectedPrize = PrizeTypes.personalityTraitUpgrade; break;
        }

        // If the employee doesn't benefit from any of these prizes, the user will gain a compensatory 1st round pick in the upcoming draft
        if (awardWinner.workEthic == EmployeeEnumerators.WorkEthic.X_Factor &&
            awardWinner.overall > 89 &&
            awardWinner.personalityTrait == EmployeeEnumerators.PersonalityTrait.Perfectionist)
        {
            switch (awardWon)
            {
                case "REOTY":
                    rookiePrize = PrizeTypes.compensatoryFirstRoundPick;
                    break;

                case "FOHEOTY":
                    frontPrize = PrizeTypes.compensatoryFirstRoundPick;
                    break;

                case "BOHEOTY":
                    backPrize = PrizeTypes.compensatoryFirstRoundPick;
                    break;

                case "MVE":
                    mostValuablePrize = PrizeTypes.compensatoryFirstRoundPick;
                    break;
            }

            generalManager.firstRoundPicks++;
        }

        switch (selectedPrize)
        {
            case PrizeTypes.workEthicUpgrade:
                if (awardWinner.workEthic == EmployeeEnumerators.WorkEthic.X_Factor) PrizeSelector(awardWinner, awardWon);
                else
                {
                    // MVE Upgrades to instant max tier, other awards upgrade one tier
                    switch (awardWon)
                    {
                        case "REOTY":
                            awardWinner.workEthic++;
                            rookiePrize = PrizeTypes.workEthicUpgrade;
                            break;

                        case "FOHEOTY":
                            awardWinner.workEthic++;
                            frontPrize = PrizeTypes.workEthicUpgrade;
                            break;

                        case "BOHEOTY":
                            awardWinner.workEthic++;
                            backPrize = PrizeTypes.workEthicUpgrade;                           
                            break;

                        case "MVE":
                            awardWinner.workEthic = EmployeeEnumerators.WorkEthic.X_Factor;
                            mostValuablePrize = PrizeTypes.workEthicUpgrade;
                           break;
                    }
                }
                break;

            case PrizeTypes.overallUpgrade:
                if (awardWinner.overall > 89) PrizeSelector(awardWinner, awardWon);
                else
                {
                    // Upgrade overall prize amount depends on what award they won
                    int amountToUpgrade = 0;

                    switch (awardWon)
                    {
                        case "REOTY": 
                            amountToUpgrade = ovrUpgradeAmountREOTY;
                            rookiePrize = PrizeTypes.overallUpgrade;
                            break;
                        
                        case "FOHEOTY": 
                            amountToUpgrade = ovrUpgradeAmountFOHEOTY;
                            frontPrize = PrizeTypes.overallUpgrade;
                            break;
                        
                        case "BOHEOTY": 
                            amountToUpgrade = ovrUpgradeAmountBOHEOTY;
                            backPrize = PrizeTypes.overallUpgrade;
                            break;
                        
                        case "MVE": 
                            amountToUpgrade = ovrUpgradeAmountMVE;
                            mostValuablePrize = PrizeTypes.overallUpgrade;
                            break;
                    }

                    awardWinner.efficiency += amountToUpgrade;
                    awardWinner.customerService += amountToUpgrade;
                    awardWinner.communication += amountToUpgrade;
                    awardWinner.teamwork += amountToUpgrade;
                    awardWinner.iq += amountToUpgrade;
                }
                break;

            case PrizeTypes.personalityTraitUpgrade:
                if (awardWinner.personalityTrait == EmployeeEnumerators.PersonalityTrait.Perfectionist) PrizeSelector(awardWinner, awardWon);
                else
                {
                    // MVE Upgrades to instant max tier, other awards upgrade one tier
                    switch (awardWon)
                    {
                        case "REOTY":
                            awardWinner.personalityTrait++;
                            rookiePrize = PrizeTypes.personalityTraitUpgrade;
                            break;

                        case "FOHEOTY":
                            awardWinner.personalityTrait++;
                            frontPrize = PrizeTypes.personalityTraitUpgrade;
                            break;

                        case "BOHEOTY":
                            awardWinner.personalityTrait++;
                            backPrize = PrizeTypes.personalityTraitUpgrade;
                            break;

                        case "MVE":
                            awardWinner.personalityTrait = EmployeeEnumerators.PersonalityTrait.Perfectionist;
                            mostValuablePrize = PrizeTypes.personalityTraitUpgrade;
                            break;
                    }
                }
                break;
        }
    }

    public void ResetAwards()
    {
        REOTY = null;
        FOHEOTY = null;
        BOHEOTY = null;
        MVE = null;

        rookiePrize = PrizeTypes.none;
        frontPrize = PrizeTypes.none;
        backPrize = PrizeTypes.none;
        mostValuablePrize = PrizeTypes.none;

        rookieEOTYNominations.Clear();
        frontOfHouseEOTYNominations.Clear();
        backOfHouseEOTYNominations.Clear();
        mostValuableEmployeeNominations.Clear();
        awardWinners.Clear();
    }

    public void ShowEmployeesToNominate()
    {
        uiManager.employeesToNominateContent.gameObject.SetActive(true);
    }
    #endregion
}
