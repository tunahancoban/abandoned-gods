using UnityEngine;

public class FallDeath : MonoBehaviour
{
    [Header("Death Zone Ayarlari")]
    [SerializeField] private float deathYLimit = -10f; // Y ekseni limiti


    private PlayerHealth health;
    private EnemyHealth enemyHealth;

    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            health = GetComponent<PlayerHealth>();
        }
        else
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }
       
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.y < deathYLimit)
        {
            if (health != null)
            {
                health.TakeDamage(9999);
            }
            else if (enemyHealth != null)
            {
                 enemyHealth.TakeDamage(9999);
            }
        }
    }
}

