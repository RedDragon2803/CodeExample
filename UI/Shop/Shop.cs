using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopItemPrefub;
    public ItemsManager IM;
    private Transform shopPanel;

    private void Awake() {
        shopPanel = transform.GetChild(1);
    }
    public void NewShop(int itemsCount)
    {
        ShopClear();
        for (int i = 0; i < itemsCount; i++)
        {
            GameObject newItem;
            newItem = Instantiate(shopItemPrefub, shopPanel);
            Item item = IM.Items[Random.Range(0, IM.Items.Length)];
            newItem.GetComponent<ShopItem>().DataUpdate(item.name, item.itemName, item.price.ToString(), item.price, item.art);
        }
        
    }

    public void ShopClear()
    {
        for (int i = 0; i < shopPanel.childCount; i++)
        {
            Destroy(shopPanel.GetChild(i).gameObject);
        }
    }

    public void UpdateInteractable()
    {
        for (int i = 0; i < shopPanel.childCount; i++)
        {
            shopPanel.GetChild(i).GetComponent<ShopItem>().PriceCheck();
        }
    }
}
