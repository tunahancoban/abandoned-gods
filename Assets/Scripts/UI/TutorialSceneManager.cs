using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialSceneManager : MonoBehaviour
{
    [SerializeField] String sceneName;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
