using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BulletExplosion : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float damage;
    public float explRad;
    public LayerMask mask;
    public float defenceDestroy = 0f;
    public float hotEpicenterStr = 0f;
    public AudioSource explSound;

    private Rigidbody2D rb2d;

    Vector2 movementVector = Vector3.zero;
    Vector2 firstMovementVector;
    Vector3 lastTargetPos;

    void destroyBullet()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Vector3 vectorToTarget = target.transform.position - transform.position;
        firstMovementVector = (vectorToTarget).normalized*speed;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            lastTargetPos = target.transform.position;
        }

        Vector3 vectorToTarget = lastTargetPos - transform.position;
        movementVector = (vectorToTarget).normalized*speed;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        if (vectorToTarget.magnitude > movementVector.magnitude * Time.fixedDeltaTime)
        {
            rb2d.MovePosition(rb2d.position + movementVector * Time.fixedDeltaTime);
        }
        else
        {
            rb2d.MovePosition(lastTargetPos);
        }
        if (vectorToTarget.magnitude < 0.5f)
        {
            Explosion();
        }
        
    }

    void Explosion()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, explRad, mask.value);
        foreach (Collider2D collider in results)
        {
            if (collider.gameObject == target)
            {
                collider.GetComponent<Enemy>().TakeExplosionDamage(damage*(1 + hotEpicenterStr*results.Length), defenceDestroy);
            }
            else
            collider.GetComponent<Enemy>().TakeExplosionDamage(damage, defenceDestroy);
        }
        destroyBullet();
    }
}
