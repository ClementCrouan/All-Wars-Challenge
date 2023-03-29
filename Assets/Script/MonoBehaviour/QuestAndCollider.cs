using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAndCollider : MonoBehaviour
{
    public QuestCollider[] questCollider;

    private void Start()
    {
        for (int i = 0; i < questCollider.Length; i++)
        {
            questCollider[i].borderMap = questCollider[i].collider.GetComponent<BorderMap>();
            questCollider[i].quest = questCollider[i].questCollider.GetComponent<Quest>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < questCollider.Length; i++)
        {
            if (!questCollider[i].quest.enabled)
            {
                questCollider[i].collider.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public struct QuestCollider
{
    public GameObject collider;
    public GameObject questCollider;
    [HideInInspector] public Quest quest;
    [HideInInspector] public BorderMap borderMap;
}