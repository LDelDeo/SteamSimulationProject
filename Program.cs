using System;
using static EmployeeFactory;

public class Program
{
    public static void Main()
    {
        // Create a new Factory
        var Factory = new EmployeeFactory();

        #region Team Generation   
        Factory.GenerateEmployees(Factory, 100000, "Roster");
        #endregion

        #region Season Simulation
        // Create a New Season
        var Season = new SeasonSimulation();

        // Simulate the Season with your Team
        //Season.SimulateSeason(Factory.MyEmployees);
        #endregion

        #region Draft Class Generation
        //Factory.GenerateEmployees(Factory, 25, "Prospect");
        #endregion

        #region Free Agent Class Generation
        //Factory.GenerateEmployees(Factory, 20, "FreeAgent");
        #endregion
    }
}

public class Employee
{
    #region Employee Variables
    public Gender gender;

    public string firstName;
    public string lastName;

    public bool isRookie;
    public int age;

    public JobType jobPosition;

    public WorkEthic developmentTrait;

    public int statOne;
    public int statTwo;
    public int statThree;
    public int statFour;
    public int statFive;

    public int[] employeeStats;

    public int overall;
    #endregion
}

public class EmployeeFactory
{
    #region Employee Creation
    public Employee CreateEmployee(string type)
    {
        Employee employee = new Employee();

        employee.gender = GetRandomEnumValue<Gender>();

        if (employee.gender == Gender.Male) { employee.firstName = maleNames[GetRandomNumberArray(maleNames)]; }
        else if (employee.gender == Gender.Female) { employee.firstName = femaleNames[GetRandomNumberArray(femaleNames)]; }

        if (type == "Roster" || type == "Prospect") { employee.isRookie = true; }
        else if (type == "FreeAgent") { employee.isRookie = false; }

        if (employee.isRookie) { employee.age = (int)GetRandomEnumValue<StartingAges>(); }
        else if (!employee.isRookie) { employee.age = (int)GetRandomEnumValue<MidCareerAges>(); }

        employee.jobPosition = GetRandomEnumValue<JobType>();
        employee.developmentTrait = GetRandomEnumValue<WorkEthic>();

        int maxStatValue = 101;
        employee.statOne = GetRandomNumber(maxStatValue);
        employee.statTwo = GetRandomNumber(maxStatValue);
        employee.statThree = GetRandomNumber(maxStatValue);
        employee.statFour = GetRandomNumber(maxStatValue);
        employee.statFive = GetRandomNumber(maxStatValue);

        employee.employeeStats = [employee.statOne, employee.statTwo, employee.statThree, employee.statFour, employee.statFive];

        int overallValue = (employee.statOne + employee.statTwo + employee.statThree + employee.statFour + employee.statFive) / 5;
        employee.overall = overallValue;

        // Adds to a specific list
        EmployeeType(type, employee);

        return employee;
    }
    #endregion

    #region Employee Statistic Printing
    public void PrintStats(Employee employee)
    {
        Console.WriteLine($"Name: {employee.firstName} {employee.lastName}");
        Console.WriteLine($"Gender: {employee.gender}");
        Console.WriteLine($"Age: {employee.age}");
        Console.WriteLine($"Is a Rookie: {employee.isRookie}");
        Console.WriteLine($"Job: {employee.jobPosition}");

        DevTraitColor(employee);
        Console.WriteLine($"Work Ethic: {employee.developmentTrait}");
        Console.ResetColor();

        Console.WriteLine($"Stats: {employee.statOne}, {employee.statTwo}, {employee.statThree}, {employee.statFour}, {employee.statFive}");
        Console.WriteLine($"Overall: {employee.overall}");
        Console.WriteLine("--------------------");
    }

    public void DraftBoard(Employee employee)
    {
        Console.WriteLine($"Name: {employee.firstName} {employee.lastName}");
        Console.WriteLine($"Gender: {employee.gender}");
        Console.WriteLine($"Age: {employee.age}");
        Console.WriteLine($"Job: {employee.jobPosition}");

        Console.WriteLine($"Work Ethic: ?");

        Console.WriteLine($"Stats: ");
        Random random = new Random();

        foreach (var employeeStat in employee.employeeStats)
        {
            int randomNumber = random.Next(2);

            if (randomNumber == 0) { Console.WriteLine(employeeStat); }
            else { Console.WriteLine("?"); }
        }

        Console.WriteLine($"Overall: ?");
        Console.WriteLine("--------------------");
    }

