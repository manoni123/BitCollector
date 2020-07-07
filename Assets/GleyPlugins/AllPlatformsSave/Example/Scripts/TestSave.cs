using UnityEngine;

public class TestSave : MonoBehaviour
{
    //local data
    public PlayerStats pStats;
    string randomText = "";
    bool showVideo;
    float musicVolume;


    //the object you want to serialize
    GameValues gameValues = new GameValues();

    string fullPath;

    //if true data will be encripted with an XOR function
    bool encrypt = false;

    //for debug purpose only
    static string logText = "";

    void Awake()
    {
        //the filename where saved data will be stored
        pStats = GameObject.FindObjectOfType<PlayerStats>();
        fullPath = Application.persistentDataPath + "/" + "LocalFileName";
        LoadGameValues();
        Debug.Log(Application.persistentDataPath + "/" + "LocalFileName");
    }

    
    private void OnApplicationQuit()
    {
        SaveGameValues();
    }

    //called from scene using built in unity events
    public void LoadGameValues()
    {
#if JSONSerializationFileSave || BinarySerializationFileSave
        logText += "\nLoad Started (File): " + fullPath;
#else
        logText += "\nLoad Started (PlayerPrefs): " + fullPath;
#endif
        SaveManager.Instance.Load<GameValues>(fullPath, DataWasLoaded, encrypt);
    }

    private void DataWasLoaded(GameValues data, SaveResult result, string message)
    {
        Debug.Log("Data was loaded");

        if (result == SaveResult.EmptyData || result == SaveResult.Error)
        {
            Debug.LogWarning("No Data File > Create New");
            gameValues = new GameValues();
        }

        if (result == SaveResult.Success)
        {
            gameValues = data;
        }
        randomText = gameValues.randomText;
        showVideo = gameValues.showVideo;
        musicVolume = gameValues.musicVolume;
        pStats.pGold = gameValues.totalCoins;
        pStats.pDiamond = gameValues.totalDiamond;
        pStats.pLevel = gameValues.playerLevel;
    }

    public void SaveGameValues()
    {
        logText += "\nSave Started";
        gameValues.randomText = randomText;
        gameValues.showVideo = showVideo;
        gameValues.musicVolume = musicVolume;
        gameValues.totalCoins = pStats.pGold;
        gameValues.totalDiamond = pStats.pDiamond;
        gameValues.playerLevel = pStats.pLevel;
        SaveManager.Instance.Save(gameValues, fullPath, DataWasSaved, encrypt);
    }

    private void DataWasSaved(SaveResult result, string message)
    {
        Debug.Log("Data Was Saved");
        if (result == SaveResult.Error)
        {
            Debug.LogWarning("Error saving data");
        }
    }
}
