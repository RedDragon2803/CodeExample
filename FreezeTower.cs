using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTower : Tower
{
    public float freezeStrength;
    public Transform freezeCircle;

    void Start()
    {
        freezeCircle.localScale = new Vector3(attackRange, attackRange, 1);
    }
    
    public override void LateStart()
    {
        if (GetTechs()[0].isEnabled)
        {
            freezeStrength *= GetTechs()[0].effectStrength;
        }
    }

    void Update()
    {

    }
    
    void FixedUpdate()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(GetPosV2(), attackRange, GetMask().value);
        foreach (Collider2D collider in results)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (GetTechs()[1].isEnabled)
            {
                enemy.freezeDamage = GetTechs()[1].effectStrength;
            }

            if (GetTechs()[2].isEnabled)
            {
                enemy.isFragiled = true;
                enemy.fragileStr = GetTechs()[2].effectStrength;
            }

            enemy.Freeze(freezeStrength);
        }
    }
}
