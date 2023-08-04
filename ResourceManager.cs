using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class ResourceManager : MonoBehaviour
{
    public LocalizeStringEvent localizedStringEvent;

    public int money;
    public float moneyEarnTime = 1f;
    public int moneyEarn = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Farming());
    }

    IEnumerator Farming()
    {
        while (true)
        {
            money += moneyEarn;
            localizedStringEvent.RefreshString();
            yield return new WaitForSeconds(moneyEarnTime);
        }
    }

    public void UpdateResources()
    {
        localizedStringEvent.RefreshString();
    }
}
