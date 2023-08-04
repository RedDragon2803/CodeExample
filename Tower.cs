using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Tower : MonoBehaviour
{

    public int towerID;
    public float attackRange;
    public float attackSpeed;
    public float damage;
    public float rotSpeed = 1f;
    public float attackRangeLevelBaseMod = 1.1f;
    public float attackSpeedLevelBaseMod = 0.9f;
    public float damageLevelBaseMod = 1.5f;
    public float rotSpeedLevelBaseMod = 1.1f;
    public int cost;
    public GameObject bulletPrefab;
    public GameObject towerHead;
    public Transform shootPoint;
    public ItemsManager itemsManager;

    public GameObject AtkRangeCircle;
    public GameObject Canvas;

    public AudioSource shotSound;
    

    [HideInInspector]public GameObject target;
    [HideInInspector]public bool isAttacking = false;
    [HideInInspector]private Vector2 pos;
    [HideInInspector]public LayerMask mask;
    [HideInInspector]private float minDistance;
    [HideInInspector]public bool ableFire = true;

    [HideInInspector]public Vector3Int towerPos;
    [HideInInspector]public SelectTile TowerManager;

    private Technology[] techs;

    public LayerMask GetMask()
    {
        return mask;
    }

    public Vector2 GetPosV2()
    {
        return pos;
    }

    void Awake()
    {       
        pos = new Vector2(transform.position.x, transform.position.y);
        mask = LayerMask.GetMask("Enemy");
        itemsManager = GameObject.FindGameObjectWithTag("ItemsManager").GetComponent<ItemsManager>();
        Invoke("LateStart", 0.01f);
    }

    public virtual void LateStart()
    {
        if (towerID == 0 && techs[0].isEnabled)
        {
            attackSpeed *= techs[0].effectStrength;
            rotSpeed *= 2;
        }
        if (attackSpeed < 0.2f) attackSpeed = 0.2f;
    }

    public float GetPathRemainingDistance(UnityEngine.AI.NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0)
            return -1f;

        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        return distance;
    }

    void Ready2Shoot()
    {
        ableFire = true;
        isAttacking = false;
    }

    public virtual void Shoot()
    {
        
        towerHead.GetComponent<Animator>().SetTrigger("Shoot");
        shotSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        BulletAim bulletAim = bullet.GetComponent<BulletAim>();
        bulletAim.target = target;
        bulletAim.mask = mask;
        bulletAim.ricochetEffectStr = techs[1].effectStrength;
        if (techs[0].isEnabled)
        {
            bulletAim.ricochetCount = 2;
        }
        
        if (Random.Range(0f, 1f) < itemsManager.towersCritChance)
        {
            
            bulletAim.damage = damage * itemsManager.towersCritDamageMod;
            Debug.Log("Crit!");
        }
        else bulletAim.damage = damage;
    }

    public virtual void FindTarget()
    {
        minDistance = float.MaxValue;
        Collider2D[] results = Physics2D.OverlapCircleAll(pos, attackRange, mask.value);
        //random targeting
        if (techs[0].isEnabled && results.Length>0)
        {
            int randomI = Random.Range(0, results.Length);
            target = results[randomI].gameObject;
            isAttacking = true;
        }
        else //first targeting
        {
            foreach (Collider2D collider in results)
            {
                float dist = GetPathRemainingDistance(collider.GetComponent<UnityEngine.AI.NavMeshAgent>());
                if (dist < minDistance && dist > 0)
                {
                    minDistance = dist;
                    target = collider.gameObject;
                    isAttacking = true;
                }
            }
        }
        
    }

    public void AttackTarget()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angleToRot = Vector3.Angle(vectorToTarget, towerHead.transform.up);
        if (ableFire && angleToRot < 20f)
        {
            Shoot();
            ableFire = false;
            Invoke("Ready2Shoot", attackSpeed);
        }
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        towerHead.transform.rotation = Quaternion.Slerp(towerHead.transform.rotation, q, Time.deltaTime * rotSpeed);
    }
    
    // Update is called once per frame
    void Update()
    {      
        if (!isAttacking)
        {
            FindTarget();
        }
        else
        {
            if (target != null)
            {
                AttackTarget();
            }
            else isAttacking = false;

        }
    }

    public void SellTower()
    {
        TowerManager.SellTower(towerPos, cost);
        Destroy(gameObject);
    }

    public void SetTechs(Technology[] _techs)
    {
        techs = _techs;
    }

    public Technology[] GetTechs()
    {
        return techs;
    }

    public float GetMinDist()
    {
        return minDistance;
    }

    public void SetMinDist(float distance)
    {
        minDistance = distance;
    }

    public Vector3 GetPos()
    {
        return pos;
    }
}
