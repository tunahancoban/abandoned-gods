using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0;
    [SerializeField] float force = 0;

    [SerializeField] SFXManager SFXManager;
    private int jumpCount = 0;

    private Rigidbody2D rb;
    private InputAction action;
    private InputAction climb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlatformEffector2D currentEffector;
    private PlayerAttack attack;
    public bool isOnAir = false;
    public bool side = false;
    private bool isClimbing = false;
    private bool isInLadder = false;
    private float originalGravity;
    private bool isInteractLadderBefore = false;
    public bool isRunning = false;
    public bool isPraying = false;

    private bool isAlive = true;

    private bool isOnEffector = false;
    private PlayerHealth healthControl;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attack = GetComponent<PlayerAttack>();
        healthControl = GetComponent<PlayerHealth>();
        isAlive = healthControl.isAlive;

    }
    void Start()
    {
        action = InputSystem.actions.FindAction("Move");
        climb = InputSystem.actions.FindAction("Climb");
        originalGravity = rb.gravityScale;

    }

    void Update()
    {
        isAlive = healthControl.isAlive;
        if (!isAlive)
        {
            return;
        }
        if (attack.getAttackBool() && !isOnAir)
        {
            rb.linearVelocityX = 0;
        }
        else if (!isClimbing && !isPraying && !healthControl.isHealing && !attack.isAttacking)
        { //X ekseninde hareketi kontrol ediyor
            Vector2 moveVector = action.ReadValue<Vector2>();
            moveVector.Normalize();
            rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocity.y);
            if (moveVector.x < 0)
            {
                side = true;
                animator.SetBool("isRunning", true);
                isRunning = true;
            }
            else if (moveVector.x > 0)
            {
                side = false;
                animator.SetBool("isRunning", true);
                isRunning = true;
            }
            else
            {
                animator.SetBool("isRunning", false);
                isRunning = false;
            }
            spriteRenderer.flipX = side;
        }
        if (isInLadder && !isRunning && !isPraying && !healthControl.isHealing)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                climbing();
            }
            else if (isInteractLadderBefore)
            {
                stopOnLadder();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                leaveLadder();
            }

        }
        else if (!isPraying && !healthControl.isHealing)
        {
            //Double jump system
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.Space)) && jumpCount < 2)
            {
                SFXManager.stopWalk();
                SFXManager.playJump();
                if (jumpCount > 0)
                {
                    rb.AddForce(Vector2.up * (force / 2), ForceMode2D.Impulse);
                    animator.Play("jump", 0, 0f);

                }
                else
                {
                    rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                    animator.SetTrigger("jump");
                    isOnAir = true;
                }

                jumpCount++;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && !isPraying && !healthControl.isHealing && isOnEffector)
        {
            StartCoroutine(DropThrough());
        }

        if (rb.linearVelocityY < -0.1f)
        {
            animator.SetBool("isOnAir", true);
            SFXManager.stopWalk();
            isOnAir = true;
        }
        else
        {
            animator.SetBool("isOnAir", false);
        }
        if (Input.GetKeyDown(KeyCode.E) && !healthControl.isHealing)
        {
            SFXManager.stopWalk();
            rb.linearVelocityX = 0;
            animator.SetTrigger("prayTrigger");
            isPraying = true;
        }
      

    }
    IEnumerator DropThrough()
    {
        currentEffector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.5f);
        currentEffector.rotationalOffset = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.linearVelocityY < 0.1 && rb.linearVelocityY > -0.1)
        {


            jumpCount = 0;
            isOnAir = false;
            animator.ResetTrigger("jump");
            if (collision.collider.GetComponent<PlatformEffector2D>() != null)
            {
                currentEffector = collision.collider.GetComponent<PlatformEffector2D>();
                isOnEffector = true;
            }
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnEffector = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isInLadder = true;
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isInLadder = false;
            rb.gravityScale = originalGravity;
            isInteractLadderBefore = false;
            animator.ResetTrigger("isClimbing");
            animator.SetBool("cancelFall", false);
            animator.speed = 1;
            isClimbing = false;
        }
    }


    public bool getOnAir()
    {
        return isOnAir;
    }

    public void endPray()
    {
        isPraying = false;
    }
    public void climbing()
    {
        isClimbing = true;
        rb.gravityScale = 0;
        Vector2 climbVector = climb.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(rb.linearVelocityX, climbVector.y * moveSpeed);
        isInteractLadderBefore = true;
        animator.speed = 1;
        animator.SetTrigger("isClimbing");
        animator.SetBool("cancelFall", true);
        animator.SetBool("climbIsEnded", false);
    }
    public void stopOnLadder()
    {
        isClimbing = true;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
        animator.SetBool("cancelFall", true);
        animator.SetBool("climbIsEnded", false);
        animator.speed = 0;
    }
    public void leaveLadder()
    {
        isInteractLadderBefore = false;
        isClimbing = false;
        rb.gravityScale = originalGravity;
        animator.speed = 1;
        animator.SetBool("cancelFall", false);
        animator.SetBool("climbIsEnded", true);
    }
    public void resetAttackBool()
    {
        attack.setAttackBool(false);
    }
    public void Flip()
    {
        side = !side;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    public void resetIsOnAirBool()
    {
        isOnAir = false;
    }
    public void playWalkSFX()
    {
        if (!isOnAir)
        {
            SFXManager.playWalk();
        }
    }
    public void stopWalkSFX()
    {
        if (!isOnAir)
        {
            SFXManager.stopWalk();
        }
    }
    public void playLandingSFX()
    {
        SFXManager.playLandingSFX();
    }
}
