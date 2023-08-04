using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;

public class ShopItem : MonoBehaviour
{
    public TMP_Text shopItemName;
    public string itemName;
    public TMP_Text shopItemPriceText;
    public int shopItemPrice;
    public Image shopItemImage;

    public ItemsManager itemsManager;
    public Shop shop;

    
    public void DataUpdate (LocalizedString name, string idName, string priceString, int price, Sprite sprite)
    {
        itemName = idName;
        shopItemName.SetText(name.GetLocalizedString());
        shopItemPriceText.SetText(priceString);
        shopItemPrice = price;
        shopItemImage.sprite = sprite;
        shopItemImage.SetNativeSize();
        PriceCheck();
    }

    void Awake()
    {
        shop = gameObject.GetComponentInParent<Shop>();
        itemsManager = shop.IM;
    }

    public void itemBuy ()
    {
        itemsManager.ItemsCount[itemName]++;
        GameStat.money -= shopItemPrice;
        GameStat.updateStats();
        shop.UpdateInteractable();
        itemsManager.ArtifactsPanelUpdate();
        Destroy(gameObject);
    }

    public void PriceCheck()
    {
        if (GameStat.money < shopItemPrice) gameObject.GetComponent<Button>().interactable = false;
        else gameObject.GetComponent<Button>().interactable = true;
    }
}
