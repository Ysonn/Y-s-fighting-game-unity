using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI SurvivalTimer; 
    public float timeSurvived;
    public bool TimeCounting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeCounting == true)
        {
            timeSurvived = TimeSurvived + Time.deltaTime; 
            SurvivalTimer.text = timeSurvived;
        }
    }
}
