using Assets.FantasyInventory.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int[] playerObjects = new int[12];
    //player objects : 0 = pLevel, 1=pGold, 2=pDiamond, 3=Health, 4 = Mana, 5=Attack, 6=Defense, 7 = AttackSpeed, 8 = playerMaxHp, 9= cutscene, 10 = usedTotalSP;
    public List<Item> playerInventoryItems = new List<Item>();
    public List<Item> playerEquipmentItems = new List<Item>();


}
