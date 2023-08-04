using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform goal;
    public int damage;
    public NavMeshAgent agent;
    public float healthMax = 20f;
    private float health = 20f;
    public float defence = 0f;
    public float hpRegen = 0f;

    private bool isFreezed = false;
    private bool isReadyUnfreeze = true;
    public float usualSpeed;
    private float freezeStrength = 0f;
    public float freezeDamage = 0f;
    public float freezeTime = 1f;
    public bool isFragiled = false;
    public float fragileStr = 1f;

    private int poisonStacks = 0;

    private GameObject fightManager;
    private ItemsManager itemsManager;


    public GameObject HealthBar;
    public Slider HealthBarSlider;
    public StatusControl statControl;
    public bool isReadyMove = false;

    // Start is called before the first frame update
    private void Start() {
        StartCoroutine(CoroutineEnemy());
    }
    void Awake()
    {
        //progress based stats up
        healthMax *= GameStat.enemyHealthMod;
        health = healthMax;
        defence *= GameStat.enemyArmorMod;

        //elite fight modification
        if (GameStat.fightModificatorID > 0)
        {
            eliteFightStatsUp(GameStat.fightModificatorID);
        }

        itemsManager = GameObject.FindGameObjectWithTag("ItemsManager").GetComponent<ItemsManager>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        Invoke("setRad", 1.5f);
        Invoke("setDestination", 0.1f);
        usualSpeed = agent.speed;
        agent.speed = 0;
        fightManager = GameObject.FindGameObjectsWithTag("FightManager")[0];
    }
    private void setRad()
    {
        agent.radius = 0.001f;
    }

    private void setDestination()
    {
        agent.destination = goal.position;
    }

    public void Spawned()
    {
        agent.speed = usualSpeed;
        isReadyMove = true;
    }

    public IEnumerator CoroutineEnemy()
    {
        while (true)
        {
            if (isFreezed && freezeDamage > 0)
            {
                TakeTrueDamage(freezeDamage);
                if (itemsManager.ItemsCount["Poison"] > 0) PoisonStacked();
            }
                
            if (poisonStacks>0)
            {
                TakeTrueDamage(itemsManager.PoisonDamage*poisonStacks);
                Debug.Log("taken damage: "+itemsManager.PoisonDamage*poisonStacks);
                poisonStacks--;
            }

            health += hpRegen;
            if (health > healthMax) health = healthMax;
            HealthBarSlider.value = health/healthMax;

            yield return new WaitForSeconds(1);
        }
    }

    void FixedUpdate()
    {
        FaceTarget();
        if (isReadyUnfreeze && isFreezed)
        {
            Unfreeze();
            isReadyUnfreeze = false;
            Invoke("ReadyUnfreeze", freezeTime);
        }
    }

    public void TakeDamage(float damage, string source)
    {
        if (isFragiled) damage *= fragileStr;
        HealthBar.SetActive(true);
        if (source == "tower" && itemsManager.ItemsCount["Poison"]>0)
            PoisonStacked();
        damage -= defence;
        if (damage < 0) damage = 0;
        health -= damage;
        HealthBarSlider.value = health/healthMax;
        if (health <= 0f) Death();
        Debug.Log("Damage");
    }

    public void TakeSniperDamage(float damage, float effectStr)
    {
        if (isFragiled) damage *= fragileStr;
        HealthBar.SetActive(true);
        if (itemsManager.ItemsCount["Poison"] > 0) PoisonStacked();
        damage -= (defence - defence * effectStr);
        if (damage < 0) damage = 0;
        health -= damage;
        HealthBarSlider.value = health / healthMax;
        if (health <= 0f) Death();
    }

    public void TakeExplosionDamage(float damage, float defenceDestroy)
    {
        if (isFragiled) damage *= fragileStr;
        HealthBar.SetActive(true);
        if (itemsManager.ItemsCount["Poison"] > 0) PoisonStacked();
        defence -= defenceDestroy;
        if (defence < 0) defence = 0;
        damage -= defence;
        if (damage < 0) damage = 0;
        health -= damage;
        HealthBarSlider.value = health / healthMax;
        if (health <= 0f) Death();
    }

    public void TakeTrueDamage(float damage)
    {
        if (isFragiled) damage *= fragileStr;
        HealthBar.SetActive(true);
        health -= damage;
        HealthBarSlider.value = health/healthMax;
        if (health <= 0f) Death();
    }

    public void PoisonStacked()
    {
        if (poisonStacks < itemsManager.maxPoisonStacks) poisonStacks++;
    }

    void Death()
    {
        fightManager.GetComponent<FightManager>().IncreaseKillsCount();
        Destroy(gameObject);
        return;
    }

    public void Freeze(float frStr)
    {
        if (isReadyMove)
        {
            if (frStr > freezeStrength)
            {
                freezeStrength = frStr;
                agent.speed = usualSpeed * freezeStrength;
            }
            if (!isFreezed)
            {
                freezeStrength = frStr;
                isFreezed = true;
                agent.speed = usualSpeed * freezeStrength;
            }
        }
        
    }

    private void Unfreeze()
    {
        if (isReadyMove)
        {
            agent.speed = usualSpeed;
        }
        isFreezed = false;
        isFragiled = false;
    }

    private void ReadyUnfreeze()
    {
        isReadyUnfreeze = true;
    }

    public void ChangeSpeed(float speedChange)
    {
        agent.speed += speedChange;
        usualSpeed = agent.speed;
    }

    private void eliteFightStatsUp(int modID)
    {
        switch (modID)
        {
            case 1:
                defence += GameStat.eliteDef;
                healthMax *= GameStat.eliteHpMod;
                health = healthMax;
                break;
            case 2:
                agent.speed *= GameStat.eliteSpeedMod;
                usualSpeed = agent.speed;
                break;
            case 3:
                hpRegen += GameStat.eliteHpRegen;
                break;
        }
    }

    public void statusAdd(int id, int add)
    {
        statControl.StatusChange(id, add);
    }

    void FaceTarget()
    {
        var turnTowardNavSteeringTarget = agent.steeringTarget;
     
        Vector3 direction = Quaternion.Euler(0, 0, 90) * (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(direction.x, direction.y, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
