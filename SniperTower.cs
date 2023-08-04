using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : Tower
{
    public override void LateStart()
    {
        if (attackSpeed < 0.2f) attackSpeed = 0.2f;
    }

    public override void FindTarget()
    {
        SetMinDist(float.MaxValue);
        Collider2D[] results = Physics2D.OverlapCircleAll(GetPos(), attackRange, mask.value);
        //armor targeting
        if (GetTechs()[1].isEnabled && results.Length > 0)
        {
            float maxDef = results[0].gameObject.GetComponent<Enemy>().defence;
            target = results[0].gameObject;
            if (results.Length > 1)
            {
                for (int i = 1; i < results.Length; i++)
                {
                    float tempDef = results[i].gameObject.GetComponent<Enemy>().defence;
                    if (tempDef > maxDef)
                    {
                        maxDef = tempDef;
                        target = results[i].gameObject;
                    }
                }
            }
            isAttacking = true;
        }
        else //first targeting
        {
            foreach (Collider2D collider in results)
            {
                float dist = GetPathRemainingDistance(collider.GetComponent<UnityEngine.AI.NavMeshAgent>());
                if (dist < GetMinDist() && dist > 0)
                {
                    SetMinDist(dist);
                    target = collider.gameObject;
                    isAttacking = true;
                }
            }
        }

    }

    public override void Shoot()
    {
        shotSound.Play();
        towerHead.GetComponent<Animator>().SetTrigger("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        SniperBullet sniperBullet = bullet.GetComponent<SniperBullet>();
        sniperBullet.target = target;

        if (GetTechs()[0].isEnabled) sniperBullet.piercingStr = GetTechs()[0].effectStrength;
        if (GetTechs()[2].isEnabled) sniperBullet.armorIgnorStr = GetTechs()[2].effectStrength;

        if (Random.Range(0f, 1f) < itemsManager.towersCritChance)
        {
            sniperBullet.damage = damage * itemsManager.towersCritDamageMod;
            Debug.Log("Crit!");
        }
        else sniperBullet.damage = damage;
    }
}
