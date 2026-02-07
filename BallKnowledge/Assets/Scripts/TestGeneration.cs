using UnityEngine;

public class TestGeneration : MonoBehaviour
{
    [SerializeField] int employeeCount;
    private void Start()
    {
        EmployeeFactory employeeFactory = new EmployeeFactory();

        for (int i = 0; i < employeeCount; i++)
        {
            employeeFactory.CreateEmployee();
        }
            
    }
}
