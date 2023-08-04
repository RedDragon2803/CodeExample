using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FightManager : MonoBehaviour
{
    public AudioManager am;

    public string currentLevel;
    public string currentSceneName;
    public GameObject rewardPanel;
    public int moneyReward = 150;
    public int blueprintsReward = 1;
    public int techReward = 1;
    public TMP_Text killText;
    private int killCount = 0;
    private int enemyCount = 0;
    private float prevoisFightSpeed = 1;
    private bool isPaused = false;
    private bool isFightEnded = false;

    public Spawn[] spawns;
    // Start is called before the first frame update
    void Awake()
    {
        am = FindObjectOfType<AudioManager>();
        currentLevel = GameStat.currentLevel;
        foreach (Spawn spawn in spawns)
        {
            enemyCount += spawn.Enemies.Length;
        }
        killText.SetText("0/"+enemyCount.ToString());
    }

    void Start() 
    {
        currentSceneName = GameStat.currentSceneName;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentSceneName));
    }

    void Update()
    {
        if (!isFightEnded)
        {
            if (Input.GetKeyDown("space"))
            {
                if (!isPaused)
                {
                    Time.timeScale = 0;
                    isPaused = true;
                }
                else
                {
                    Time.timeScale = prevoisFightSpeed;
                    isPaused = false;
                }
            }

            if (Input.GetKeyDown("z"))
            {
                if (prevoisFightSpeed == 1)
                {
                    Time.timeScale = 2;
                    prevoisFightSpeed = 2;
                }
                else {
                    Time.timeScale = 1;
                    prevoisFightSpeed = 1;
                }
            }
        }
    }

    public void IncreaseKillsCount()
    {
        killCount++;
        killText.SetText(killCount.ToString()+"/"+enemyCount.ToString());
        if (killCount >= enemyCount) showReward();
    }

    public void showReward()
    {
        Time.timeScale = 0;
        rewardPanel.SetActive(true);
        rewardPanel.GetComponent<Chest>().NewChest(2, moneyReward, blueprintsReward, techReward);
        isFightEnded = true;
    }

    public void EndFight()
    {
        am.Stop("fight_music");
        am.Stop("elite_music");
        am.Stop("boss_music");
        am.Play("menu_music");
        Time.timeScale = 1;
        GameStat.Main.SetActive(true);
        GameStat.updateStats();
        GameStat.fightModificatorID = 0;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        am.Stop("fight_music");
        am.Stop("elite_music");
        am.Stop("boss_music");
        am.Play("menu_music");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
