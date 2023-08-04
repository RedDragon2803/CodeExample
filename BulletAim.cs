using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAim : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float damage;
    public int ricochetCount = 0;
    public LayerMask mask;
    public float ricochetEffectStr;

    private GameObject damagedEnemy = null;
    private Rigidbody2D rb2d;

    Vector2 movementVector = Vector3.zero;
    Vector2 firstMovementVector;

    private bool isTargeted = true;
    void destroyBullet()
    {
        //ricochet
        if (ricochetCount > 0)
        {
            damagedEnemy = target;
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, 1f, mask.value);
            List<Collider2D> colliders = new List<Collider2D>(results);

            //delete damaged enemy from array
            int j = 0;
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject == damagedEnemy)
                {
                    colliders.RemoveAt(j);
                    break;
                }
                j++;
            }
            results = colliders.ToArray();

            if (results.Length > 0)
            {
                //finding closest enemy
                float closestDistance = (results[0].transform.position - transform.position).sqrMagnitude;
                target = results[0].gameObject;
                if (results.Length > 1)
                {
                    for (int i = 1; i < results.Length; i++)
                    {
                        float dist = (results[i].transform.position - transform.position).sqrMagnitude;
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            target = results[i].gameObject;
                        }
                    }
                }
                //new bullet
                GameObject bullet = Instantiate(gameObject, transform.position, Quaternion.identity);
                BulletAim bulletAim = bullet.GetComponent<BulletAim>();
                bulletAim.damage = damage * ricochetEffectStr;
                bulletAim.ricochetCount = ricochetCount - 1;
                bulletAim.ricochetEffectStr = ricochetEffectStr;
                bulletAim.target = target;
            }
        }
        Destroy(gameObject);
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (target != null)
        {
            Vector3 vectorToTarget = target.transform.position - transform.position;
            firstMovementVector = (vectorToTarget).normalized * speed;
        }
        else
        {
            firstMovementVector = new Vector3(1, 0, 0);
            Invoke("destroyBullet", 2f);
            isTargeted = false;
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 vectorToTarget = target.transform.position - transform.position;
            movementVector = (vectorToTarget).normalized * speed;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;
            rb2d.MovePosition(rb2d.position + movementVector * Time.fixedDeltaTime);
        }
        else
        {
            rb2d.MovePosition(rb2d.position + firstMovementVector * Time.fixedDeltaTime);
            if (isTargeted)
            {
                Invoke("destroyBullet", 2f);
                isTargeted = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyBoss")
        {
            if (other.gameObject == target)
            {
                other.gameObject.GetComponent<Enemy>().TakeDamage(damage, "tower");
                destroyBullet();
            }

        }
    }
}
