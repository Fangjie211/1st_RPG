using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private GameData gameData1;
    private GameData gameData2;
    private GameData gameData3;
    public static SaveManager instance;
    private FileDataHandler dataHandler;
    private List<ISavemanager> saveManagers;
    [SerializeField] private string fileName;

    [SerializeField] private bool encryptData;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [ContextMenu("Delete save file")]
    public void DeleteSavedFile()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        dataHandler.Delete();
    }
    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName,encryptData);
        saveManagers=FindAllSaveManagers();
        LoadGame();
    }
    public void NewGame()
    {
        gameData1 = new GameData();
    }

    public void LoadGame()
    {
        gameData1 = dataHandler.Load();
        if (this.gameData1 == null)
        {
            Debug.Log("no data found");
            NewGame();
        }
        foreach (ISavemanager manager in saveManagers)
        {
            manager.LoadData(gameData1);
        }
    }

    public void SaveGame()
    {
        foreach(ISavemanager save in saveManagers)
        {
            save.SaveData(ref gameData1);
        }
        dataHandler.Save(gameData1);
        Debug.Log("Saved");

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISavemanager> FindAllSaveManagers()
    {
        IEnumerable<ISavemanager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISavemanager>();
        return new List<ISavemanager>(saveManagers);
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }
        return false;

    }
}
