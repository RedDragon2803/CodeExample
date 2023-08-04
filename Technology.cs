using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewTech", menuName = "Tech")]
public class Technology : ScriptableObject
{
    public int id;
    public LocalizedString techName;
    public int towerID;
    public Sprite towerOutlined;
    public int rarity;
    public Sprite rarityCircle;
    public LocalizedString description;
    public float effectStrength;
    public bool isEnabled = false;
}
