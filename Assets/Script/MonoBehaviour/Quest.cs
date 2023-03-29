using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public QuestObjective[] enemyOrObject;
    public GameObject[] otherObject;

    [HideInInspector] public bool showGUI = false;

    [HideInInspector]
    public int currentEnemyKilled;
    public int maxEnemyKilled;

    PanelQuestState panelQuestState;
    GameObject panelQuest;
    Text questT;
    Text questI;
    Button questR;
    Button questA;

    public int questXP;
    public int questGoldCoins;

    public int iDQuest;
    public string questName;
    public string questInstruction;

    [Header("Size")]
    public float heightPanel;
    public float widthPanel;
    public float heightTitle;
    public float widthTitle;
    public float heightText;
    public float widthText;
    public float heightButton;
    public float widthButton;

    [Header("Position")]
    public float posXButtonAccept;
    public float posYButton;
    public float posXButtonRefuse;

    private PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        for (int i = 0; i < enemyOrObject.Length; i++)
        {
            if (enemyOrObject[i].objective.activeInHierarchy)
            {
                enemyOrObject[i].objective.SetActive(false);
                enemyOrObject[i].isVisible = false;
            }
        }

        for (int i = 0; i < otherObject.Length; i++)
        {
            if (otherObject[i].activeInHierarchy)
            {
                otherObject[i].SetActive(false);
            }
        }

        panelQuest = GameObject.Find("Panel - Quest");
        panelQuestState = GameObject.Find("Quest").GetComponent<PanelQuestState>();
        questA = panelQuest.transform.GetChild(3).GetComponent<Button>();
        questR = panelQuest.transform.GetChild(4).GetComponent<Button>();
        questT = panelQuest.transform.GetChild(1).GetComponent<Text>();
        questI = panelQuest.transform.GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemyKilled >= maxEnemyKilled)
        {
            playerInventory.currentXP += questXP;
            playerInventory.goldCoins += questGoldCoins;
            showGUI = false;
            panelQuestState.panelActive = false;
            GetComponent<Quest>().enabled = false;
        }

        for (int i = 0; i < enemyOrObject.Length; i++)
        {
            if (enemyOrObject[i].objective == null && enemyOrObject[i].isVisible)
            {
                currentEnemyKilled++;
                enemyOrObject[i].isVisible = false;
            }
        }

        if (showGUI)
        {
            panelQuest.GetComponent<RectTransform>().sizeDelta = new Vector2(widthPanel, heightPanel);

            questA.GetComponent<RectTransform>().sizeDelta = new Vector2(widthButton, heightButton);
            questA.transform.localPosition = new Vector3(posXButtonAccept, posYButton);

            questR.GetComponent<RectTransform>().sizeDelta = new Vector2(widthButton, heightButton);
            questR.transform.localPosition = new Vector3(posXButtonRefuse, posYButton);

            questT.rectTransform.sizeDelta = new Vector2(widthTitle, heightTitle);
            questT.text = questName;

            questI.rectTransform.sizeDelta = new Vector2(widthText, heightText);
            questI.text = questInstruction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            showGUI = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            showGUI = false;
            panelQuestState.panelActive = false;
        }
    }

    public void ShowGUI(bool show)
    {
        if (show)
        {
            for (int i = 0; i < enemyOrObject.Length; i++)
            {
                enemyOrObject[i].objective.SetActive(true);
                enemyOrObject[i].isVisible = true;
            }

            for (int i = 0; i < otherObject.Length; i++)
            {
                otherObject[i].SetActive(true);
            }
        }

        showGUI = false;
        panelQuestState.panelActive = false;
    }
}

[System.Serializable]
public struct QuestObjective
{
    public GameObject objective;
    [HideInInspector] public bool isVisible;
}