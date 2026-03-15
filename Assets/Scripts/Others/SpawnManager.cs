using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private GameObject spawnPoint; // ilk spawnpoint sahneye atanmalı

    private Vector3 savedPosition;
    private float savedHealth;

    private bool hasSavedData = false;
    private string sceneCreatedIn;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sceneCreatedIn = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Yalnızca ilk kez pozisyon belirlemek için çalışır
        if (!hasSavedData && spawnPoint != null)
        {
            savedPosition = spawnPoint.transform.position;
            hasSavedData = true;

            PlayerHealth playerHealth = GameObject.FindWithTag("Player")?.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                savedHealth = playerHealth.GetCurrentHealth();
        }
    }

    public void spawnSave(GameObject other)
    {
        PlayerHealth playerHealth = GameObject.FindWithTag("Player")?.GetComponent<PlayerHealth>();
        if (playerHealth == null) return;

        Debug.Log("Yeni checkpoint kaydedildi.");
        savedPosition = other.transform.position;
        savedHealth = playerHealth.GetCurrentHealth();
        hasSavedData = true;
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        if (scene.name != sceneCreatedIn)
        {
            Debug.Log("SpawnManager farklı sahneye geçti → yok ediliyor.");
            Destroy(gameObject);
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && hasSavedData)
        {
            Debug.Log($"Oyuncu respawn oluyor. Pozisyon: {savedPosition}");
            player.transform.position = savedPosition;

            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
                health.SetHealth(savedHealth);
        }
        else
        {
            Debug.LogWarning("Respawn için oyuncu veya veri bulunamadı.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
