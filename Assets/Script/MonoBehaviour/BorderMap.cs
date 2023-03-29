using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderMap : MonoBehaviour
{
    public float directionExitX;
    public float directionExitZ;
    public int iDBorderMap;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 vectorPosition = other.transform.position;
            Vector3 vectorDirection = new Vector3(directionExitX, 0, directionExitZ);
            Vector3 vectorMovement = vectorPosition + vectorDirection;

            other.GetComponent<CharacterController>().enabled = false;

            other.transform.position = vectorMovement;
        }
    }
}
