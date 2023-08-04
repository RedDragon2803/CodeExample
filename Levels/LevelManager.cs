using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LevelManager : MonoBehaviour
{
    public AudioManager am;

    public TMP_Text MoneyCount;
    public TMP_Text BlueprintsCount;
    public TMP_Text HealthCount;
    public TMP_Text ShieldCount;
    public int startHealth;
    public int startShield;
    public int startMoney;
    public int startBlueprints;

    public int currentPos;
    private int roomCompleted = -1;
    public bool isFirstLoad = true;
    public int rows;
    public int cols;
    
    public int fightsMin;
    public int fightsMax;
    public int fightsEliteMin;
    public int fightsEliteMax;
    public int shopMin;
    public int shopMax;
    public int eventsMin;
    public int eventsMax;
    public int restsMin;
    public int restsMax;
    public int chestsMin;
    public int chestMax;
    int totalMins;
    int generatedCount = 0;
    bool roadSpawned = false;

    public GameObject Main;
    public GameObject GameOverPanel;
    public GameObject panel;
    public GameObject fight;
    public GameObject fightElite;
    public GameObject eventPoint;
    public GameObject shop;
    public GameObject rest;
    public GameObject empty;
    public GameObject first;
    public GameObject boss;
    public GameObject chest;
    public GameObject techRewardPanel;
    public GameObject shopPanel;
    public int shopItemsCount = 12;
    public GameObject chestPanel;
    public int chestItemsCount = 1;
    public GameObject eventPanel;
    public GameObject restPanel;

    public float roadChance = 0.3f;

    public string[] fightMaps;
    public string[] eliteFightMaps;
    public string[] bossFightMaps;

    struct Point
    {
        public GameObject type;
        public int min;
        public int max;
        public int count;
        
        public Point (GameObject pType, int pMin, int pMax)
        {
            type = pType;
            min = pMin;
            max = pMax;
            count = 0;
        }
    }

    Point[] pointsArr = new Point[6];
    List<int> minPList;
    List<int> maxPList;

    // Start is called before the first frame update
    void Awake()
    {
        am = FindObjectOfType<AudioManager>();
        techRewardPanel.GetComponent<TechReward>().ResetTechs();
        currentPos = 1;
        GameStat.health = startHealth;
        GameStat.shield = startShield;
        GameStat.money = startMoney;
        GameStat.blueprints = startBlueprints;
        GameStat.currentLevel = SceneManager.GetActiveScene().name;
        GameStat.ShieldCount = ShieldCount;
        GameStat.HealthCount = HealthCount;
        GameStat.MoneyCount = MoneyCount;
        GameStat.BlueprintsCount = BlueprintsCount;
        GameStat.levelManager = this;
        GameStat.updateStats();

        pointsArr[0] = new Point(fight, fightsMin, fightsMax);
        pointsArr[1] = new Point(fightElite, fightsEliteMin, fightsEliteMax);
        pointsArr[2] = new Point(eventPoint, eventsMin, eventsMax);
        pointsArr[3] = new Point(shop, shopMin, shopMax);
        pointsArr[4] = new Point(rest, restsMin, restsMax);
        pointsArr[5] = new Point(chest, chestsMin, chestMax);
        minPList = new List<int>() {0, 1, 2, 3, 4, 5};
        maxPList = new List<int>() {0, 1, 2, 3, 4, 5};
        totalMins = shopMin+restsMin+chestsMin+eventsMin+fightsMin+fightsEliteMin;
        GenerateMap();
        isFirstLoad = false;
    }

    void Start()
    {
        GameStat.Main = Main;
        GameStat.fightModificatorID = 0;
    }

    void GenerateMap()
    {
        //first col
        Instantiate(empty, panel.transform);
        Instantiate(first, panel.transform);
        Instantiate(empty, panel.transform);
        for (int i = 1; i < cols-1; i++)
        {
            roadSpawned = false;
            //if center col
            if (i == cols/2)
            {
                //spawn chests
                for (int j = 0; j < rows; j++)
                {
                    float randF;
                    var chestTemp = Instantiate(chest, panel.transform);
                    if (j != 0 && !roadSpawned)
                    {
                        randF = Random.Range(0f, 1f);
                        if (randF <= roadChance)
                        {
                            chestTemp.transform.GetChild(0).gameObject.SetActive(true);
                            roadSpawned = true;
                        }
                    }
                    if (j != rows-1 && !roadSpawned)
                    {
                        randF = Random.Range(0f, 1f);
                        if (randF <= roadChance)
                        {
                            chestTemp.transform.GetChild(2).gameObject.SetActive(true);
                            roadSpawned = true;
                        }
                    }
                }
                continue;
            }

            for (int j = 0; j < rows; j++)
            {
                //priority of necessary
                if (generatedCount < totalMins)
                {
                    var rand = new System.Random();
                    var shuffled = minPList.OrderBy(_ => rand.Next()).ToList();
                    InstantiatePoint(shuffled[0], j, i);
                    if (pointsArr[shuffled[0]].count == pointsArr[shuffled[0]].min) minPList.Remove(shuffled[0]);
                    if (pointsArr[shuffled[0]].count == pointsArr[shuffled[0]].max) maxPList.Remove(shuffled[0]);
                }
                else
                {
                    var rand = new System.Random();
                    var shuffled = maxPList.OrderBy(_ => rand.Next()).ToList();
                    InstantiatePoint(shuffled[0], j, i);
                    if (pointsArr[shuffled[0]].count == pointsArr[shuffled[0]].max) maxPList.Remove(shuffled[0]);
                }
            }            
        }

        //generate last col
        Instantiate(empty, panel.transform);
        Instantiate(boss, panel.transform);
        Instantiate(empty, panel.transform);
    }

    private void InstantiatePoint(int pointIndex, int rowIndex, int colIndex)
    {
        generatedCount++;
        float randF;
        var pTemp = Instantiate(pointsArr[pointIndex].type, panel.transform);
        pointsArr[pointIndex].count++;

        if (colIndex != cols-2)
        {
            if (rowIndex != 0 && !roadSpawned)
            {
                randF = Random.Range(0f, 1f);
                if (randF <= roadChance)
                {
                    pTemp.transform.GetChild(0).gameObject.SetActive(true);
                    roadSpawned = true;
                }
            }
            if (rowIndex != rows-1 && !roadSpawned)
            {
                randF = Random.Range(0f, 1f);
                if (randF <= roadChance)
                {
                    pTemp.transform.GetChild(2).gameObject.SetActive(true);
                    roadSpawned = true;
                }
            }
        }
        else
        {
            if (rowIndex == 0)
            {
                pTemp.transform.GetChild(2).gameObject.SetActive(true);
                pTemp.transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (rowIndex == 2)
            {
                pTemp.transform.GetChild(0).gameObject.SetActive(true);
                pTemp.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        
    }

    private void enableNext(int pos)
    {
        var currentPoint = panel.transform.GetChild(pos);
        for (int i = 0; i < 3; i++)
        {
            var pathTemp = currentPoint.GetChild(i).gameObject;
            if (pathTemp.activeInHierarchy)
            {
                panel.transform.GetChild(pos+2+i).gameObject.GetComponent<Button>().interactable = true;
            } 
        }
        roomCleared();
    }

    private void roomCleared()
    {
        roomCompleted++;
        GameStat.updateEnemyMod(roomCompleted);
    }

    private void disableNext(int pos)
    {
        for (int i = 0; i < 3; i++)
        {
            panel.transform.GetChild(pos+2+i).gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void loadFight(GameObject button)
    {
        am.Stop("menu_music");
        am.Play("fight_music");
        disableNext(currentPos);
        currentPos = button.transform.GetSiblingIndex();
        GameStat.currentPos = currentPos;
        enableNext(currentPos);
        int rand = Random.Range(0, fightMaps.Length-1);
        GameStat.currentSceneName = fightMaps[rand];
        SceneManager.LoadScene(fightMaps[rand], LoadSceneMode.Additive);
        Main.SetActive(false);
    }

    public void LoadFirstFight(GameObject button)
    {
        am.Stop("menu_music");
        am.Play("fight_music");
        GameStat.currentPos = currentPos;
        enableNext(currentPos);
        int rand = Random.Range(0, fightMaps.Length-1);
        GameStat.currentSceneName = fightMaps[rand];
        SceneManager.LoadScene(fightMaps[rand], LoadSceneMode.Additive);
        Main.SetActive(false);
    }

    public void loadEliteFight(GameObject button, int modificator)
    {
        am.Stop("menu_music");
        am.Play("elite_music");
        disableNext(currentPos);
        currentPos = button.transform.GetSiblingIndex();
        GameStat.currentPos = currentPos;
        GameStat.fightModificatorID = modificator;
        enableNext(currentPos);
        int rand = Random.Range(0, eliteFightMaps.Length-1);
        GameStat.currentSceneName = eliteFightMaps[rand];
        SceneManager.LoadScene(eliteFightMaps[rand], LoadSceneMode.Additive);
        Main.SetActive(false);
    }

    public void loadBossFight()
    {
        am.Stop("menu_music");
        am.Play("boss_music");
        int rand = Random.Range(0, bossFightMaps.Length-1);
        GameStat.currentSceneName = bossFightMaps[rand];
        SceneManager.LoadScene(bossFightMaps[rand], LoadSceneMode.Additive);
        Main.SetActive(false);
    }

    public void shopClicked(GameObject button)
    {
        am.Pause("menu_music");
        am.Play("shop_music");
        disableNext(currentPos);
        currentPos = button.transform.GetSiblingIndex();
        GameStat.currentPos = currentPos;
        enableNext(currentPos);
        shopPanel.SetActive(true);
        shopPanel.GetComponent<Shop>().NewShop(shopItemsCount);
        panel.SetActive(false);
        
    }

    public void chestClicked(GameObject button)
    {
        disableNext(currentPos);
        currentPos = button.transform.GetSiblingIndex();
        GameStat.currentPos = currentPos;
        enableNext(currentPos);
        chestPanel.SetActive(true);
        chestPanel.GetComponent<Chest>().NewChest(chestItemsCount);
        panel.SetActive(false);
        
    }

    public void eventClicked(GameObject button)
    {
        disableNext(currentPos);
        currentPos = button.transform.GetSiblingIndex();
        GameStat.currentPos = currentPos;
        enableNext(currentPos);
        eventPanel.SetActive(true);
        eventPanel.GetComponent<EventManager>().SetRandomEvent();
        panel.SetActive(false);
        
    }

    public void restClicked(GameObject button)
    {
        disableNext(currentPos);
        currentPos = button.transform.GetSiblingIndex();
        GameStat.currentPos = currentPos;
        enableNext(currentPos);
        restPanel.SetActive(true);
        restPanel.GetComponent<Rest>().resetRest();
        panel.SetActive(false);
        
    }

    public void QuitClicked()
    {
        am.Stop("fight_music");
        am.Stop("elite_music");
        am.Stop("boss_music");
        am.Play("menu_music");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {       
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayClick()
    {
        am.Play("click");
    }

    public void PlaySelect()
    {
        am.Play("select");
    }
}


