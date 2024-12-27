using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] [SerializeField]
    private string fileName, tmpFileName;

    [SerializeField] private bool useEncryption;
    
    private GameData gameData, tmpGameData;

    private List<IDataPersistence> dataPersistencesObjects;
    private FileDataHandler dataHandler, tmpDataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        
        DontDestroyOnLoad(this.gameObject);
        
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        tmpDataHandler = new FileDataHandler(Application.persistentDataPath, tmpFileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called!");
        this.dataPersistencesObjects = FindAllDataPersistenceObjects();
        LoadTmpGame();
    }
    
    public void NewGame()
    {
        tmpGameData = new GameData();
        gameData = new GameData();
        SaveTmpGame();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        
        if (gameData == null)
        {
            Debug.Log("No data was found.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogWarning("No data was found!");
            return;
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        
        dataHandler.Save(gameData);
    }
    
    public void LoadTmpGame()
    {
        tmpGameData = tmpDataHandler.Load();
        
        if (tmpGameData == null)
        {
            Debug.Log("No data was found.");
            return;
        }

        foreach (IDataPersistence tmpDataPersistenceObj in dataPersistencesObjects)
        {
            tmpDataPersistenceObj.LoadData(tmpGameData);
        }
    }

    public void SaveTmpGame()
    {
        if (tmpGameData == null)
        {
            Debug.LogWarning("No data was found!");
            return;
        }
        foreach (IDataPersistence tmpDataPersistenceObj in dataPersistencesObjects)
        {
            tmpDataPersistenceObj.SaveData(tmpGameData);
        }
        
        tmpDataHandler.Save(tmpGameData);
    }

    public void Transfer()
    {
        tmpGameData = gameData;
        SaveTmpGame();
    }

    public void DeleteTmp()
    {
        if (tmpGameData == null)
        {
            Debug.LogWarning("No data was found!");
            return;
        }
        foreach (IDataPersistence tmpDataPersistenceObj in dataPersistencesObjects)
        {
            tmpDataPersistenceObj.DeleteData(tmpGameData);
        }
        
        tmpDataHandler.Delete(tmpGameData);
    }

    private void OnApplicationQuit()
    {
        tmpDataHandler.Delete(tmpGameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistencesObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }
}
