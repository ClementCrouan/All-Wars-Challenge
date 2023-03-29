using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    public GameObject UIPanel;
    public Text pointsText;

    public int availablePoints;
    public string openKey;

    private bool isOpen;
    private PlayerInventory playerInv;

    // Start is called before the first frame update
    void Start()
    {
        playerInv = gameObject.GetComponent<PlayerInventory>();
        pointsText = GameObject.Find("availablePoints").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            isOpen = !isOpen;
        }

        if (isOpen)
        {
            pointsText.text = "Point disponible : " + availablePoints;
            UIPanel.SetActive(true);
        }
        else
        {
            UIPanel.SetActive(false);
        }
    }

    public void addHealthMax(float amountUp)
    {
        if (availablePoints >= 1)
        {
            playerInv.maxHealth += amountUp;
            availablePoints -= 1;
        }
    }
}
