using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class SelectTile : MonoBehaviour
{

    [SerializeField] private TechReward techReward;
    [SerializeField] private Tile selectedGreen;
    [SerializeField] private Tile selectedRed;
    [SerializeField] private Tile unselected;
    [SerializeField] private Tilemap walls = null;
    [SerializeField] private ResourceManager ResManager;
    [SerializeField] private GameObject TowersPanel;
    [SerializeField] Grid grid;
    [SerializeField] GameObject[] towers;
    [SerializeField] private GameObject towerRad;
    private ItemsManager itemsManager;
    private float[] towersRad;
    private float[] towersDamage;
    private float[] towersAttackSpeed;
    private float[] towersRotSpeed;
    private TechArray[] techs;
    private AudioManager am;
    

    private GameObject newTowerRad;
    private Tower TowerOpened;

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    private Transform[] TowersOnPanel;
    public TMP_Text DPText;

    Vector3Int position;
    Vector2 mousePosition;
    int selectedTower = 0;

    private Vector3Int previousMousePos = new Vector3Int();

    private Dictionary<Vector3Int, GameObject> objOnTile;

    private void Awake() {
        am = FindObjectOfType<AudioManager>();
        techs = techReward.GetTechs();
        objOnTile = new Dictionary<Vector3Int, GameObject>();
        TowersOnPanel = new Transform[towers.Length];
        itemsManager = GameObject.FindGameObjectWithTag("ItemsManager").GetComponent<ItemsManager>();
        int i = 0;
        towersRad = new float[towers.Length];
        towersDamage = new float[towers.Length];
        towersAttackSpeed = new float[towers.Length];
        towersRotSpeed = new float[towers.Length];
        foreach (GameObject tower in towers)
        {
            TowersOnPanel[i] = TowersPanel.transform.GetChild(i);
            Tower tempScriptTower = tower.GetComponent<Tower>();
            towersRad[i] = tempScriptTower.attackRange * itemsManager.attackRadMod * Mathf.Pow(tempScriptTower.attackRangeLevelBaseMod, GameStat.towersLevels[i]-1);
            towersAttackSpeed[i] = tempScriptTower.attackSpeed * itemsManager.attackSpeedMod * Mathf.Pow(tempScriptTower.attackSpeedLevelBaseMod, GameStat.towersLevels[i]-1);
            if (towersAttackSpeed[i] < 0.2f) towersAttackSpeed[i] = 0.2f;
            towersDamage[i] = tempScriptTower.damage * Mathf.Pow(tempScriptTower.damageLevelBaseMod, GameStat.towersLevels[i]-1);
            towersRotSpeed[i] = tempScriptTower.rotSpeed * Mathf.Pow(tempScriptTower.rotSpeedLevelBaseMod, GameStat.towersLevels[i]-1);

            if (i == 3 && techs[3].techArray[0].isEnabled)
            {
                towersRad[3] *= 0.5f;
            }
            if (i == 1 && techs[1].techArray[1].isEnabled)
            {
                towersRad[1] *= techs[1].techArray[1].effectStrength;
            }
            i++;
        }
    }

    void TowerSwitch() {
        if (Input.GetKeyDown(keyCodes[0]))
        {
            if (selectedTower != 0) am.Play("select");
            if (newTowerRad != null) Destroy(newTowerRad);
            TowersOnPanel[selectedTower].localScale = new Vector3(1, 1, 1);
            selectedTower = 0;
            TowersOnPanel[selectedTower].localScale = new Vector3(1.2f, 1.2f, 1);
        }
        if (Input.GetKeyDown(keyCodes[1]))
        {
            if (selectedTower != 1) am.Play("select");
            if (newTowerRad != null) Destroy(newTowerRad);
            TowersOnPanel[selectedTower].localScale = new Vector3(1, 1, 1);
            selectedTower = 1;
            TowersOnPanel[selectedTower].localScale = new Vector3(1.2f, 1.2f, 1);
        }
        if (Input.GetKeyDown(keyCodes[2]))
        {
            if (selectedTower != 2) am.Play("select");
            if (newTowerRad != null) Destroy(newTowerRad);
            TowersOnPanel[selectedTower].localScale = new Vector3(1, 1, 1);
            selectedTower = 2;
            TowersOnPanel[selectedTower].localScale = new Vector3(1.2f, 1.2f, 1);
        }
        if (Input.GetKeyDown(keyCodes[3]))
        {
            if (selectedTower != 3) am.Play("select");
            if (newTowerRad != null) Destroy(newTowerRad);
            TowersOnPanel[selectedTower].localScale = new Vector3(1, 1, 1);
            selectedTower = 3;
            TowersOnPanel[selectedTower].localScale = new Vector3(1.2f, 1.2f, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TowerSwitch();
        int currentMoney = ResManager.money;
        int tcost = towers[selectedTower].GetComponent<Tower>().cost;
        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos) || newTowerRad == null) 
        {
            if (walls.HasTile(previousMousePos))
            {
                if (newTowerRad != null) Destroy(newTowerRad);
            }

            if (walls.HasTile(mousePos) && !objOnTile.ContainsKey(mousePos))
            {
                newTowerRad = Instantiate(towerRad, mousePos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                newTowerRad.GetComponent<CursorTower>().setTowerSprite(selectedTower);
                newTowerRad.GetComponent<CursorTower>().setRadius(towersRad[selectedTower]);
                if (currentMoney >= tcost)
                    newTowerRad.GetComponent<CursorTower>().setRadColor(true);
                else
                    newTowerRad.GetComponent<CursorTower>().setRadColor(false);
            }
                
            previousMousePos = mousePos;
        }
        else if (newTowerRad != null && currentMoney >= tcost)
        {
            newTowerRad.GetComponent<CursorTower>().setRadColor(true);
        }

        if (Input.GetMouseButtonDown(0) && walls.HasTile(mousePos))
        {
            if (!objOnTile.ContainsKey(mousePos)) //if empty tile
            {
                if (currentMoney >= tcost)
                {
                    am.Play("tower_place");
                    GameObject newTower = Instantiate(towers[selectedTower], mousePos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    Tower temp = newTower.GetComponent<Tower>();
                    temp.towerPos = mousePos;
                    temp.TowerManager = this;
                    objOnTile.Add(mousePos, newTower);
                    temp.damage = towersDamage[selectedTower];
                    temp.attackRange = towersRad[selectedTower];
                    temp.attackSpeed = towersAttackSpeed[selectedTower];
                    temp.rotSpeed = towersRotSpeed[selectedTower];
                    temp.AtkRangeCircle.transform.localScale = new Vector3(towersRad[selectedTower], towersRad[selectedTower], 1);
                    Technology[] techs_tower = techs[selectedTower].techArray;
                    temp.SetTechs(techs_tower);
                    ResManager.money -= tcost;
                    ResManager.UpdateResources();
                    if (newTowerRad != null) Destroy(newTowerRad);
                }
                
            }
            else //на клетке башня
            {
                if (TowerOpened != null) 
                    CloseTowerUI();
                //отображение интерфейса башни
                Tower temp = objOnTile[mousePos].GetComponent<Tower>();
                temp.Canvas.SetActive(true);
                temp.AtkRangeCircle.SetActive(true);
                TowerOpened = temp;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TowerOpened != null)
            {
                CloseTowerUI();
            }
        }
    }

    private void CloseTowerUI()
    {
        TowerOpened.Canvas.SetActive(false);
        TowerOpened.AtkRangeCircle.SetActive(false);
        TowerOpened = null;
    }

    Vector3Int GetMousePosition () 
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    public void SellTower(Vector3Int pos, int cost)
    {
        if (TowerOpened.GetComponent<Tower>().towerID == 0 && techs[0].techArray[2].isEnabled)
        {
            ResManager.money += cost;
        }
        else ResManager.money += cost/2;
        TowerOpened = null;
        ResManager.UpdateResources();
        objOnTile.Remove(pos);
    }
}
