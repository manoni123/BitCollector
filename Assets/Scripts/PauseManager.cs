using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Inventory, Shop;
    private bool isShown, inventoryShown, shopShown;
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
