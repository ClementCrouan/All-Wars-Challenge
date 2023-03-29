using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    public Deplacement deplacement;
    public PlayerInventory playerInv;
    Tooltip tooltip;

    // Start is called before the first frame update
    void Start()
    {
        deplacement = GameObject.FindGameObjectWithTag("Player").GetComponent<Deplacement>();
        playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
    }

    public void sellItem()
    {
        if (Input.GetKey(KeyCode.LeftShift) && deplacement.isInShop)
        {
            int itemValue = GetComponent<ItemOnObject>().item.itemValue;

            playerInv.goldCoins += GetComponent<ItemOnObject>().itemPrice * itemValue;
            tooltip.deactivateTooltip();
            Destroy(gameObject);
        }
    }
}
