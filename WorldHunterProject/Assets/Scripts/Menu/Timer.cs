using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerText2;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDowm;

    [Header("Limit Settings")]
    public bool stop=false;

    private void Start()
    {
        currentTime=PlayerPrefs.GetFloat("currentTime");
    }

    private void Update()
    {
        if (!stop)
        {
            currentTime = countDowm? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
            timerText.text = currentTime.ToString("0.00");
            timerText2.text = currentTime.ToString();
        }
    }
}
