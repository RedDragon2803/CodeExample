using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTower : MonoBehaviour
{
    [SerializeField] private Sprite[] towersSprites;
    [SerializeField] private Tower[] towersScripts;
    [SerializeField] private SpriteRenderer radiusSprite;

    public void setTowerSprite(int id)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = towersSprites[id];
    }

    public void setRadColor(bool isEnough)
    {
        if (isEnough) radiusSprite.color = Color.green;
        else radiusSprite.color = Color.red;
    }

    public void setRadius(float rad)
    {
        transform.GetChild(0).localScale = new Vector3(rad, rad, 1);
    }
}
