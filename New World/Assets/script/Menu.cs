using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject pasueMenu;
    public AudioMixer audioMixer;

    public void PlayGame()
    {
        SceneManager.LoadScene("StartScene");
// SceneManager.GetActiveScene().buildIndex + 1
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void UIEnable()
    {
        GameObject.Find("Canvas/Menu/UI").SetActive(true);
    }

    public void PauseGame()
    {
        pasueMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pasueMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", value);
    }

    public void SetEffectVolume(float value)
    {
        audioMixer.SetFloat("EffectSoundVolume", value);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
