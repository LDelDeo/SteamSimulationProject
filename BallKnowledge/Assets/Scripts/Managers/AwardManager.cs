using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AwardManager : MonoBehaviour
{
    [Header("Award Configuration")]
    [SerializeField] int ovrRequiredToBeNominatedForREOTY;
    [SerializeField] int ovrRequiredToBeNominatedForEOTY;
    [SerializeField] int ovrRequiredToBeNominatedForMVE;
    [SerializeField] int ovrUpgradeAmount;

    #region Award Lists
    private List<Employee> rookieEOTYNominations = new List<Employee>();
    private List<Employee> frontOfHouseEOTYNominations = new List<Employee>();
    private List<Employee> backOfHouseEOTYNominations = new List<Employee>();
    private List<Employee> mostValuableEmployeeNominations = new List<Employee>();
    public List<Employee> awardWinners = new List<Employee>();
    #endregion

    #region Award Winners
    private Employee REOTY;
    private Employee FOHEOTY;
    private Employee BOHEOTY;
    private Employee MVE;
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
        personalityTraitUpgrade
    }
    #endregion

    #region Functionality/Checks
    public void GetAwardWinners()
    {
        AwardCriteraCheck();
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
        var rookieWinner = Random.Range(0, rookieEOTYNominations.Count + 1);
        if (rookieWinner < rookieEOTYNominations.Count) REOTY = rookieEOTYNominations[rookieWinner];
        else REOTY = null;
    }

    private void SelectFOHEOTY()
    {
        var frontWinner = Random.Range(0, frontOfHouseEOTYNominations.Count + 1);
        if (frontWinner < frontOfHouseEOTYNominations.Count) FOHEOTY = frontOfHouseEOTYNominations[frontWinner];
        else FOHEOTY = null;
    }

    private void SelectBOHEOTY()
    {
        var backWinner = Random.Range(0, backOfHouseEOTYNominations.Count + 1);
        if (backWinner < backOfHouseEOTYNominations.Count) BOHEOTY = backOfHouseEOTYNominations[backWinner];
        else BOHEOTY = null;
    }

    private void SelectMVE()
    {
        var mostValuableWinner = Random.Range(0, mostValuableEmployeeNominations.Count + 1);
        if (mostValuableWinner < mostValuableEmployeeNominations.Count + 1) MVE = mostValuableEmployeeNominations[mostValuableWinner];
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
            PrizeSelector(REOTY);
            REOTY.rookieOfTheYear++;

            awardWinners.Add(REOTY);
        }   

        if (FOHEOTY != null)
        {
            PrizeSelector(FOHEOTY);
            FOHEOTY.employeeOfTheYear++;

            awardWinners.Add(FOHEOTY);
        }
            
        if (BOHEOTY != null)
        {
            PrizeSelector(BOHEOTY);
            BOHEOTY.employeeOfTheYear++;

            awardWinners.Add(BOHEOTY);
        }

        if (MVE != null)
        {
            PrizeSelector(MVE);
            MVE.mostValuableEmployee++;

            awardWinners.Add(MVE);
        }       
    }

    private void PrizeSelector(Employee awardWinner)
    {
        // Select 1 of 3 prizes, if the employee doesn't benefit from the prize, it will select another
        var randomPrize = Random.Range(0, 3);
        PrizeTypes selectedPrize = PrizeTypes.none;

        switch (randomPrize)
        {
            case 0: selectedPrize = PrizeTypes.workEthicUpgrade; break;
            case 1: selectedPrize = PrizeTypes.overallUpgrade; break;
            case 3: selectedPrize = PrizeTypes.personalityTraitUpgrade; break;
        }

        // If the employee doesn't benefit from any of these prizes, the user will gain a complementary 1st round pick in the upcoming draft
        if (awardWinner.workEthic != EmployeeEnumerators.WorkEthic.X_Factor &&
            awardWinner.overall <= 89 &&
            awardWinner.personalityTrait != EmployeeEnumerators.PersonalityTrait.Perfectionist)
        {
            switch (selectedPrize)
            {
                case PrizeTypes.workEthicUpgrade:
                    if (awardWinner.workEthic == EmployeeEnumerators.WorkEthic.X_Factor)
                        PrizeSelector(awardWinner);
                    else
                        awardWinner.workEthic++;
                    break;

                case PrizeTypes.overallUpgrade:
                    if (awardWinner.overall > 89)
                        PrizeSelector(awardWinner);
                    else
                    {
                        awardWinner.efficiency += ovrUpgradeAmount;
                        awardWinner.customerService += ovrUpgradeAmount;
                        awardWinner.communication += ovrUpgradeAmount;
                        awardWinner.teamwork += ovrUpgradeAmount;
                        awardWinner.iq += ovrUpgradeAmount;
                    }
                    break;

                case PrizeTypes.personalityTraitUpgrade:
                    if (awardWinner.personalityTrait == EmployeeEnumerators.PersonalityTrait.Perfectionist)
                        PrizeSelector(awardWinner);
                    else
                        awardWinner.personalityTrait++;
                    break;
            }
        }
        else
        {
            // We may have to display that they got a first round pick for the franchise
            // Award first round pick
            generalManager.firstRoundPicks++;
        }
    }

    public void ShowAwardWinnersList()
    {
        uiManager.BuildUI();
        ResetAwards();
    }

    private void ResetAwards()
    {
        REOTY = null;
        FOHEOTY = null;
        BOHEOTY = null;
        MVE = null;

        rookieEOTYNominations.Clear();
        frontOfHouseEOTYNominations.Clear();
        backOfHouseEOTYNominations.Clear();
        mostValuableEmployeeNominations.Clear();
        awardWinners.Clear();
    }
    #endregion
}
