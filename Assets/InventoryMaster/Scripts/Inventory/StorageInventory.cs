using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
#endif
using System.Collections.Generic;
using UnityEngine.UI;

public class StorageInventory : MonoBehaviour
{

    [SerializeField]
    public GameObject storage;
    public GameObject inventory;
    public GameObject equipmentSystem;

    public int numberItemStorage;

    [SerializeField]
    public List<Item> storageItems = new List<Item>();

    [SerializeField]
    private ItemDataBaseList itemDatabase;

    [SerializeField]
    public int distanceToOpenStorage;

    public float timeToOpenStorage;

    private InputManager inputManagerDatabase;

    float startTimer;
    float endTimer;
    bool showTimer;

    public int itemAmount;

    Tooltip tooltip;
    Inventory inv;

    GameObject player;

    static Image timerImage;
    static GameObject timer;

    bool closeInv;

    bool showStorage;

    public bool isChest;

    public void addItemToStorage(int id, int value)
    {
        Item item = itemDatabase.getItemByID(id);
        item.itemValue = value;
        storageItems.Add(item);
    }

    void Start()
    {
        if (inputManagerDatabase == null)
            inputManagerDatabase = (InputManager)Resources.Load("InputManager");

        player = GameObject.FindGameObjectWithTag("Player");
        inv = storage.GetComponent<Inventory>();
        ItemDataBaseList inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        int creatingItemsForChest = 0;

        int randomItemAmount = 0;

        if (itemAmount > numberItemStorage && itemAmount > 0)
            randomItemAmount = Random.Range(1, itemAmount);

        if (creatingItemsForChest != randomItemAmount)
        {
            while (creatingItemsForChest < randomItemAmount)
            {
                int randomItemNumber = Random.Range(1, inventoryItemList.itemList.Count - 1);
                int raffle = Random.Range(1, 100);

                if (raffle <= inventoryItemList.itemList[randomItemNumber].rarity)
                {
                    int randomValue = Random.Range(1, inventoryItemList.itemList[randomItemNumber].getCopy().maxStack);
                    Item item = inventoryItemList.itemList[randomItemNumber].getCopy();
                    item.itemValue = randomValue;
                    storageItems.Add(item);
                    creatingItemsForChest++;
                }
            }
        }

        if (GameObject.FindGameObjectWithTag("Timer") != null)
        {
            timerImage = GameObject.FindGameObjectWithTag("Timer").GetComponent<Image>();
            timer = GameObject.FindGameObjectWithTag("Timer");
            timer.SetActive(false);
        }
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();

    }

    public void setImportantVariables()
    {
        if (itemDatabase == null)
            itemDatabase = (ItemDataBaseList)Resources.Load("ItemDatabase");
    }

    void Update()
    {

        float distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);

        if (showTimer)
        {
            if (timerImage != null)
            {
                timer.SetActive(true);
                float fillAmount = (Time.time - startTimer) / timeToOpenStorage;
                timerImage.fillAmount = fillAmount;
            }
        }

        if (showStorage)
        {
            inventory.SetActive(true);
            equipmentSystem.SetActive(true);
            player.GetComponent<PlayerInventory>().chestIsOpen = true;
        }
        if (!showStorage)
        {
            player.GetComponent<PlayerInventory>().chestIsOpen = false;
        }

        if (distance <= distanceToOpenStorage && Input.GetKeyDown(inputManagerDatabase.StorageKeyCode) && !showStorage)
        {
            showStorage = true;
            StartCoroutine(OpenInventoryWithTimer());
        }
        else if (distance <= distanceToOpenStorage && Input.GetKeyDown(inputManagerDatabase.StorageKeyCode) && showStorage)
        {
            showStorage = false;
            StartCoroutine(OpenInventoryWithTimer());
        }

        if (distance > distanceToOpenStorage && showStorage)
        {
            if (isChest)
            {
                gameObject.GetComponent<Animator>().SetBool("IsOpen", false);
            }

            showStorage = false;
            inventory.SetActive(false);
            equipmentSystem.SetActive(false);

            if (storage.activeSelf)
            {
                storageItems.Clear();
                setListofStorage();
                storage.SetActive(false);
                inv.deleteAllItems();
            }
            tooltip.deactivateTooltip();
            timerImage.fillAmount = 0;
            timer.SetActive(false);
            showTimer = false;
        }
    }

    IEnumerator OpenInventoryWithTimer()
    {
        if (showStorage)
        {
            startTimer = Time.time;
            showTimer = true;
            yield return new WaitForSeconds(timeToOpenStorage);
            if (showStorage)
            {
                if (isChest)
                {
                    gameObject.GetComponent<Animator>().SetBool("IsOpen", true);
                }

                inv.ItemsInInventory.Clear();
                storage.SetActive(true);

                addItemsToInventory();
                showTimer = false;
                if (timer != null)
                    timer.SetActive(false);
            }
        }
        else
        {
            if (isChest)
            {
                gameObject.GetComponent<Animator>().SetBool("IsOpen", false);
            }

            storageItems.Clear();
            setListofStorage();
            storage.SetActive(false);
            inventory.SetActive(false);
            equipmentSystem.SetActive(false);
            inv.deleteAllItems();
            tooltip.deactivateTooltip();
        }


    }



    void setListofStorage()
    {
        Inventory inv = storage.GetComponent<Inventory>();
        storageItems = inv.getItemList();
    }

    void addItemsToInventory()
    {
        Inventory iV = storage.GetComponent<Inventory>();
        for (int i = 0; i < storageItems.Count; i++)
        {
            iV.addItemToInventory(storageItems[i].itemID, storageItems[i].itemValue);
        }
        iV.stackableSettings();
    }






}
