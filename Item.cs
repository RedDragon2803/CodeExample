using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public LocalizedString name;
    public string description;
    public float buffPerItem;
    public float startBuff;
    public Sprite art;
    public int price;
    public int rarity;
}
