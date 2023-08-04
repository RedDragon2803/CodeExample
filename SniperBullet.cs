using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float damage;
    public float piercingStr = 0f;
    public float armorIgnorStr = 0f;

    private Rigidbody2D rb2d;

    Vector2 movementVector = Vector3.zero;
    Vector2 firstMovementVector;

    void destroyBullet()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        Invoke("destroyBullet", 2f);
        rb2d = GetComponent<Rigidbody2D>();
        Vector3 vectorToTarget = target.transform.position - transform.position;
        firstMovementVector = (vectorToTarget).normalized * speed;
    }

    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + firstMovementVector * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyBoss")
        {
            other.gameObject.GetComponent<Enemy>().TakeSniperDamage(damage, armorIgnorStr);

            if (piercingStr > 0)
            {
                damage *= piercingStr;
            }
            else
            {
                destroyBullet();
            }
        }
    }
}