    private void DevTraitColor(Employee employee)
    {
        switch (employee.developmentTrait)
        {
            case WorkEthic.Bum:
                Console.ForegroundColor = ConsoleColor.White;
                return;
            case WorkEthic.Paycheck_Collector:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                return;
            case WorkEthic.Gets_The_Job_Done:
                Console.ForegroundColor = ConsoleColor.Green;
                return;
            case WorkEthic.Motivated:
                Console.ForegroundColor = ConsoleColor.Magenta;
                return;
            case WorkEthic.X_Factor:
                Console.ForegroundColor = ConsoleColor.Red;
                return;
        }
    }

    public void PrintHighestOverall(List<Employee> employeeList)
    {
        int highestOverall = 0;
        Employee bestEmployee = null;

        foreach (var employee in employeeList)
        {
            foreach (var employee2 in employeeList)
            {
                if (employee.overall > employee2.overall && employee.overall > highestOverall)
                {
                    highestOverall = employee.overall;
                    bestEmployee = employee;
                }
            }
        }

        Console.WriteLine($"Highest Overall in List: {highestOverall}");
        Console.WriteLine("Player Card");
        Console.WriteLine("===========");
        PrintStats(bestEmployee);
    }
    #endregion

    #region Employee Lists
    public List<Employee> MyEmployees = new List<Employee>();
    public List<Employee> DraftClass = new List<Employee>();
    public List<Employee> FreeAgentClass = new List<Employee>();
    private void EmployeeType(string type, Employee employee)
    {
        switch (type)
        {
            case "Roster":
                MyEmployees.Add(employee);
                return;

            case "Prospect":
                DraftClass.Add(employee);
                return;

            case "FreeAgent":
                FreeAgentClass.Add(employee);
                return;
        }
    }
    #endregion

    #region Arrays & Enums
    public string[] maleNames = ["Luke", "Jaxson", "Samuel", "James", "Mike", "Daniel", "Jared", "Aaron", "Ben", "Noah", "William"];
    public string[] femaleNames = ["Fallon", "Jenna", "Eden", "Emily", "Sophia", "Olivia", "Emma", "Ava", "Mia", "Charlotte", "Amelia", "Grace"];

    public enum StartingAges { Twenty_One = 21, Twenty_Two = 22, Twenty_Three = 23, Twenty_Four = 24 }
    public enum MidCareerAges { Twenty_Five = 25, Twenty_Six = 26, Twenty_Seven = 27, Twenty_Eight = 28, Twenty_Nine = 29, Thirty = 30, Thirty_One = 31, Thirty_Two = 32, Thirty_Three = 33 }
    public enum Gender { Male = 0, Female = 1 }
    public enum JobType { Fryer, Line_Cook, Head_Cook, Prep_Cook, Manager, Cashier, Janitor, Media_Manager, Drive_Thru_Attendee, Expediter, Shift_Supervisor }
    public enum WorkEthic { Bum, Paycheck_Collector, Gets_The_Job_Done, Motivated, X_Factor }
    #endregion

    #region Random Generators
    private static T GetRandomEnumValue<T>() where T : Enum
    {
        Random random = new Random();
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }

    private int GetRandomNumberArray(Array array)
    {
        Random random = new Random();
        int arrayItem = random.Next(array.Length);
        return arrayItem;
    }

    private int GetRandomNumber(int maxVal)
    {
        Random random = new Random();
        int randomNumber = random.Next(maxVal);
        return randomNumber;
    }
    #endregion

