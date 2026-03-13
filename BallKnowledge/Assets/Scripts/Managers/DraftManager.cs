using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class DraftManager : MonoBehaviour
{
    [Header("Draft Period Configuration")]
    [SerializeField] public int draftClassSize;
    [SerializeField] public int currentRound;
    [SerializeField] int picksInBetweenRounds;
    [SerializeField] int maxOverallInRoundTwo;
    [SerializeField] int maxOverallInRoundThree;
    [SerializeField] int additionalCapSpacePerFirstRoundPick; 
    [SerializeField] int additionalCapSpacePerSecondRoundPick; 
    [SerializeField] int additionalCapSpacePerThirdRoundPick; 

    // At the end of the draft we must display each employee that was drafted in each round and their stats
    // We can add them to this list as well as current roster and when the user advances to the next period we just clear this list
    public List<Employee> latestDraftClass = new List<Employee>();

    private EmployeeLists employeeLists;
    private UIManager uiManager;
    private GeneralManager manager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();  
        uiManager = GetComponent<UIManager>();
        manager = GetComponent<GeneralManager>();

        currentRound = 1;
    }

    #region Drafting Functionality
    public void AdvanceRound()
    {
        // If you advance a round with left over draft picks in the current round, you can increase your max cap space by an X amount depending on the value of the pick
        TradingLeftOverPicks(); 

        if (currentRound < 3)
        {
            currentRound++;
            uiManager.currentRoundText.text = $"Round {currentRound}";

            for (int i = 0; i < picksInBetweenRounds; i++) // Removes a set amount of prospects in between user picks to simulate a real snake draft
            {
                int randomNumber = Random.Range(0, employeeLists.draftClass.Count);
                employeeLists.RemoveEmployee(employeeLists.draftClass[randomNumber], employeeLists.draftClass);
            }

            foreach (Employee prospect in employeeLists.draftClass.ToList()) // For realism, the max overall a prospect can be is capped by the current round
            {
                if (currentRound == 2 && prospect.overall >= maxOverallInRoundTwo)
                    employeeLists.RemoveEmployee(prospect, employeeLists.draftClass);
                else if (currentRound == 3 && prospect.overall >= maxOverallInRoundThree)
                    employeeLists.RemoveEmployee(prospect, employeeLists.draftClass);
            }
        }
        else { uiManager.nextPeriodButton.SetActive(true); }

        //uiManager.RefreshProspectStatus();
        uiManager.UpdateDraftPicks();
        uiManager.UpdateCapSpace();
    }

    private void TradingLeftOverPicks()
    {
        int capSpaceAdded = 0;

        switch (currentRound)
        {
            case 1:
                if (manager.firstRoundPicks > 0)
                {
                    for (int i = 0; i < manager.firstRoundPicks; i++)
                    {
                        manager.maxCapSpace += additionalCapSpacePerFirstRoundPick;
                        capSpaceAdded += additionalCapSpacePerFirstRoundPick;
                    }

                    uiManager.DraftPicksForCapSpace(manager.firstRoundPicks, "first", capSpaceAdded);
                    manager.firstRoundPicks = 0;
                }
                break;
            case 2:
                if (manager.secondRoundPicks > 0)
                {
                    for (int i = 0; i < manager.secondRoundPicks; i++)
                    {
                        manager.maxCapSpace += additionalCapSpacePerSecondRoundPick;
                        capSpaceAdded += additionalCapSpacePerSecondRoundPick;
                    }

                    uiManager.DraftPicksForCapSpace(manager.secondRoundPicks, "second", capSpaceAdded);
                    manager.secondRoundPicks = 0;
                } 
                break;

            case 3:
                if (manager.thirdRoundPicks > 0)
                {
                    for (int i = 0; i < manager.thirdRoundPicks; i++)
                    {
                        manager.maxCapSpace += additionalCapSpacePerThirdRoundPick;
                        capSpaceAdded += additionalCapSpacePerThirdRoundPick;
                    }

                    uiManager.DraftPicksForCapSpace(manager.thirdRoundPicks, "thrid", capSpaceAdded);
                    manager.thirdRoundPicks = 0;
                }
                break;
        }

        uiManager.UpdateCapSpace();
    }
    #endregion

    #region List Sorting
    // We must make a sorting button to be able to sort by: Position, Revealed Dev Trait, Revealed Overall, Drafted, Not Drafted etc.
    public void SortByPosition()
    {

    }

    public void SortByRevealedDevelopmentTrait()
    {

    }

    public void SortByRevealedOverall()
    {

    }

    public void SortByDrafted()
    {

    }

    public void SortByAvailable()
    {

    }
    #endregion
}
