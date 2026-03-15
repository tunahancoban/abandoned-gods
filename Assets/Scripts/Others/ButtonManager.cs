using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private SpawnManager spawnManager;
    void Start()
    {
        spawnManager = GameObject.FindWithTag("RespawnManager").GetComponent<SpawnManager>();
    }

    public void respawnButton()
    {
        spawnManager.Respawn();
    }
}
