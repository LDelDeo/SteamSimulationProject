using UnityEngine;

public class DraftManager : MonoBehaviour
{
    [SerializeField] public int currentRound;
    [SerializeField] private int picksPerRound = 5;

    private EmployeeLists employeeLists;

    private void Start()
    {
        employeeLists = GetComponent<EmployeeLists>();  

        currentRound = 1;
    }

    public void AdvanceRound()
    {
        currentRound++;

        for (int i = 0; i < picksPerRound; i++)
        {
            int randomNumber = Random.Range(0, employeeLists.draftClass.Count);
            employeeLists.RemoveEmployee(employeeLists.draftClass[randomNumber], employeeLists.draftClass);
        }
    }
}
