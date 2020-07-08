using Assets.FantasyInventory.Scripts.Data;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public List<int> objects = new List<int>();
    public List<Item> inventoryItems = new List<Item>();

    private void Awake()
    {
        Debug.Log("game save anager awake");
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
        SavePlayerProgress();
    }

    public void SavePlayerProgress()
    {
        SaveBySerilizationPlayer();

    }

    public void LoadPlayerProgress()
    {
        LoadBySerilizationPlayer();
    }

    public void ResetPlayerObjectsSave()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath +
                string.Format("/{0}.sav", i)))
            {
                File.Delete(Application.persistentDataPath +
                    string.Format("/{0}.sav", i));
            }
        }
    }

    public void LoadBySerilizationPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/Data.sav"))
        {
            FileStream fileStream = File.Open(Application.persistentDataPath + "/Data.sav", FileMode.Open);
            BinaryFormatter binary = new BinaryFormatter();
            Save save = binary.Deserialize(fileStream) as Save;
            fileStream.Close();

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i] = save.playerObjects[i];
            }
        }
        else
        {
            Debug.Log("no save file found");
        }
    }
    public void SaveBySerilizationPlayer()
    {
        Save save = createSavePlayerObjects();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/Data.sav");
        BinaryFormatter binary = new BinaryFormatter();
        binary.Serialize(fileStream, save);
        fileStream.Close();
    }

    public Save createSavePlayerObjects()
    {
        Save save = new Save();

        for (int i = 0; i < objects.Count; i++)
        {
            save.playerObjects[i] = objects[i];
        }
        return save;
    }

    public void SaveObject(int objectInList, int objectName)
    {
        objects[objectInList] = objectName;
        SavePlayerProgress();
    }
}
