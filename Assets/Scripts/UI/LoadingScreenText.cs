using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreenText : MonoBehaviour
{
    public float timePassed = 0f;
    public int numberOfPeriods = 3;
    public int maxNumberOfPeriods = 3;
    public float lengthOfWait = 0.5f;
    public TextMeshProUGUI textElement;

    void Start()
    {
        textElement = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        textElement.text = string.Concat(textElement.text, new string(".".ToCharArray()[0], numberOfPeriods));
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= lengthOfWait)
        {
            numberOfPeriods %= maxNumberOfPeriods;
            numberOfPeriods++;
            textElement.text = string.Concat(textElement.text.TrimEnd(".".ToCharArray()[0]), new string(".".ToCharArray()[0], numberOfPeriods));
            timePassed = 0f;
        }
    }
}
