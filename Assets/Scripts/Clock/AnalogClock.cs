using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    TimeManager timeManager;

    public RectTransform minuteHand;
    public RectTransform hourHand;

    const float hoursToDegrees = 360 / 12, minutesToDegrees = 360 / 60;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    void Update()
    {
        hourHand.rotation = Quaternion.Euler(0, 0, -timeManager.GetHour() * hoursToDegrees);
        minuteHand.rotation = Quaternion.Euler(0, 0, -timeManager.GetMinutes() * minutesToDegrees);
    }
}