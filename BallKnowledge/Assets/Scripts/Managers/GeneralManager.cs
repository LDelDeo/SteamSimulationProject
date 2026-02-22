using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    [Header("League Stats")]
    public int currentYear { get; set; } = 2025;

    [Header("Roster Stats")]
    public int playersCut { get; set; }
    public int playersTraded { get; set; }

    [Header("Draft Stats")]
    public int draftPicks { get; set; } = 3;
    public int playersDrafted { get; set; }

    [Header("Free Agency Stats")]
    public int currentUsedCapSpace { get; set; }
    public int maxCapSpace { get; private set; } = 275;

    [Header("Legacy Stats")]
    public int championshipsWon { get; set; }
    public int seasonsElapsed { get; set; }
}
