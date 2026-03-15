using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float health;
    [SerializeField] float armor;
    [SerializeField] float healthPotVal;
    [SerializeField] Canvas deathMenu;
    private Animator animator;
    private PlayerMovement playerMovement;
    public GameObject bloodEffect;
    public bool isHealing = false;
    public bool isAlive = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        bloodEffect.GetComponent<SpriteRenderer>().enabled = false;
        deathMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !playerMovement.isPraying && !playerMovement.isOnAir && !playerMovement.isRunning)
        {
            isHealing = true;
            TakeHeal();
        }
        Vector2 zoneLocalPos = bloodEffect.transform.localPosition;
        if (playerMovement.side)
        {
            zoneLocalPos.x = 1;
            bloodEffect.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            zoneLocalPos.x = -1;
            bloodEffect.GetComponent<SpriteRenderer>().flipY = false;

        }
        bloodEffect.transform.localPosition = zoneLocalPos;
    }

    public void TakeDamage(float damage)
    {
        bloodEffect.GetComponent<SpriteRenderer>().enabled = true;
        if (armor > 0)
        {
            armor -= damage;
            animator.SetTrigger("isHurt");
        }
        else
        {
            health -= damage;
            animator.SetTrigger("isHurt");
        }
        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetBool("isDead", false);
        }
    }
    void Die()
    {
        isAlive = false;
        animator.SetBool("isDead", true);
        GetComponent<Rigidbody2D>().linearVelocityX = 0f;
        deathMenu.enabled = true;
        Time.timeScale = 0f;
    }

    public void TakeHeal()
    {
        if (health < maxHealth)
        {
            health += healthPotVal;
            animator.SetTrigger("heal");
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public void healingControl()
    {
        isHealing = false;
    }
    public float updateHealth()
    {
        return health / 100;
    }
    public float GetCurrentHealth()
    {
        return health;
    }

    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxHealth);
    }


}
