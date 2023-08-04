using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;

public class EventManager : MonoBehaviour
{
    public Event[] events;
    public TMP_Text description;
    public TMP_Text[] choiceTexts;
    public GameObject choicesPanel;
    public GameObject continuePanel;
    private int currentEvent;
    private int currentChoice;
    public GameObject TechRewardPanel;
    private TechReward techRewardScript;
    public GameObject levelPanel;
    public GameObject[] choices;

    private void Awake()
    {
        techRewardScript = TechRewardPanel.GetComponent<TechReward>();
    }
    public void SetRandomEvent()
    {
        foreach (GameObject choice in choices) choice.SetActive(true);
        currentEvent = Random.Range(0, events.Length);
        description.SetText(events[currentEvent].description.GetLocalizedString());
        choiceTexts[0].SetText(events[currentEvent].choice1.GetLocalizedString());
        choiceTexts[1].SetText(events[currentEvent].choice2.GetLocalizedString());
        choiceTexts[2].SetText(events[currentEvent].choice3.GetLocalizedString());
        if (choiceTexts[2].text == " ") choices[2].SetActive(false);
        choicesPanel.SetActive(true);
        continuePanel.SetActive(false);
    }
    public void ChoicePressed(int choice)
    {
        currentChoice = choice;
        switch (currentChoice)
        {
            case 1:
                description.SetText(events[currentEvent].endDescription1.GetLocalizedString());
                break;
            case 2:
                description.SetText(events[currentEvent].endDescription2.GetLocalizedString());
                break;
            case 3:
                description.SetText(events[currentEvent].endDescription3.GetLocalizedString());
                break;
        }
        choicesPanel.SetActive(false);
        continuePanel.SetActive(true);
    }

    public void ContinuePressed()
    {
        Invoke("RE"+(currentEvent+1).ToString()+"_"+currentChoice.ToString(), 0f);
        gameObject.SetActive(false);
    }

    private void TakeTech()
    {
        TechRewardPanel.SetActive(true);
        techRewardScript.updateRewards();
        techRewardScript.previousPanel = levelPanel;
    }

    private void RE1_1()
    {
        GameStat.health -= 2;
        GameStat.money += 300;
        GameStat.updateStats();
        levelPanel.SetActive(true);
    }
    private void RE1_2()
    {
        float random = Random.Range(0f,100f);
        if (random > 70)
        {
            TakeTech();
        }
        else
        {
            GameStat.health += 2;
            GameStat.updateStats();
            levelPanel.SetActive(true);
        }
    }
    private void RE1_3()
    {
        levelPanel.SetActive(true);
    }

    private void RE2_1()
    {
        TakeTech();
    }
    private void RE2_2()
    {
        GameStat.money -= 100;
        GameStat.updateStats();
        TakeTech();
    }
    private void RE2_3()
    {
        levelPanel.SetActive(true);
    }

    private void RE3_1()
    {
        GameStat.health -= 2;
        GameStat.updateStats();
        levelPanel.SetActive(true);
    }
    private void RE3_2()
    {
        GameStat.money -= 100;
        GameStat.updateStats();
        levelPanel.SetActive(true);
    }
}
