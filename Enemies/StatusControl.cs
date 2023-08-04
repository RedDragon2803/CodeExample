using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusControl : MonoBehaviour
{
    public Enemy enemyScript;

    public GameObject[] StatusArr;
    private int[] StatusStacks;
    // Start is called before the first frame update
    void Start()
    {
        StatusStacks = new int[StatusArr.Length];
        for (int i = 0; i < StatusArr.Length; i++)
        {
            StatusStacks[i] = 0;
            UpdateStatus(i);
        }
    }

    public void UpdateStatus(int id)
    {
        if (StatusStacks[id] > 1)
            {
                StatusArr[id].SetActive(true);
                GameObject text = StatusArr[id].transform.GetChild(0).gameObject;
                text.SetActive(true);
                text.GetComponent<TMP_Text>().SetText(StatusStacks[id].ToString());
            }
            else if (StatusStacks[id] == 1)
            {
                StatusArr[id].SetActive(true);
                GameObject text = StatusArr[id].transform.GetChild(0).gameObject;
                text.SetActive(false);
            }
            else 
            {
                StatusArr[id].SetActive(false);
            }
    }

    public void StatusChange(int id, int change)
    {
        StatusStacks[id] += change;
        if (StatusStacks[id] < 0) 
        {
            StatusStacks[id] = 0;
        }
        UpdateStatus(id);
    }

    public int GetStatusStacks(int id)
    {
        return StatusStacks[id];
    }
}
