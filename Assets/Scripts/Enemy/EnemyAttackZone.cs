using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);

        {
            collision.GetComponent<PlayerHealth>()?.TakeDamage(GetComponentInParent<EnemyAttack>().damage);
        }
    }
    
}
