using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnHover : MonoBehaviour, IPointerEnterHandler
{
    public AudioManager am;
    public string sound_name;

    public void Awake()
    {
        am = FindObjectOfType<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        am.Play(sound_name);
    }

    public void PlayClick()
    {
        am.Play("click");
    }

    public void UnPauseMenu()
    {
        am.UnPause("menu_music");
    }

    public void StopShop()
    {
        am.Stop("shop_music");
    }
}
