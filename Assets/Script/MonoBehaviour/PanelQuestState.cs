using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelQuestState : MonoBehaviour
{
    public Quest[] panelState;
    GameObject panelQuest;
    [HideInInspector] public bool panelActive = false;
    int questID;

    private void Start()
    {
        panelQuest = GameObject.Find("Panel - Quest");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < panelState.Length; i++)
        {
            if (panelState[i].enabled)
            {
                if (panelState[i].showGUI)
                {
                    panelQuest.SetActive(true);
                    panelActive = true;
                    questID = i;
                    break;
                }
                else if (!panelActive)
                    panelQuest.SetActive(false);
            }
        }
    }

    public void ColliderQuest(bool answer)
    {
        panelState[questID].ShowGUI(answer);
    }
}
