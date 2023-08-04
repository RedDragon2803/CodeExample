using UnityEngine;
using TMPro;

public static class GameStat
{
    public static int health;
    public static int shield;
    public static int money;
    public static int blueprints;
    public static string currentLevel;
    public static int currentPos = 1;
    public static GameObject Main;

    public static TMP_Text ShieldCount;
    public static TMP_Text HealthCount;
    public static TMP_Text MoneyCount;
    public static TMP_Text BlueprintsCount;

    public static LevelManager levelManager;

    public static float enemyHealthModChange = 0.15f;
    public static float enemyArmorModChange = 0.15f;
    public static float enemyHealthMod = 1;
    public static float enemyArmorMod = 1;

    public static int fightModificatorID = 0;
    public static float eliteDef = 2f;
    public static float eliteHpMod = 1.1f;
    public static float eliteHpRegen = 3f;
    public static float eliteSpeedMod = 1.1f;
    public static string currentSceneName;

    public static int[] towersLevels = {1, 1, 1, 1};

    public static void updateStats()
    {
        HealthCount.SetText(health.ToString());
        ShieldCount.SetText(shield.ToString());
        MoneyCount.SetText(money.ToString());
        BlueprintsCount.SetText(blueprints.ToString());
        if (health <= 0)
        {
            levelManager.GameOver();
        }
    }

    public static void updateEnemyMod(int roomCleared)
    {
        enemyArmorMod = 1 + enemyArmorModChange*roomCleared;
        enemyHealthMod = 1 + enemyHealthModChange*roomCleared;
    }

    public static void ResetTowerLevel()
    {
        for (int i = 0; i < towersLevels.Length; i++)
        {
            towersLevels[i] = 1;
        }
    }
}
