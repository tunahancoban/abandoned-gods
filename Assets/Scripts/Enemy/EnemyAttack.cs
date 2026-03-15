using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage;
    [SerializeField] Transform playerCheck;
    [SerializeField] float range;
    [SerializeField] float cooldown;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float arrow_speed;
    public Collider2D attackZone;
    private Animator animator;
    private float lastAttackTime;
    private EnemyMovement enemyMovement;
    private Rigidbody2D rb;
    public bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        lastAttackTime = Time.time;
        attackZone.enabled = false;
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, enemyMovement.player.position);
        //Vector2 direction = enemyMovement.getDirection() ? Vector2.right : Vector2.left;
        //RaycastHit2D playerInfo = Physics2D.Raycast(playerCheck.position, direction, range, playerLayer);


        if (distanceToPlayer <= range && !isAttacking && enemyMovement.playerInSight && enemyMovement.player.GetComponent<PlayerHealth>().isAlive)
        {
            if ((Time.time - lastAttackTime) > cooldown)
            {
                animator.SetBool("isAttacking", true);
                isAttacking = true;
                lastAttackTime = Time.time;
            }
        }

    }
    public void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D arrow_rb = arrow.GetComponent<Rigidbody2D>();
        Debug.Log("Arrow created");
        Debug.Log("Arrow velocity: " + arrow_rb.linearVelocity);
        if (enemyMovement.getDirection())
        {
            arrow_rb.linearVelocity = new Vector2(arrow_speed, 0f);
        }
        else
        {
            arrow_rb.linearVelocity = new Vector2(-arrow_speed, 0f);
        }
        animator.SetBool("isAttacking", false);
    }

    public void AttackEnd()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("idle", true);
        isAttacking = false;
    
    }

    public void EnableAttackZone()
    {
        attackZone.enabled = true;
    }
    public void DisableAttackZone()
    {
        attackZone.enabled = false;
    }

    public void IdleToWalk()
    {
        animator.SetBool("idle", false);
    }
    public void setFlag()
    {
        isAttacking = true;
    }
    public void resetFlag()
    {
        isAttacking = false;
    }
}
