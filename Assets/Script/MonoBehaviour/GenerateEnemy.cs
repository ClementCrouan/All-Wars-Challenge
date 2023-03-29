using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemy : MonoBehaviour
{
    public GameObject[] theEnemy;
    [HideInInspector] public int enemyCount;
    public Vector2 posMax = new Vector2(250, 250);
    public Vector2 posMin = new Vector2(0, 0);
    public float yPos = 10f;
    public int numberMaxEnemy = 5;
    bool isCoroutineEnd;

    void Start()
    {
        StartCoroutine(EnemyDrop());
        enemyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCoroutineEnd)
            StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        isCoroutineEnd = false;
        while (enemyCount < numberMaxEnemy)
        {
            float xPos = Random.Range(posMin.x, posMax.x);
            float zPos = Random.Range(posMin.y, posMax.y);
            int randomNumber = Random.Range(0, theEnemy.Length);
            GameObject finalEnemy = theEnemy[randomNumber];
            Instantiate(finalEnemy, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount++;
        }
        isCoroutineEnd = true;
    }
}
