using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] Canvas pauseMenu;
    void Start()
    {
        pauseMenu.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.enabled)
            {
                OffMenu();
            }
            else
            {
                OpenMenu();
            }

        }
    }
    void OpenMenu()
    {
        pauseMenu.enabled = true;
        Time.timeScale = 0f;
    }
    void OffMenu()
    {
        pauseMenu.enabled = false;
        Time.timeScale = 1f;
    }
}
