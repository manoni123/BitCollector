using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addGold : MonoBehaviour
{
    // Update is called once per frame
    GameSaveManager save;
    PlayerStats pStats;

    void Start()
    {
        save = GameObject.FindObjectOfType<GameSaveManager>();
        pStats = FindObjectOfType<PlayerStats>();
    }

    void AddGold()
    {
        pStats.AddGold(15000);
    }
}


