using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNGAnimation : MonoBehaviour
{
    Animation animationsPNG;
    public string animationName;

    // Start is called before the first frame update
    void Start()
    {
        animationsPNG = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        animationsPNG.Play(animationName);
    }
}
