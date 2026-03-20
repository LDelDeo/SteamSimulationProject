using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DraftManager : MonoBehaviour
{
    // Allow employees to trade employees for picks during the draft
    // Show the screen of assets for picks, remove the back button, Get Offers button is non interactable for rookies

    // We should hide the work ethic, overall and stats of rookie employees in roster screen during the draft

    // We could also display the top 3 positional needs
    [Header("Draft Period Configuration")]
    public int draftClassSize;
    public int currentRound;
    [SerializeField] int picksInBetweenRounds;
    [SerializeField] int maxOverallInRoundTwo;
    [SerializeField] int maxOverallInRoundThree;
    [SerializeField] int additionalCapSpacePerFirstRoundPick; 
    [SerializeField] int additionalCapSpacePerSecondRoundPick; 
    [SerializeField] int additionalCapSpacePerThirdRoundPick;
    [SerializeField] int amountOfTopProspectsToDisplay;

    [Header("Draft Pick Configuration")]
    public int firstRoundPicksRecouped;
    public int secondRoundPicksRecouped;
    public int thirdRoundPicksRecouped;

    public List<Employee> latestDraftClass = new List<Employee>();
    public List<Employee> topOverallProspects = new List<Employee>();

    public List<GameObject> prospectCardsSortingList = new List<GameObject>();
    private EmployeeEnumerators.JobType jobToSortBy;

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
    private void AdvanceRound()
    {
        // If you advance a round with left over draft picks in the current round, you can increase your max cap space by an X amount depending on the value of the pick
        TradingLeftOverPicks(); 

        if (currentRound < 3)
        {
            currentRound++;

            for (int i = 0; i < picksInBetweenRounds; i++) // Removes a set amount of prospects in between user picks to simulate a real snake draft
            {
                int randomNumber = UnityEngine.Random.Range(0, employeeLists.draftClass.Count);
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
            uiManager.topProspectsContent.gameObject.SetActive(true);
        }

        uiManager.RefreshUI();
        uiManager.UpdateDraftPicks();
        uiManager.UpdateCapSpace();
    }

    public void RequestPicksForCapSpace()
    {
        int capSpaceAdded = 0;

        switch (currentRound)
        {
            case 1:
                if (manager.firstRoundPicks > 0)
                {
                    for (int i = 0; i < manager.firstRoundPicks; i++)
                        capSpaceAdded += additionalCapSpacePerFirstRoundPick;

                    uiManager.AttemptToSkipDraftRound(AdvanceRound, manager.firstRoundPicks, "first", capSpaceAdded);
                }
                else AdvanceRound();
                break;

            case 2:
                if (manager.secondRoundPicks > 0)
                {
                    for (int i = 0; i < manager.secondRoundPicks; i++)
                        capSpaceAdded += additionalCapSpacePerSecondRoundPick;

                    uiManager.AttemptToSkipDraftRound(AdvanceRound, manager.secondRoundPicks, "second", capSpaceAdded);
                }
                else AdvanceRound();
                break;

            case 3:
                if (manager.thirdRoundPicks > 0)
                {
                    for (int i = 0; i < manager.thirdRoundPicks; i++)
                        capSpaceAdded += additionalCapSpacePerThirdRoundPick;

                    uiManager.AttemptToSkipDraftRound(AdvanceRound, manager.thirdRoundPicks, "third", capSpaceAdded);
                }
                else AdvanceRound();
                break;
        }
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

                    uiManager.DraftPicksForCapSpace(manager.thirdRoundPicks, "third", capSpaceAdded);
                    manager.thirdRoundPicks = 0;
                }
                break;
        }

        uiManager.UpdateCapSpace();
    }
    #endregion

    #region Post-Draft Lists

    private void DisplayFinalDraftClass()
    {
        uiManager.nextPeriodButton.SetActive(true);

        employeeLists.draftClass.Clear();
        uiManager.BuildUI();
        uiManager.draftScreen.GetComponent<ScrollRect>().content = uiManager.finalDraftClassContent.GetComponent<RectTransform>();
    }

    public void GrabTopProspects()
    {
        var sortedByOverall = employeeLists.draftClass.OrderByDescending(prospect => prospect.overall).ToList();

        for (int i = 0; i < amountOfTopProspectsToDisplay; i++)
            topOverallProspects.Add(sortedByOverall[i]);  
        
        uiManager.topProspectsContent.gameObject.SetActive(false);
    }
    #endregion

    #region List Sorting
    private void AddProspectCardsToSortingList()
    {
        foreach (Transform prospectCard in uiManager.prospectContent)
        {
            var cardObject = prospectCard.gameObject;
            prospectCardsSortingList.Add(cardObject);
        }
    }

    public void SortByFavorites()
    {
        AddProspectCardsToSortingList();

        NothingToSort("Favorites", "No Favorited Prospects to Sort");

        var sorted = prospectCardsSortingList.OrderByDescending
            (prospectCard => prospectCard.GetComponent<ProspectCard>().isFavorited).ToList();

        for (int i = 0; i < sorted.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    public void SetJobPositionToSortBy()
    {
        int dropdownIndex = uiManager.positionDropDown.value;

        switch (dropdownIndex)
        {
            case 0: jobToSortBy = EmployeeEnumerators.JobType.Busser; break;
            case 1: jobToSortBy = EmployeeEnumerators.JobType.Janitor; break;
            case 2: jobToSortBy = EmployeeEnumerators.JobType.Drive_Thru_Attendee; break;
            case 3: jobToSortBy = EmployeeEnumerators.JobType.Cashier; break;
            case 4: jobToSortBy = EmployeeEnumerators.JobType.Media_Manager; break;
            case 5: jobToSortBy = EmployeeEnumerators.JobType.Prep_Cook; break;
            case 6: jobToSortBy = EmployeeEnumerators.JobType.Line_Cook; break;
            case 7: jobToSortBy = EmployeeEnumerators.JobType.Fry_Cook; break;
            case 8: jobToSortBy = EmployeeEnumerators.JobType.Patty_Flipper; break;
            case 9: jobToSortBy = EmployeeEnumerators.JobType.Expediter; break;
            case 10: jobToSortBy = EmployeeEnumerators.JobType.Shift_Manager; break;
            case 11: jobToSortBy = EmployeeEnumerators.JobType.Manager; break;
        }
    }
    
    public void SortByPosition()
    {
        foreach (Transform prospectCard in uiManager.prospectContent)
        {
            var cardObject = prospectCard.gameObject;
            var cardScript = cardObject.GetComponent<ProspectCard>();

            if (cardScript.jobTypeToSort == jobToSortBy)
                prospectCardsSortingList.Add(cardObject);
        }

        var sorted = prospectCardsSortingList.OrderByDescending
            (prospectCard => prospectCard.GetComponent<ProspectCard>().jobTypeToSort).ToList();

        for (int i = 0; i < sorted.Count; i++)
                sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    public void SortByPersonalityTrait()
    {
        AddProspectCardsToSortingList();

        var sorted = prospectCardsSortingList.OrderByDescending
            (prospectCard => prospectCard.GetComponent<ProspectCard>().personalityTraitToSort).ToList();

        for (int i = 0; i < sorted.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    public void SortByRevealedWorkEthics()
    {
        AddProspectCardsToSortingList();

        NothingToSort("WorkEthics", "No Prospects with Revealed Work Ethics");

        var sorted = prospectCardsSortingList
            .OrderByDescending(prospectCard => prospectCard.GetComponent<ProspectCard>().workEthicsRevealed)
            .ThenByDescending(prospectCard => prospectCard.GetComponent<ProspectCard>().workEthicToSort)
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    public void SortByRevealedOverall()
    {
        AddProspectCardsToSortingList();

        NothingToSort("Overall", "No Prospects with Revealed Overall");

        var sorted = prospectCardsSortingList
            .OrderByDescending(prospectCard => prospectCard.GetComponent<ProspectCard>().overallRevealed)
            .ThenByDescending(prospectCard => prospectCard.GetComponent<ProspectCard>().overallToSort)
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);

        prospectCardsSortingList.Clear();
    }

    private void NothingToSort(string varToSort, string textToDisplay)
    {
        var prospectsToSort = 0;

        foreach (Transform prospectCard in uiManager.prospectContent)
        {
            ProspectCard cardObject = prospectCard.gameObject.GetComponent<ProspectCard>();
            
            switch (varToSort)
            {
                case "Favorites": if (cardObject.isFavorited) prospectsToSort++; break;
                case "WorkEthics": if (cardObject.workEthicsRevealed) prospectsToSort++; break;
                case "Overall": if (cardObject.overallRevealed) prospectsToSort++; break;
            }
        }

        if (prospectsToSort == 0)
        {
            uiManager.GenericText(textToDisplay);
            prospectCardsSortingList.Clear();
            return;
        }
    }
    #endregion
}
