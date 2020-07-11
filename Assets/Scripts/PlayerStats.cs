using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Text levelText, goldText, diamondText;
    [Header("player info")]
    public int pLevel, pGold, pDiamond;

    [Header("Stats")]
    public Text playerStats;
    public int playerHealth, playerMana, playerAttack, playerDefense, playerAttackSpeed;
    GameSaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<GameSaveManager>();
        pLevel = saveManager.objects[0];
        pGold = saveManager.objects[1];
        pDiamond = saveManager.objects[2];
        playerHealth = saveManager.objects[3];
        playerMana = saveManager.objects[4];
        playerAttack = saveManager.objects[5];
        playerDefense = saveManager.objects[6];
        playerAttackSpeed = saveManager.objects[7];
        pLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = pLevel.ToString();
        goldText.text = pGold.ToString();
        diamondText.text = pDiamond.ToString();
        //health, mana, attack, defense, attackSpeed
        playerStats.text = playerHealth.ToString() + Environment.NewLine +
                            playerMana.ToString() + Environment.NewLine +
                            playerAttack.ToString() + Environment.NewLine +
                            playerDefense.ToString() + Environment.NewLine +
                            playerAttackSpeed.ToString();
    }

    public void AddGold(int amount)
    {
        pGold += amount;
        saveManager.objects[1]+= amount;
        saveManager.SaveGameValues();
    }

    public void AddDiamond(int amount)
    {
        pDiamond += amount;
        saveManager.objects[2]+=amount;
        saveManager.SaveGameValues();
    }

    public void GainExp()
    {

    }

    public void LevelIncrease()
    {

    }
}
