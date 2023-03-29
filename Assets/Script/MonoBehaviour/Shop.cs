using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Inventory inventoryPlayer;
    public PlayerInventory playerInv;
    Deplacement deplacement;
    ProgressState progressState;
    public GameObject shopPanel;
    public ItemDataBaseList itemDb;

    public string closeKey;

    public Sell[] sell;

    private int amountSlots;
    private int slotsChecked;
    private bool transactionDone;

    // Start is called before the first frame update
    void Start()
    {
        shopPanel.SetActive(false);
        deplacement = GameObject.FindGameObjectWithTag("Player").GetComponent<Deplacement>();
        progressState = GameObject.Find("ProgressState").GetComponent<ProgressState>();
    }


    void PrepareShop()
    {
        for (int i = 0; i < sell.Length; i++)
        {
            sell[i].theItem = itemDb.getItemByID(sell[i].itemID);

            sell[i].textItem.text = sell[i].theItem.itemName + " (Prix : " + sell[i].theItem.itemWeight + ") " + sell[i].theItem.itemDesc;

            sell[i].iconItem.sprite = sell[i].theItem.itemIcon;
        }
        sell[0].iconItem.transform.GetComponent<Button>().onClick.AddListener(delegate { BuyItem(sell[0].theItem); });
        sell[1].iconItem.transform.GetComponent<Button>().onClick.AddListener(delegate { BuyItem(sell[1].theItem); });
        sell[2].iconItem.transform.GetComponent<Button>().onClick.AddListener(delegate { BuyItem(sell[2].theItem); });

        shopPanel.SetActive(true);
    }

    void BuyItem(Item finalItem)
    {
        amountSlots = inventoryPlayer.transform.GetChild(1).childCount;
        transactionDone = false;
        slotsChecked = 0;

        for (int i = 0; i < amountSlots; i++)
        {
            ItemOnObject item = null;
            if (inventoryPlayer.transform.GetChild(1).GetChild(i).childCount != 0)
                item = inventoryPlayer.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>();
            if (item != null)
            {
                if (playerInv.goldCoins >= finalItem.itemWeight)
                {
                    if (item.item.itemIcon == finalItem.itemIcon && item.item.maxStack != item.item.itemValue)
                    {
                        item.item.itemValue += 1;
                        playerInv.goldCoins -= Mathf.RoundToInt(finalItem.itemWeight);
                        transactionDone = true;
                        break;
                    }
                }
                else
                {
                    progressState.ProgressAnimation("Vous n'avez pas assez d'argent !", 10);
                    break;
                }
            }
            slotsChecked++;
        }

        if (slotsChecked == amountSlots && !transactionDone)
        {
            slotsChecked = 0;
            for (int i = 0; i < amountSlots; i++)
            {
                if (i == -1)
                    i = 0;
                ItemOnObject item = null;
                if (inventoryPlayer.transform.GetChild(1).GetChild(i).childCount != 0)
                    item = inventoryPlayer.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>();
                if (item == null)
                {
                    if (playerInv.goldCoins >= finalItem.itemWeight)
                    {
                        inventoryPlayer.addItemToInventory(finalItem.itemID);
                        playerInv.goldCoins -= Mathf.RoundToInt(finalItem.itemWeight);
                        transactionDone = true;
                        break;
                    }
                    else
                    {
                        progressState.ProgressAnimation("Vous n'avez pas assez d'argent !", 10);
                        break;
                    }
                }
                slotsChecked++;
            }
        }

        if (slotsChecked == amountSlots && !transactionDone)
        {
            progressState.ProgressAnimation("Vous n'avez pas assez de place !", 10);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            deplacement.isInShop = true;
            PrepareShop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            sell[1].iconItem.GetComponent<Button>().onClick.RemoveAllListeners();
            sell[2].iconItem.GetComponent<Button>().onClick.RemoveAllListeners();
            sell[0].iconItem.GetComponent<Button>().onClick.RemoveAllListeners();

            shopPanel.SetActive(false);
            deplacement.isInShop = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            if (Input.GetKeyDown(closeKey))
                shopPanel.SetActive(false);
    }
}

[System.Serializable]

public struct Sell
{
    public int itemID;
    public Text textItem;
    public Image iconItem;
    [HideInInspector]
    public Item theItem;
}

