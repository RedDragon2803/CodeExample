using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPoint : MonoBehaviour
{
    public LevelPoint nextLP1;
    public LevelPoint nextLP2;
    public LevelPoint nextLP3;

    public string pointType;

    public LevelPoint(LevelPoint next1, LevelPoint next2, LevelPoint next3, string type)
    {
        nextLP1 = next1;
        nextLP2 = next2;
        nextLP3 = next3;
        pointType = type;
    }
}
