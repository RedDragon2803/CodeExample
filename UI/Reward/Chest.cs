using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;

public class Chest : MonoBehaviour
{
    public GameObject chestItemPrefub;
    public ItemsManager IM;
    private Transform chestPanel;

    public Sprite moneySprite;
    public Sprite blueprintSprite;
    public Sprite techSprite;
    public GameObject technologiesPanel;

    public LocalizedString[] localizedStrings;

    private void Awake() {
        chestPanel = transform.GetChild(1);
        IM = GameObject.FindGameObjectsWithTag("ItemsManager")[0].GetComponent<ItemsManager>();
    }

    public void NewChest(int itemsCount, int moneyCount = 0, int blueprintsCount = 0, int techCount = 0)
    {
        ChestClear();

        if (techCount > 0)
        {
            for (int i = 0; i < techCount; i++)
            {
                GameObject newItem;
                newItem = Instantiate(chestItemPrefub, chestPanel);
                newItem.GetComponent<RewardItem>().DataUpdate(this, "Technology", localizedStrings[0], techCount.ToString(), techCount, techSprite, "Tech");
            }
        }

        for (int i = 0; i < itemsCount; i++)
        {
            GameObject newItem;
            newItem = Instantiate(chestItemPrefub, chestPanel);
            Item item = IM.Items[Random.Range(0, IM.Items.Length)];
            newItem.GetComponent<RewardItem>().DataUpdate(this, item.itemName, item.name, "1", 1, item.art, "Item");
        }

        if  (moneyCount > 0)
        {
            GameObject newItem;
            newItem = Instantiate(chestItemPrefub, chestPanel);
            newItem.GetComponent<RewardItem>().DataUpdate(this, "Aurit", localizedStrings[1], moneyCount.ToString(), moneyCount, moneySprite, "Money");
        }

        if (blueprintsCount > 0)
        {
            GameObject newItem;
            newItem = Instantiate(chestItemPrefub, chestPanel);
            newItem.GetComponent<RewardItem>().DataUpdate(this, "Blueprint", localizedStrings[2], blueprintsCount.ToString(), blueprintsCount, blueprintSprite, "Blueprint");
        }
        
    }

    public void ChestClear()
    {
        for (int i = 0; i < chestPanel.childCount; i++)
        {
            Destroy(chestPanel.GetChild(i).gameObject);
        }
    }
}
