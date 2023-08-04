using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;

public class RewardItem : MonoBehaviour
{
    public TMP_Text rewardItemName;
    public TMP_Text rewardItemCountText;
    public int rewardItemCount;
    public Image rewardItemImage;
    public string rewardType;

    public ItemsManager itemsManager;
    public Chest chest;
    public GameObject chestPanel;
    public GameObject technologiesPanel;

    private string idName;

    
    public void DataUpdate (Chest chestScript, string name, LocalizedString localizedName, string countString, int count, Sprite sprite, string type)
    {
        idName = name;
        chest = chestScript;
        chestPanel = chest.gameObject;
        technologiesPanel = chest.technologiesPanel;
        itemsManager = chest.IM;
        rewardType = type;
        rewardItemName.SetText(localizedName.GetLocalizedString());
        rewardItemCount = count;
        if (rewardItemCount > 1)
            rewardItemCountText.SetText(countString);
        else rewardItemCountText.SetText("");
        
        rewardItemImage.sprite = sprite;
        rewardItemImage.SetNativeSize();
    }

    public void GetRewardItem()
    {
        switch (rewardType)
        {
            case "Item":
                itemsManager.ItemsCount[idName] += rewardItemCount;
                itemsManager.ArtifactsPanelUpdate();
                break;
            case "Money":
                GameStat.money += rewardItemCount;
                GameStat.updateStats();
                break;
            case "Blueprint":
                GameStat.blueprints += rewardItemCount;
                GameStat.updateStats();
                break;
            case "Tech":
                technologiesPanel.SetActive(true);
                TechReward techReward = technologiesPanel.GetComponent<TechReward>();
                techReward.updateRewards();
                techReward.previousPanel = chestPanel;
                chestPanel.SetActive(false);
                break;
        }
        
        Destroy(gameObject);
    }
}