    #region Employee Generation
    public void GenerateEmployees(EmployeeFactory Factory, int amount, string type)
    {
        for (int i = 0; i < amount; i++)
            Factory.CreateEmployee(type);

        #region Roster
        foreach (var employee in Factory.MyEmployees)
            Factory.PrintStats(employee);
        #endregion

        #region Prospects
        foreach (var prospect in Factory.DraftClass)
            Factory.DraftBoard(prospect);

        foreach (var prospect in Factory.DraftClass)
            Factory.PrintStats(prospect);
        #endregion

        #region Free Agents
        foreach (var freeAgent in Factory.FreeAgentClass)
            Factory.PrintStats(freeAgent);
        #endregion

        // Use this to view the top employee in any list (Roster, Draft Class, Free Agents etc.)
        Factory.PrintHighestOverall(Factory.MyEmployees);
    }
    #endregion
}

public class SeasonSimulation
{
    #region Season Variables
    private int gamesInSeason = 17;
    private int gamesInPlayoffs = 4;

    private int wins;
    private int playoffWins;

    private int losses;
    private int playoffLosses;
    #endregion

    #region Simulation
    public void SimulateSeason(List<Employee> team)
    {
        int teamOverall = 0;

        int opponentDifficulty = 25;
        int playoffOpponentDifficulty = 30;

        foreach (Employee employee in team)
        {
            teamOverall += employee.overall;
        }

        Console.WriteLine("--------------------");
        Console.WriteLine($"Team Overall: {teamOverall}");
        Console.WriteLine("--------------------");

        for (int i = 0; i < gamesInSeason; i++)
        {
            var difficulty = team.Count * opponentDifficulty;
            SimulationAlgorithm(team, teamOverall, difficulty, false);
        }

        PrintRecord();

        if (wins >= 10)
        {
            for (int i = 0; i < gamesInPlayoffs; i++)
            {
                if (playoffWins < gamesInPlayoffs && playoffLosses == 0)
                {
                    var difficulty = team.Count * playoffOpponentDifficulty;
                    TypeOfPlayoffGame();
                    SimulationAlgorithm(team, teamOverall, difficulty, true);
                }
            }
        }
        else
        {
            Console.WriteLine("Missed Playoffs");
        }

    }

    private void SimulationAlgorithm(List<Employee> roster, int teamOverall, int opponentDifficulty, bool isPlayoffGame)
    {
        Random decideGame = new Random();
        var maxOpponentOverall = roster.Count * opponentDifficulty;
        var opponentOverall = decideGame.Next(maxOpponentOverall);
        Console.WriteLine($"Opponent Overall {opponentOverall}");

        if (teamOverall > opponentOverall && !isPlayoffGame) Win(false);
        else if (teamOverall <= opponentOverall && !isPlayoffGame) Loss(false);
        else if (teamOverall > opponentOverall && isPlayoffGame) Win(true);
        else if (teamOverall <= opponentOverall && isPlayoffGame) Loss(true);
    }
    #endregion

    #region Game Outcomes
    private void Win(bool isPlayoffWin)
    {
        if (isPlayoffWin)
        {
            Console.WriteLine("Playoff Win!");
            Console.WriteLine("--------------------");

            playoffWins++;

            if (playoffWins == 4)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("SUPERBOWL WIN!");
                Console.WriteLine("--------------------");
                return;
            }
        }
        else
        {
            Console.WriteLine("Win!");
            Console.WriteLine("--------------------");
            wins++;
        }
    }

    private void Loss(bool isPlayoffLoss)
    {
        if (isPlayoffLoss)
        {
            Console.WriteLine("Playoff Loss");
            Console.WriteLine("--------------------");
            playoffLosses++;
        }
        else
        {
            Console.WriteLine("Loss");
            Console.WriteLine("--------------------");
            losses++;
        }   
    }
    #endregion

    #region Simulation Printing
    private void TypeOfPlayoffGame()
    {
        switch (playoffWins)
        {
            case 0:
                Console.WriteLine("Wild Card Round: ");
                return;
            case 1:
                Console.WriteLine("Divisional Round: ");
                return;
            case 2:
                Console.WriteLine("Conference Championship Round: ");
                return;
            case 3: Console.WriteLine("Super Bowl Round: ");
                return;
        }
    }

    private void PrintRecord()
    {
        Console.WriteLine($"Record: {wins} - {losses}");
        Console.WriteLine("--------------------");
    }
    #endregion
}