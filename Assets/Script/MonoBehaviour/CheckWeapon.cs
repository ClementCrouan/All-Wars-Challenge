using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWeapon : MonoBehaviour
{

    private int weaponID;

    public GameObject bodyPart;

    [SerializeField]
    public List<GameObject> weaponList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 0)
        {
            weaponID = gameObject.GetComponentInChildren<ItemOnObject>().item.itemID;          
        }
        else
        {
            weaponID = 0;

            for (int i = 0; i < weaponList.Count; i++)
            {
                weaponList[i].SetActive(false);
            }
        }

        if (bodyPart.transform.childCount > 0)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                weaponList[i].SetActive(false);
            }
        }

        //Iron Sword
        if(weaponID == 1 && transform.childCount > 0)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (i == 0)
                {
                    weaponList[i].SetActive(true);
                }
            }
        }

        //Gold Sword
        if (weaponID == 2 && transform.childCount > 0)
        {
            for (int i = 1; i < weaponList.Count; i++)
            {
                if (i == 1)
                {
                    weaponList[i].SetActive(true);
                }
            }
        }

        //Leather Helmet
        if (weaponID == 3 && transform.childCount > 0)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (i == 0)
                {
                    weaponList[i].SetActive(true);
                }
            }
        }

        //Magic Sword
        if (weaponID == 4 && transform.childCount > 0)
        {
            for (int i = 2; i < weaponList.Count; i++)
            {
                if (i == 2)
                {
                    weaponList[i].SetActive(true);
                }
            }
        }

        //Iron Sword
        if (weaponID == 5 && transform.childCount > 0)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (i == 0)
                {
                    weaponList[i].SetActive(true);
                }
            }
        }

        //Iron Large Sword
        if (weaponID == 6 && transform.childCount > 0)
        {
            for (int i = 3; i < weaponList.Count; i++)
            {
                if (i == 3)
                {
                    weaponList[i].SetActive(true);
                }
            }
        }
    }
}