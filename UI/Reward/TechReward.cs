using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;

[System.Serializable]
public class TechArray
{
    public Technology[] techArray = new Technology[3];
}

public class TechReward : MonoBehaviour
{
    public GameObject chestPanel;
    public float rareChance;
    public float LegendaryChance;
    public GameObject[] techPanels;
    public Image[] techRarityCircles;
    public Image[] towerTypeImages;
    public TMP_Text[] techDescriptions;
    public TMP_Text[] names;
    public TechArray[] techs;
    public GameObject previousPanel;
    private List<Technology> availableTechsCommon;
    private List<Technology> availableTechsRare;
    private List<Technology> availableTechsLegendary;
    private List<Technology>[] RaritiesLists;
    private Technology[] currentTechs;


    private void Awake()
    {
        currentTechs = new Technology[3];
        availableTechsCommon = new List<Technology>();
        availableTechsRare = new List<Technology>();
        availableTechsLegendary = new List<Technology>();
        RaritiesLists = new List<Technology>[3] {availableTechsCommon, availableTechsRare, availableTechsLegendary};
    }
    
    public void updateRewards()
    {
        availableTechsCommon.Clear();
        availableTechsRare.Clear();
        availableTechsLegendary.Clear();

        foreach (TechArray tech in techs)
        {
            for (int i = 0; i < tech.techArray.Length; i++)
            {
                if (!tech.techArray[i].isEnabled)
                {
                    RaritiesLists[tech.techArray[i].rarity].Add(tech.techArray[i]);
                }
                else Debug.Log("Already have");
            }
        }

        for (int i = 0; i < techDescriptions.Length; i++)
        {
            if (availableTechsCommon.Count == 0 && availableTechsRare.Count == 0 && availableTechsLegendary.Count == 0)
            {
                techPanels[i].SetActive(false);
            }
            else
            {
                int rarity = GetRandomRarity();
                if (RaritiesLists[rarity].Count != 0)
                {
                    GetRandomTechnology(rarity, i);
                        
                }
                else if (RaritiesLists[(rarity+1)%3].Count != 0)
                {
                    GetRandomTechnology((rarity+1)%3, i);
                }
                else
                {
                    GetRandomTechnology((rarity+2)%3, i);
                }
            }
        }
    }

    public int GetRandomRarity()
    {
        float random = Random.Range(0f, 1f);
        int rarity = 0;
        if (random >= 1-LegendaryChance) rarity = 2;
        else if (random >= 1 - rareChance) rarity = 1;
        return rarity;
    }

    public void GetRandomTechnology(int rarity, int i)
    {
        techPanels[i].SetActive(true);
        int random = 0;
        Technology tech = null;
        random = Random.Range(0, RaritiesLists[rarity].Count);
        tech = RaritiesLists[rarity][random];
        RaritiesLists[rarity].RemoveAt(random);
        techRarityCircles[i].sprite = tech.rarityCircle;
        towerTypeImages[i].sprite = tech.towerOutlined;
        towerTypeImages[i].SetNativeSize();
        techDescriptions[i].SetText(tech.description.GetLocalizedString());
        names[i].SetText(tech.techName.GetLocalizedString());
        currentTechs[i]= tech;
    }

    public void TechPicked(int i)
    {
        currentTechs[i].isEnabled = true;
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ResetTechs()
    {
        foreach(TechArray tech in techs)
        {
            for (int i = 0; i < tech.techArray.Length; i++)
            {
                tech.techArray[i].isEnabled = false;
            }
        }
    }

    public TechArray[] GetTechs()
    {
        return techs;
    }
}
