using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Rest : MonoBehaviour
{
    public int healAmount = 5;
    public GameObject[] towersButtons;
    public Button[] buttons;
    public void heal()
    {
        GameStat.health += 5;
        GameStat.updateStats();
    }

    public void updateButtons()
    {
        for (int i = 0; i < towersButtons.Length; i++)
        {
            towersButtons[i].transform.GetChild(0).gameObject.GetComponent<TMP_Text>().SetText(GameStat.towersLevels[i].ToString());
            if (GameStat.blueprints < GameStat.towersLevels[i])
                towersButtons[i].GetComponent<Button>().interactable = false;
            else towersButtons[i].GetComponent<Button>().interactable = true;
        }
    }

    public void resetRest()
    {
        foreach (Button b in buttons)
        {
            b.interactable = true;
        }
        updateButtons();
    }

    public void towerUp(int index)
    {
        GameStat.blueprints -= GameStat.towersLevels[index];
        GameStat.towersLevels[index]++;
        GameStat.updateStats();
        updateButtons();
    }

}
