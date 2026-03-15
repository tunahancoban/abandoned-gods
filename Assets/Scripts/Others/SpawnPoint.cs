using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
          
            PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            if (playerMovement.isPraying)
            {
                Debug.Log("test_2");
                SpawnManager.Instance.spawnSave(gameObject);
            }
        }
    }

}
