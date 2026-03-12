using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    [Header("League Stats")]
    public int currentYear { get; set; } = 2025;

    [Header("Franchise Stats")]
    public string franchiseName { get; set; } = "My Franchise";

    [Header("Roster Move Stats")]
    public int playersCut { get; set; }
    public int tradesCompleted { get; set; }

    [Header("Draft Stats")]
    public int totalDraftPicks { get; set; }
    public int firstRoundPicks { get; set; } = 1;
    public int secondRoundPicks { get; set; } = 1;
    public int thirdRoundPicks { get; set; } = 1;
    public int playersDrafted { get; set; }

    [Header("Free Agency Stats")]
    public int currentUsedCapSpace { get; set; }
    public int maxCapSpace { get; set; } = 625;

    [Header("Legacy Stats")]
    public int championshipsWon { get; set; }
    public int seasonsElapsed { get; set; }
}
