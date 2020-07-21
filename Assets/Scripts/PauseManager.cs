using Assets.FantasyInventory.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Inventory, Shop;
    public bool inventoryShown, shopShown;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInventory()
    {
        inventoryShown = !inventoryShown;
        if (inventoryShown)
        {
            Inventory.GetComponent<Inventory>().toInitialize = true;
            Inventory.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Inventory.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ShowShop()
    {
        shopShown = !shopShown;
        if (shopShown)
        {
            Inventory.GetComponent<Inventory>().toInitialize = true;
            Shop.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Shop.SetActive(false);
            Time.timeScale = 1f;
        }
    }

}
