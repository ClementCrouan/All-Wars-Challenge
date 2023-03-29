using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptableObject : MonoBehaviour
{
    public ColliderQuest[] colliderQuest;
}

[System.Serializable]
public struct ColliderQuest
{
    public GameObject collider;
    public GameObject quest;
}