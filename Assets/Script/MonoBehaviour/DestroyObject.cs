using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float health = 100;

    public Transform loot;
    public Vector3 appearPos;

    public void ApplyDamage(float theDamage)
    {
        health -= theDamage;

        if (health <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        Instantiate(loot, (appearPos), Quaternion.identity);
        Destroy(gameObject);
    }
}
