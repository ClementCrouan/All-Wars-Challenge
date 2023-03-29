using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    EnemyAI enemyAI;

    public GameObject healthBarUI;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = gameObject.GetComponent<EnemyAI>();
        healthBarUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyAI.enemyHealth / enemyAI.maxHealth; 

        if (enemyAI.enemyHealth < enemyAI.maxHealth)
        {
            healthBarUI.SetActive(true);
        }
    }
}
