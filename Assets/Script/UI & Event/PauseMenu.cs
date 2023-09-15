using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject playerObject; // Reference to the player object

    private AudioSource bgmAudioSource;
    private List<AudioSource> playerAudioSources = new List<AudioSource>();
    private float normalVolume = 0.3f;
    private float pauseVolume = 0.1f;

    private float playerNormalVolume = 1f;
    private float playerPauseVolume = 0f;

    public void Start()
    {
        bgmAudioSource = Camera.main.GetComponent<AudioSource>();
        AudioSource[] audioSources = playerObject.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            playerAudioSources.Add(audioSource);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        bgmAudioSource.volume = normalVolume;
        foreach (AudioSource playerAudioSource in playerAudioSources)
        {
            playerAudioSource.volume = playerNormalVolume;
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        bgmAudioSource.volume = pauseVolume;
        foreach (AudioSource playerAudioSource in playerAudioSources)
        {
            playerAudioSource.volume = playerPauseVolume;
        }
    }

    public void retryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void ExitStage()
    {
        SceneManager.LoadScene(0);
    }


}