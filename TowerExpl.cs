using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerExpl : Tower
{
    public override void Shoot()
    {
        shotSound.Play();
        towerHead.GetComponent<Animator>().SetTrigger("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        BulletExplosion bulletExplosion = bullet.GetComponent<BulletExplosion>();
        bulletExplosion.target = target;
        bulletExplosion.mask = mask;

        if (GetTechs()[0].isEnabled) bulletExplosion.defenceDestroy = GetTechs()[0].effectStrength;
        if (GetTechs()[1].isEnabled) bulletExplosion.explRad *= GetTechs()[1].effectStrength;
        if (GetTechs()[2].isEnabled) bulletExplosion.hotEpicenterStr *= GetTechs()[2].effectStrength;

        if (Random.Range(0f, 1f) < itemsManager.towersCritChance)
        {
            bulletExplosion.damage = damage * itemsManager.towersCritDamageMod;
            Debug.Log("Crit!");
        }
        else bulletExplosion.damage = damage;
    }
}
