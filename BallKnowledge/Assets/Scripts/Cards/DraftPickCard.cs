using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class DraftPickCard : MonoBehaviour
{
    [Header("Draft Pick Card Visuals")]
    [SerializeField] TMP_Text roundOfPickText;
    [SerializeField] TMP_Text yearOfPickText;
    [SerializeField] Image cardBackground;
    [SerializeField] GameObject addButton;
    [SerializeField] GameObject removeButton;

    public int thisPicksRound;
    public int thisPicksYear;

    public TradeManager tradeManager;
    public UIManager uiManager;

    public void SetValuesOfPick(int roundOfPick, int yearOfPick)
    {
        thisPicksRound = roundOfPick;
        thisPicksYear = yearOfPick;

        tradeManager = FindAnyObjectByType<TradeManager>();
        uiManager = FindAnyObjectByType<UIManager>();

        SetVisualsOfPick();
    }

    private void SetVisualsOfPick()
    {
        switch (thisPicksRound)
        {
            case 1: 
                roundOfPickText.text = "First Round Pick"; 
                cardBackground.color = new Color32(196, 167, 47, 255); // Gold
                break;
            case 2: 
                roundOfPickText.text = "Second Round Pick"; 
                cardBackground.color = new Color32(115, 114, 112, 255); // Light Grey
                break;
            case 3: 
                roundOfPickText.text = "Third Round Pick"; 
                cardBackground.color = new Color32(115, 59, 3, 255); // Brown
                break;
        }

        yearOfPickText.text = thisPicksYear.ToString();

        addButton.SetActive(true);
        removeButton.SetActive(false);
    }

    public void AddDraftPickToTradePackage()
    {
        if (tradeManager.TradePackageIsFull())
        {
            uiManager.TradePackageIsFull();
            return;
        }

        switch (thisPicksRound)
        {
            case 1:
                tradeManager.outgoingTradePackageValue.Add(tradeManager.firstRoundPickValue);
                tradeManager.outgoingDraftPicks.Add(1);
                break;
            case 2:
                tradeManager.outgoingTradePackageValue.Add(tradeManager.secondRoundPickValue);
                tradeManager.outgoingDraftPicks.Add(2);
                break;
            case 3:
                tradeManager.outgoingTradePackageValue.Add(tradeManager.thirdRoundPickValue);
                tradeManager.outgoingDraftPicks.Add(3);
                break;
        }

        addButton.SetActive(false);
        removeButton.SetActive(true);
    }

    public void RemoveDraftPickFromTradePackage()
    {
        switch (thisPicksRound)
        {
            case 1:
                tradeManager.outgoingTradePackageValue.Remove(tradeManager.firstRoundPickValue);
                tradeManager.outgoingDraftPicks.Remove(1);
                break;
            case 2:
                tradeManager.outgoingTradePackageValue.Remove(tradeManager.secondRoundPickValue);
                tradeManager.outgoingDraftPicks.Remove(2);
                break;
            case 3:
                tradeManager.outgoingTradePackageValue.Remove(tradeManager.thirdRoundPickValue);
                tradeManager.outgoingDraftPicks.Remove(3);
                break;
        }

        addButton.SetActive(true);
        removeButton.SetActive(false);
    }
}
