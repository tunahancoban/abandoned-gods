using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SoundController : MonoBehaviour
{
    public static SoundController Instance; // 🔹 Singleton referansı

    private AudioSource backgroundSounds;
    [SerializeField] List<AudioSource> sfxList;
    private Slider backgroundSlider;

    private Slider SFXSlider;
    private SFXManager SFXManager;

    void Awake()
    {
        // 🔐 Eğer daha önce oluşturulmuşsa bunu yok et
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 🔐 İlk ve tek örnek buysa:
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Sadece ilk kez abone ol
        SceneManager.sceneLoaded += OnSceneLoaded;

        // İlk çalıştırmada volume uygula
        float savedVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        if (backgroundSounds != null)
            backgroundSounds.volume = savedVolume;
    }

    public void updateBackgroundSound()
    {
        if (backgroundSlider != null && backgroundSounds != null)
        {
            float volume = backgroundSlider.value;
            backgroundSounds.volume = volume;
            PlayerPrefs.SetFloat("BackgroundVolume", volume);
            Debug.Log("Volume updated: " + volume);
        }
    }
    public void updateSFXSound()
    {
        if (SFXSlider != null && SFXManager != null)
        {
            float volume = SFXSlider.value;
            SFXManager.changeVolume(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
            Debug.Log("SFX Volume updated: " + volume);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
       
        SFXManager = GameObject.FindWithTag("SFXManager")?.GetComponent<SFXManager>();
        SFXSlider =  GameObject.FindWithTag("SFXSlider")?.GetComponent<Slider>();
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        if (backgroundSounds == null)
        {
            Debug.Log("test sound");
        }
         backgroundSounds = GameObject.FindWithTag("BackgroundSound")?.GetComponent<AudioSource>();
         if (SFXManager != null)
            SFXManager.changeVolume(savedSFXVolume);

        if (SFXSlider != null)
        {
            SFXSlider.value = savedSFXVolume;
            SFXSlider.onValueChanged.RemoveAllListeners();
            SFXSlider.onValueChanged.AddListener(delegate { updateSFXSound(); });
        }


        backgroundSounds = GameObject.FindWithTag("BackgroundSound")?.GetComponent<AudioSource>();
        backgroundSlider = GameObject.Find("BackgroundSounds")?.GetComponent<Slider>();

        float savedVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);

        if (backgroundSounds != null)
            backgroundSounds.volume = savedVolume;

        if (backgroundSlider != null)
        {
            backgroundSlider.value = savedVolume;
            backgroundSlider.onValueChanged.RemoveAllListeners();
            backgroundSlider.onValueChanged.AddListener(delegate { updateBackgroundSound(); });
        }
        else
        {
            Debug.LogWarning("Slider not found in scene!");
        }
    }

    private void OnDestroy()
    {
        // 🔄 Sahne kapanırken çıkış yap
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
