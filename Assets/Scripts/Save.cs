using Assets.FantasyInventory.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int[] playerObjects = new int[12];
    //player objects : 0 = pLevel, 1=pGold, 2=pDiamond, 3=pSkillPoints, 4 = pCurrentExp, 5=pLeftExpreince, 6=CurrentWeDamage, 7 = currentWepEquipped, 8 = playerMaxHp, 9= cutscene, 10 = usedTotalSP;
    public List<Item> playerInventoryItems = new List<Item>();

}
