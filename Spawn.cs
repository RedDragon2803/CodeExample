using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject[] Enemies;
    public float[] spawnDelay;

    public float firstDelay;
    public GameObject Base;
    public bool isEmpty = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawning());
    }

    void SpawnEnemy(GameObject enemy)
    {
        GameObject spawned;
        spawned = Instantiate(enemy, transform.position, Quaternion.identity);
        spawned.GetComponent<Enemy>().goal = Base.transform;
    }

    IEnumerator Spawning()
    {
        yield return new WaitForSeconds(firstDelay);
        for (int i = 0; i < Enemies.Length; i++)
        {
            SpawnEnemy(Enemies[i]);
            yield return new WaitForSeconds(spawnDelay[i]);
        }

        isEmpty = true;
        StopCoroutine(Spawning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
