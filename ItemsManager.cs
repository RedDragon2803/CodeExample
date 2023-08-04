using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemsManager : MonoBehaviour
{
    public GameObject[] ArtifactsIcons;
    //crit
    public float towersCritChance = 0;
    public float towersCritDamageMod = 2;

    //attackSpeed
    public float attackSpeedMod = 1;

    //attackRad
    public float attackRadMod = 1;

    //poison
    public int maxPoisonStacks;
    public float PoisonDamage = 5;
    public Item[] Items;
    public Dictionary<string, int> ItemsCount;
    // Start is called before the first frame update
    void Start()
    {
        ItemsCount = new Dictionary<string, int>();
        foreach(Item item in Items)
        {
            ItemsCount.Add(item.itemName, 0);
        }


        if (ItemsCount["4 leaf clover"] > 0) towersCritChance = Items[0].startBuff + (ItemsCount["4 leaf clover"]-1)*Items[0].buffPerItem;
        if (ItemsCount["AtkSpeedMod"] > 0) attackSpeedMod = 1 - (ItemsCount["AtkSpeedMod"]-1)*Items[1].buffPerItem - Items[1].startBuff;
        if (ItemsCount["Optical Sight"] > 0) attackRadMod = 1 + (ItemsCount["Optical Sight"]-1)*Items[2].buffPerItem + Items[2].startBuff;
        if (ItemsCount["Poison"] > 0) maxPoisonStacks = (ItemsCount["Poison"]-1)*(int)Items[3].buffPerItem + (int)Items[3].startBuff;

        ArtifactsPanelUpdate();
    }

    public void ArtifactsPanelUpdate()
    {
        int i = 0;
        foreach (KeyValuePair<string, int> item in ItemsCount)
        {
            if (item.Value > 1)
            {
                ArtifactsIcons[i].SetActive(true);
                ArtifactsIcons[i].transform.GetChild(0).gameObject.SetActive(true);
                ArtifactsIcons[i].GetComponentInChildren<TMP_Text>().SetText(item.Value.ToString());
            }
            else if (item.Value == 1)
            {
                ArtifactsIcons[i].SetActive(true);
                ArtifactsIcons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                ArtifactsIcons[i].SetActive(false);
            }
            i++;
        }

        ArtifactsStatsUpdate();
    }

    public void ArtifactsStatsUpdate()
    {
        if (ItemsCount["4 leaf clover"] > 0) towersCritChance = Items[0].startBuff + (ItemsCount["4 leaf clover"]-1)*Items[0].buffPerItem;
        if (ItemsCount["AtkSpeedMod"] > 0) attackSpeedMod = 1 - (ItemsCount["AtkSpeedMod"]-1)*Items[1].buffPerItem - Items[1].startBuff;
        if (ItemsCount["Optical Sight"] > 0) attackRadMod = 1 + (ItemsCount["Optical Sight"]-1)*Items[2].buffPerItem + Items[2].startBuff;
        if (ItemsCount["Poison"] > 0) maxPoisonStacks = (ItemsCount["Poison"]-1)*(int)Items[3].buffPerItem + (int)Items[3].startBuff;
    }
}
