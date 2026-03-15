using UnityEngine;

public enum ArrowType
{
    ToPlayer,
    ToEnemy
}

public class Arrow : MonoBehaviour
{
    private SFXManager SFXManager;
    public ArrowType arrowType;
    private float arrow_speed;
    private Rigidbody2D rb;
    Camera mainCam;
    [SerializeField] float damage;
    void Start()
    {
        arrowType = ArrowType.ToPlayer;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        arrow_speed = rb.linearVelocityX;
        SFXManager = GameObject.FindWithTag("SFXManager")?.GetComponent<SFXManager>();
    }
    void Update()
    {
        Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);

        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
        {
            Destroy(gameObject);
        }
    }
    public void Reverse()
    {
        arrowType = ArrowType.ToEnemy;
        rb.linearVelocityX = -arrow_speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && arrowType == ArrowType.ToPlayer)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            SFXManager.playArrow();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") && arrowType == ArrowType.ToEnemy)
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            SFXManager.playArrow();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            SFXManager.playArrow();
            Destroy(gameObject);
        }
    }
   
}
