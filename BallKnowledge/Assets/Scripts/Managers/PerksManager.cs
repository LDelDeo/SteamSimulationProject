using UnityEngine;

public class PerksManager : MonoBehaviour
{
    // We must find cool names for these, these are currently place holders
    [Header("General Perks")]
    public bool juiced; // More likely to generate X Factor employees (Draft, Free Agency, Trade Block)
    public bool doubled; // 2 Perk Tokens per year

    [Header("Retirement Perks")]
    public bool youth; // Less likely for employees under the age of 40 to want to retire
    public bool returnArtist; // More likely for employees that want to retire to return for additional years

    [Header("Free Agency Perks")]
    public bool theNegotiator; // Additional attempts at a contract negotation
    public bool largerPools; // Larger free agency classes

    [Header("Draft Perks")]
    public bool workEthics; // Reveal one prospects work ethic
    public bool potential; // Reveal one prospects overall
    public bool moreProspects; // Larger draft classes
    public bool investor; // Conditional 3rd round pick each year

    [Header("Resigning Perks")]
    public bool scamArtist; // More interested to resign for less money
    public bool tagged; // Franchise Tag unlock

    [Header("Trading Perks")]
    public bool takeGold; // First round picks are more valuable
    public bool takeSilver; // Second round picks are more valuable
    public bool takeBronze; // Third round picks are more valuable
    public bool treasure; // Employees fetch more pick value

    [Header("Event Perks")]
    public bool cultureArtist; // Less likely for toxic, diva & difficult employees to trigger an event

    [Header("Awards Perks")]
    public bool nominator; // More likely for employees to win awards
    public bool confidenceBooster; // Juiced prizes for award winners;
}
