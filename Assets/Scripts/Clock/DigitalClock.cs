using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DigitalClock : MonoBehaviour
{
    TimeManager timeManager;
    TextMeshProUGUI display;

    public bool _24HourClock = true;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        display = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_24HourClock)
        {
            display.text = timeManager.Clock24Hour();
        }
        else
        {
            display.text = timeManager.Clock12Hour();
        }
        
    }
}