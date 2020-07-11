using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Interface.Elements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public List<int> objects = new List<int>();
    public List<Item> inventoryItems = new List<Item>();
    public PlayerStats pStats;

    Save saveFile = new Save();

    bool encrypt = false;
    string fullPath;

        static string logText = "";


    private void Awake()
    {
        Debug.Log("game save anager awake");
        pStats = GameObject.FindObjectOfType<PlayerStats>();
        fullPath = Application.persistentDataPath + "/" + "LocalFileName";
        LoadGameValues();
        Debug.Log(Application.persistentDataPath + "/" + "LocalFileName");

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnApplicationQuit()
    {
        SaveGameValues();
        Debug.Log("save to file: " + Application.persistentDataPath);
    }

    public void LoadGameValues()
    {
#if JSONSerializationFileSave || BinarySerializationFileSave
        logText += "\nLoad Started (File): " + fullPath;
#else
        logText += "\nLoad Started (PlayerPrefs): " + fullPath;
#endif
        SaveManager.Instance.Load<Save>(fullPath, DataWasLoaded, encrypt);
    }

    private void DataWasLoaded(Save data, SaveResult result, string message)
    {
        Debug.Log("Data was loaded");

        if (result == SaveResult.EmptyData || result == SaveResult.Error)
        {
            Debug.LogWarning("No Data File > Create New");
            saveFile = new Save();
        }

        if (result == SaveResult.Success)
        {
            saveFile = data;
        }
        objects[0] = saveFile.playerObjects[0];
        objects[1] = saveFile.playerObjects[1];
        objects[2] = saveFile.playerObjects[2];
        objects[3] = saveFile.playerObjects[3];
        objects[4] = saveFile.playerObjects[4];
        objects[5] = saveFile.playerObjects[5];
        objects[6] = saveFile.playerObjects[6];
        objects[7] = saveFile.playerObjects[7];
        if (saveFile.playerInventoryItems.Count > 0)
        {
            for (int i = 0; i < saveFile.playerInventoryItems.Count; i++)
            {
                inventoryItems.Add(new Item(saveFile.playerInventoryItems[i].Id, saveFile.playerInventoryItems[i].Count));
            }
        }

    }

    public void SaveGameValues()
    {
        logText += "\nSave Started";
        saveFile.playerObjects[0] = objects[0];
        saveFile.playerObjects[1] = objects[1];
        saveFile.playerObjects[2] = objects[2];
        saveFile.playerObjects[3] = objects[3];
        saveFile.playerObjects[4] = objects[4];
        saveFile.playerObjects[5] = objects[5];
        saveFile.playerObjects[6] = objects[6];
        saveFile.playerObjects[7] = objects[7];

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (!saveFile.playerInventoryItems.Any(j => j.Id == inventoryItems[i].Id))
            {
                saveFile.playerInventoryItems.Add(new Item(inventoryItems[i].Id, inventoryItems[i].Count));
                Debug.Log("added item to inventory" + inventoryItems[i].Id);
            }
            else
            {
                saveFile.playerInventoryItems[i].Count = inventoryItems[i].Count;
            }
        }

        for (int i = 0; i < saveFile.playerInventoryItems.Count; i++)
        {
            if (!inventoryItems.Any(j => j.Id == saveFile.playerInventoryItems[i].Id))
            {
                saveFile.playerInventoryItems.Remove(saveFile.playerInventoryItems[i]);
            }
        }
        SaveManager.Instance.Save(saveFile, fullPath, DataWasSaved, encrypt);
    }

    public void removeItemFromSave(Item item)
    {
    }

    private void DataWasSaved(SaveResult result, string message)
    {
        Debug.Log("Data Was Saved");
        if (result == SaveResult.Error)
        {
            Debug.LogWarning("Error saving data");
        }
    }


    //public void SavePlayerProgress()
    //{
    //    SaveBySerilizationPlayer();

    //}

    //public void LoadPlayerProgress()
    //{
    //    LoadBySerilizationPlayer();
    //}

    //public void ResetPlayerObjectsSave()
    //{
    //    for (int i = 0; i < objects.Count; i++)
    //    {
    //        if (File.Exists(Application.persistentDataPath +
    //            string.Format("/{0}.sav", i)))
    //        {
    //            File.Delete(Application.persistentDataPath +
    //                string.Format("/{0}.sav", i));
    //        }
    //    }
    //}
    //public void LoadBySerilizationPlayer()
    //{
    //    if (File.Exists(Application.persistentDataPath + "/Data.sav"))
    //    {
    //        FileStream fileStream = File.Open(Application.persistentDataPath + "/Data.sav", FileMode.Open);
    //        BinaryFormatter binary = new BinaryFormatter();
    //        Save save = binary.Deserialize(fileStream) as Save;
    //        fileStream.Close();

    //        for (int i = 0; i < objects.Count; i++)
    //        {
    //            objects[i] = save.playerObjects[i];
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("no save file found");
    //    }
    //}
    //public void SaveBySerilizationPlayer()
    //{
    //    Save save = createSavePlayerObjects();
    //    FileStream fileStream = File.Create(Application.persistentDataPath + "/Data.sav");
    //    BinaryFormatter binary = new BinaryFormatter();
    //    binary.Serialize(fileStream, save);
    //    fileStream.Close();
    //}
    //public Save createSavePlayerObjects()
    //{
    //    Save save = new Save();
    //    for (int i = 0; i < objects.Count; i++)
    //    {
    //        save.playerObjects[i] = objects[i];
    //    }
    //    return save;
    //}
    //public void SaveObject(int objectInList, int objectName)
    //{
    //    objects[objectInList] = objectName;
    //    SavePlayerProgress();
    //}
}
