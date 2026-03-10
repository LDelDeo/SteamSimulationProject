using System.Linq;
using UnityEngine;

public class DraftManager : MonoBehaviour
{
    [Header("Draft Period Configuration")]
    [SerializeField] public int draftClassSize;
    [SerializeField] public int currentRound;
    [SerializeField] int picksInBetweenRounds;
    [SerializeField] int maxOverallInRoundTwo;
    [SerializeField] int maxOverallInRoundThree;

    private EmployeeLists employeeLists;
    private UIManager uiManager;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();  
        uiManager = GetComponent<UIManager>();

        currentRound = 1;
    }

    public void AdvanceRound()
    {
        // We must do something about unused draft picks, trading back? Gaining something in return for not using?
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

            uiManager.RefreshUI();
        }
        else
        {
            uiManager.nextPeriodButton.SetActive(true);
        }
    }
}
