using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class TowerCost : MonoBehaviour
{
     public LocalizeStringEvent localizedStringEvent;
    public Tower towerScript;
    public int cost;

    private void Start() 
    {
        cost = towerScript.cost;
        localizedStringEvent.RefreshString();
    }
}
