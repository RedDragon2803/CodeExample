using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        GameStat.ResetTowerLevel();
    }
    public void PlayClicked()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void QuitClicked()
    {
        Application.Quit();
    }


}
