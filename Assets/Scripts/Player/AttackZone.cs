using UnityEngine;

public class AttackZone : MonoBehaviour
{
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(20);
            GetComponentInParent<PlayerAttack>().playAttackHitSFX();
        }
        else if (collision.CompareTag("Arrow"))
        {
            collision.GetComponent<Arrow>().Reverse();
        }
    }
}
