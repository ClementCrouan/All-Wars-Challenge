using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressState : MonoBehaviour
{
    Text progressText;
    Image progressImage;

    // Start is called before the first frame update
    void Start()
    {
        progressText = gameObject.GetComponentInChildren<Text>();
        progressImage = gameObject.GetComponentInChildren<Image>();
        progressImage.color = new Vector4(progressImage.color.r, progressImage.color.b, progressImage.color.g, 0);
        progressText.color = new Vector4(progressText.color.r, progressText.color.b, progressText.color.g, 0);
    }

    public void ProgressAnimation(string name, float timeStay)
    {
        progressText.text = name;
        progressImage.color = new Vector4(progressImage.color.r, progressImage.color.b, progressImage.color.g, 255);
        progressText.color = new Vector4(progressText.color.r, progressText.color.b, progressText.color.g, 255);
        StartCoroutine(AnimationTime(timeStay));
    }

    public IEnumerator AnimationTime(float timeStay)
    {
        yield return new WaitForSeconds(timeStay);
        progressImage.color = new Vector4(progressImage.color.r, progressImage.color.b, progressImage.color.g, 0);
        progressText.color = new Vector4(progressText.color.r, progressText.color.b, progressText.color.g, 0);
    }
}
