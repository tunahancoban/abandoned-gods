using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] GameObject player;
    private PlayerHealth playerHealth;
    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
   
    }


    void Update()
    {
        float health = playerHealth.updateHealth();
        updateHealthBar(health);
    }
    public void updateHealthBar(float health)
    {
        scrollbar.size = health;
    }
}
