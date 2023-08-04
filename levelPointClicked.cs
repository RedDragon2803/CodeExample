using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class levelPointClicked : MonoBehaviour, IPointerEnterHandler
{
    public GameObject levelManager;
    public string levelPointType;

    public int modificator;
    public int modCount = 1;

    private LevelManager lm;
    private Button btn;
    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnLevelPointClicked);
        levelManager = GameObject.FindGameObjectsWithTag("LevelManager")[0];
        lm = levelManager.GetComponent<LevelManager>();
    }

    private void OnLevelPointClicked()
    {
        lm.PlayClick();
        switch (levelPointType)
        {
            case "fight":
                lm.loadFight(EventSystem.current.currentSelectedGameObject);
                gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Vector4(200, 200, 200, 255);
                break;
            case "elite":
                lm.loadEliteFight(EventSystem.current.currentSelectedGameObject, Random.Range(1, modCount));
                gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Vector4(200, 200, 200, 255);
                break;
            case "boss":
                lm.loadBossFight();
                break;
            case "shop":
                lm.shopClicked(EventSystem.current.currentSelectedGameObject);
                gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Vector4(200, 200, 200, 255);
                break;
            case "rest":
                lm.restClicked(EventSystem.current.currentSelectedGameObject);
                gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Vector4(200, 200, 200, 255);
                break;
            case "event":
                lm.eventClicked(EventSystem.current.currentSelectedGameObject);
                gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Vector4(200, 200, 200, 255);
                break;
            case "chest":
                lm.chestClicked(EventSystem.current.currentSelectedGameObject);
                gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Vector4(200, 200, 200, 255);
                break;
            case "first":
                lm.LoadFirstFight(EventSystem.current.currentSelectedGameObject);
                gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Vector4(200, 200, 200, 255);
                break;
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btn.interactable) lm.PlaySelect();
    }
}
