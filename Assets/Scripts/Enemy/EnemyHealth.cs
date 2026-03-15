using System.Collections;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    [SerializeField] float health = 100;
    Animator animator;
    public CinemachineImpulseSource impulse;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    public void TakeDamage(float damage)
    {
        enemyMovement.LookAtPlayer();
        animator.SetTrigger("takeDamage");
        impulse.GenerateImpulse(0.2f);
        health -= damage;
        if (health <= 0)
        {
            animator.SetBool("isAlive", false);
            enemyMovement.state = EnemyState.Dead;
        }
    }

    void Die()
    {
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(name + " is dead!");
        Destroy(gameObject);
    }
    
  
}
