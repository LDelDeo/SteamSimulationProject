using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public List<Employee> latestDraftClass = new List<Employee>();

    public List<GameObject> prospectCardsSortingList = new List<GameObject>();

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
        else 
        { 
            DisplayFinalDraftClass();
        }

        uiManager.RefreshUI();
        uiManager.UpdateDraftPicks();
        uiManager.UpdateCapSpace();
    }

    private void DisplayFinalDraftClass()
    {
        uiManager.nextPeriodButton.SetActive(true);

        employeeLists.draftClass.Clear();
        uiManager.BuildUI();
        uiManager.draftScreen.GetComponent<ScrollRect>().content = uiManager.finalDraftClassContent.GetComponent<RectTransform>();
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
    // We should use a dropdown or a check box for ascending/descending order when sorting
    // Maybe sorting mutliple items at once as well (this would require checkboxes and not buttons)
    // We should also look into favoriting prospects
    private void AddProspectCardsToSortingList()
    {
        foreach (Transform prospectCard in uiManager.prospectContent)
        {
            var cardObject = prospectCard.gameObject;
            prospectCardsSortingList.Add(cardObject);
        }
    }

    public void SortByPosition()
    {
        AddProspectCardsToSortingList();

        var sorted = prospectCardsSortingList.OrderByDescending
            (prospectCard => prospectCard.GetComponent<ProspectCard>().jobType).ToList();

        for (int i = 0; i < prospectCardsSortingList.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    public void SortByPersonalityTrait()
    {
        AddProspectCardsToSortingList();

        var sorted = prospectCardsSortingList.OrderByDescending
            (prospectCard => prospectCard.GetComponent<ProspectCard>().personalityTrait).ToList();

        for (int i = 0; i < prospectCardsSortingList.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    public void SortByRevealedDevelopmentTrait()
    {
        AddProspectCardsToSortingList();

        var sorted = prospectCardsSortingList.OrderByDescending
            (prospectCard => prospectCard.GetComponent<ProspectCard>().developmentTraitRevealed).ToList();
        
        for (int i = 0; i < prospectCardsSortingList.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    public void SortByRevealedOverall()
    {
        AddProspectCardsToSortingList();

        var sorted = prospectCardsSortingList.OrderByDescending
            (prospectCard => prospectCard.GetComponent<ProspectCard>().overallRevealed).ToList();

        for (int i = 0; i < prospectCardsSortingList.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }
    #endregion
}
