using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private string animalScene ="Animals Scene";
    private string birdsScene= "birds Scene";
    private string bodyPartsScene= "BodyParts Scene";
    private string colorScene ="color Scene";
    private string fruitsScene ="Fruites Scene";
    private string numberScene="Numbers Scene";
    private string vehicleScene = "vehicles Scene";
    private string vegetableScene ="Vegetables Scene";
    public GameObject imagePanel;
    public GameObject pausePanel;
    public Button infobtn;

    public AudioSource audioSource;

    // private void Awake()
    // {
    //     DontDestroyOnLoad(audioSource);
    // }
    public void AnimalScene()
    {
        SceneManager.LoadScene(animalScene);
    }
    public void VehicleScene()
    {
        SceneManager.LoadScene(vehicleScene);
    }
    public void BirdsScene()
    {
        SceneManager.LoadScene(birdsScene);
    }
    public void BodyPartsScene()
    {
        SceneManager.LoadScene(bodyPartsScene);
    }
    public void NumberScene()
    {
        // Debug.Log("Scene numbers is loading...");
        SceneManager.LoadScene(numberScene);
    }
    public void ColorScene()
    {
        SceneManager.LoadScene(colorScene);
    }
    public void VegetableScene()
    {
        SceneManager.LoadScene(vegetableScene);
    }
    public void FruitsScene()
    {
        //   Debug.Log("Scene fruits is loading...");
        SceneManager.LoadScene(fruitsScene);
    }

    public void InfoBtnCall()
    {
        if(imagePanel.activeInHierarchy)
        {
            // mainGamePanel.SetActive(false);
            imagePanel.SetActive(false);
            infobtn.gameObject.SetActive(false);
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeBtnCall()
    {
        if(pausePanel.activeInHierarchy)
        {
            imagePanel.gameObject.SetActive(true);
            infobtn.gameObject.SetActive(true);
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void HomeBtnCall()
    {
        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene("ScrollSelectScene");
    }
    public void QuitBtnCall()
    {
        Application.Quit();
    }

}
