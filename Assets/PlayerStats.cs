using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Text levelText, goldText, diamondText;
    public int pLevel, pGold, pDiamond;
    GameSaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<GameSaveManager>();
        saveManager.LoadPlayerProgress();
        pLevel = saveManager.objects[0];
        pGold = saveManager.objects[1];
        pDiamond = saveManager.objects[2];
        pLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = pLevel.ToString();
        goldText.text = pGold.ToString();
        diamondText.text = pDiamond.ToString();
    }

    public void AddGold(int amount)
    {
        pGold += amount;
        saveManager.objects[1]+= amount;
    }

    public void AddDiamond(int amount)
    {
        pDiamond += amount;
        saveManager.objects[2]+=amount;
    }

    public void GainExp()
    {

    }

    public void LevelIncrease()
    {

    }
}
