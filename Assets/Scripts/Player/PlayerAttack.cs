
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float baseDamage;
    [SerializeField] float cooldown;
    [SerializeField] float range;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] SFXManager SFXManager;
    [Header("Direction and Point")]
    [SerializeField] Transform attackPoint;
    [SerializeField] Collider2D attackZone;
    [SerializeField] Vector2 attackDirection = Vector2.right;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float lastAttackTime;
    private float nextAttackTime = 0f;
    private int comboIndex = 0;
    public float comboResetTime = 0.5f;
    public bool isAttacking = false;
   
    private PlayerMovement playerMovement;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        attackZone.enabled = false;
    }

    void Update()
    {
        Vector2 zoneLocalPos = attackZone.transform.localPosition;
        if (playerMovement.side)
        {
            zoneLocalPos.x = -1;
        }
        else
        {
            zoneLocalPos.x = 1;
        }
        attackZone.transform.localPosition = zoneLocalPos;
        // Eğer çok zaman geçtiyse combo sıfırlanır (ama animasyon sırasında değil)
        if (Time.time - lastAttackTime > comboResetTime && !isAttacking)
        {
            ResetCombo();
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && !isAttacking && !playerMovement.getOnAir())
        {

            lastAttackTime = Time.time;
            nextAttackTime = Time.time + cooldown;
            animator.SetInteger("comboIndex", comboIndex);
            animator.SetTrigger("attack");
            isAttacking = true; // Animasyon süresince yeni saldırıyı engelle
            comboIndex = (comboIndex + 1) % 4;
        }
    }

    public void ResetCombo()
    {
        comboIndex = 0;
        animator.SetInteger("comboIndex", 0);
    }

    // Animasyon bittiğinde çağrılmalı
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }

    public void GoToIdle()
    {
        animator.SetTrigger("attackIsEnded");
    }
    public bool getAttackBool()
    {
        return isAttacking;
    }
    public void setAttackBool(bool attacking)
    {
        isAttacking = attacking;
    }
    public void EnableAttackZone()
    {
        attackZone.enabled = true;
    }
    public void DisableAttackZone()
    {
        attackZone.enabled = false;
    }
    public void playAttackSFX()
    {
        SFXManager.playAttack();
    }
    public void playAttackHitSFX()
    {
        SFXManager.playHit();
    }
}
