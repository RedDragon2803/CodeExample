using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{

    public int Health;
    public int Shield;

    public TMP_Text ShieldCount;
    public TMP_Text HealthCount;

    public FightManager fightManager;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
        Health = GameStat.health;
        Shield = GameStat.shield;
        ShieldCount = GameStat.ShieldCount;
        HealthCount = GameStat.HealthCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (Shield > 0)
            Shield -= damage;
        else Health -= damage;
        if (Shield < 0)
        {
            Health += Shield;
            Shield = 0;
        }
        HealthCount.SetText(Health.ToString());
        ShieldCount.SetText(Shield.ToString());
        GameStat.health = Health;

        if (Health <= 0) Death();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy" || other.tag == "EnemyBoss")
        {
            TakeDamage(other.gameObject.GetComponent<Enemy>().damage);
            Destroy(other.gameObject);
            fightManager.IncreaseKillsCount();
        }
    }

    void Death()
    {
        levelManager.GameOver();
    }
}
