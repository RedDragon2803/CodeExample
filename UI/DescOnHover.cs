using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descPanel;

    public void OnPointerEnter(PointerEventData eventData) {
        descPanel.SetActive(true);    
    }

    public void OnPointerExit(PointerEventData eventData) {
        descPanel.SetActive(false);    
    }
}
