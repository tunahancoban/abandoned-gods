
using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine;

public enum EnemyState
{
    Patrol,
    Alert,
    Dead
}
public enum EnemyType
{
        Archer,
        Swordman
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float force;
    [SerializeField] float knockBackDuration;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] float stopRange;
    [SerializeField] SFXManager SFXManager;
    [SerializeField] AudioSource walkSound;
    float tempSpeed;
    Rigidbody2D rb;
    Animator animator;
    public Transform groundCheck;
    public Transform playerCheck;
    public Transform firePosition;
    public LayerMask enemyLayer;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;
    private bool movingRight = true;
    public EnemyState state = EnemyState.Patrol;
    public EnemyType type;
    private RaycastHit2D groundInfo;
    bool isForce = false;



    // ===================== //
    // Eklenen Görüş Alanı Kodları
    public float viewDistance = 6f;
    [Range(0, 360)] public float viewAngle = 90f;
    public int rayCount = 15;
    public LayerMask targetMask;
    public Transform player;
    public float alertDuration = 3f;
    float alertTimer;
    public bool playerInSight = false;
    // ===================== //
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float maxHearingDistance = 10f;
    private float volume;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tempSpeed = speed;
        bloodEffect.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        if (isForce)
            return;
        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                MultiRayVisionCheck();
                if (playerInSight)
                {
                    state = EnemyState.Alert;
                    alertTimer = alertDuration;
                    if (type == EnemyType.Archer)
                    {
                        stopMovement();
                    }
                }
                break;

            case EnemyState.Alert:
                MultiRayVisionCheck();
                LookAtPlayer();

                if (type == EnemyType.Archer)
                {
                    animator.SetBool("isWalking", false);
                }
                else if (type == EnemyType.Swordman)
                {
                    MoveTowardsPlayer();
                }

                if (!playerInSight)
                {
                    alertTimer -= Time.deltaTime;
                }

                if (alertTimer <= 0f)
                {
                    state = EnemyState.Patrol;
                    if (type == EnemyType.Archer)
                    {
                        startMovement();
                    }
                }
                break;
            case EnemyState.Dead:
                break;
        }
        float dist = Vector2.Distance(transform.position, player.position);
        float t = Mathf.Clamp01(dist / maxHearingDistance);
        maxVolume = SFXManager.currentVolume;
        if (dist <= maxHearingDistance)
        {
            volume = Mathf.Lerp(maxVolume, 0f, dist / maxHearingDistance);
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void stopMovement()
    {
        animator.SetBool("idle", true);
        animator.SetBool("isWalking", false);
        speed = 0;
        rb.linearVelocity = Vector2.zero;
    }

    public void startMovement()
    {

        animator.SetBool("idle", false);
        animator.SetBool("isWalking", true);
        speed = tempSpeed;
    }

    public bool getDirection()
    {
        return movingRight;
    }

    public void Patrol()
    {
        playWalk(volume);
        if (movingRight)
        {

            rb.linearVelocityX = speed;
        }
        else
        {

            rb.linearVelocityX = -speed;
        }

        groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, groundInfo.collider ? Color.green : Color.red);

        if (groundInfo.collider == false)
        {
            Flip();
        }
    }

    // ============================ //
    // Eklenen Fonksiyonlar:

    void MultiRayVisionCheck()
    {
        playerInSight = false;

        float startAngle = -viewAngle / 2f;
        float angleStep = viewAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Vector2 rayDir = DirFromAngle(currentAngle);

            RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, rayDir, viewDistance, targetMask);

            Debug.DrawRay(playerCheck.position, rayDir * viewDistance, Color.yellow);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerInSight = true;
                alertTimer = alertDuration;
                break;
            }
        }
    }

    Vector2 DirFromAngle(float angleInDegrees)
    {
        if (!movingRight)
            angleInDegrees = 180f - angleInDegrees;

        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public void LookAtPlayer()
    {
        Vector2 direction = player.position - transform.position;
        if ((direction.x > 0 && !movingRight) || (direction.x < 0 && movingRight))
        {
            Flip();
        }
    }
    void MoveTowardsPlayer()
    {
        LookAtPlayer();

        groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, groundInfo.collider ? Color.green : Color.red);
        if (groundInfo.collider == false)
        {
            Debug.Log("Kontrol Noktasi");
            state = EnemyState.Patrol;
            Flip();
            return;
        }
        if (isForce)
        {
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= stopRange)
        {
            stopMovement();
            return;
        }
        else
        {
            playWalk(volume);
            startMovement();
            animator.SetBool("isWalking", true);
        }


        if (movingRight)
        {
            rb.linearVelocityX = speed;
        }
        else
        {
            rb.linearVelocityX = -speed;
        }

    }
    public void addForce()
    {
        Debug.Log("test");
        bloodEffect.GetComponent<SpriteRenderer>().enabled = true;
        isForce = true;
        GetComponent<EnemyAttack>().isAttacking = false;
        if (movingRight)
        {
            rb.AddForce(Vector2.left * force, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.right * force, ForceMode2D.Impulse);
        }
        StartCoroutine(Knockback(knockBackDuration));

    }
    IEnumerator Knockback(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }


        rb.linearVelocity = Vector2.zero;
        isForce = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Flip();
        }
    }
    public void bowSFX()
    {
        SFXManager.playBow();
    }

    public void attackSFX()
    {
        SFXManager.playAttack2();
    }
    
    public void playWalk(float volume)
    {
        if (!walkSound.isPlaying)
        {
            walkSound.volume = volume;
            walkSound.Play();
        }
    }
    
}
