using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    private int variant;
    public float buffCD;
    public float speedBuff;
    public float defenceBuff;
    public float hpRegenBuff;
    public int buffIconID;
    private GameObject[] enemies;
    public Animator buffWaveAnim;
    public Animator buffAnim;
    private string buffType;
    private Enemy enemyScript;

    public void AbilityStart()
    {
        enemyScript.agent.speed = 0;
        enemyScript.isReadyMove = false;
    }
    public void AbilityEnd()
    {
        enemyScript.agent.speed = gameObject.GetComponent<Enemy>().usualSpeed;
        enemyScript.isReadyMove = true;
    }
    private void Start() {
        enemyScript = gameObject.GetComponent<Enemy>();
        variant = Random.Range(0, 3);
        switch (variant)
        {
            case 0:
                buffType = "SpeedBuff";
                defenceBuff = 0;
                hpRegenBuff = 0;
                buffIconID = 2;
                break;
            case 1:
                buffType = "ArmorBuff";
                speedBuff = 0;
                hpRegenBuff = 0;
                buffIconID = 0;
                break;
            case 2:
                buffType = "RegenBuff";
                speedBuff = 0;
                defenceBuff = 0;
                buffIconID = 4;
                break;
        }
        StartCoroutine(CoroutineBoss());
    }

    private IEnumerator CoroutineBoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            buffWaveAnim.SetTrigger(buffType);
            buffAnim.SetTrigger("buff");

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Enemy enemyScriptAnother = enemy.GetComponent<Enemy>();
                enemyScriptAnother.statusAdd(buffIconID, 1);
                enemyScriptAnother.ChangeSpeed(speedBuff);
                enemyScriptAnother.defence += defenceBuff;
                enemyScriptAnother.hpRegen += hpRegenBuff;
            }
        }
    }
}
