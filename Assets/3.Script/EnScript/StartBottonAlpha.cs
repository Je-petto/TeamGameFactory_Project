using System.Collections;
using TMPro;
using UnityEngine;

public class StartBottonAlpha : MonoBehaviour
{

    public TextMeshProUGUI text;
    
    void Start()
    {
        StartCoroutine(TextAlpha());
    }

    IEnumerator TextAlpha()
    {
        while(true)
        {
            text.alpha = 0;
            yield return new WaitForSeconds(0.5f);
            text.alpha = 255;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
