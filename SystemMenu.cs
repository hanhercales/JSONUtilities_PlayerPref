using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SystemMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Button continueButton;

    public int sceneToContinue;

    private void Start()
    {
        sceneToContinue = PlayerPrefs.GetInt("SavedScene");
        DataPersistenceManager.instance.LoadGame();
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
        }
    }

    public void NewGame()
    {
        DataPersistenceManager.instance.NewGame();
        StartCoroutine(Delay(1));
    }

    public void Continue()
    {
        DataPersistenceManager.instance.Transfer();
        
        if (sceneToContinue != 0)
        {
            StartCoroutine(Delay(sceneToContinue));
        }
    }
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Delay(int scene)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(scene);
    }
}
