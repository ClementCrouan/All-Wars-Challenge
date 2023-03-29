using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellCollision : MonoBehaviour
{
    public float spellDamage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" | col.gameObject.tag == "EnemyQuest")
        {
            col.gameObject.GetComponent<EnemyAI>().ApplyDamage(spellDamage);
        }

        if (col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
