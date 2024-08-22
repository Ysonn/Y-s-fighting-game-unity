using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TimeManager : MonoBehaviour
{
    
    public TextMeshProUGUI SurvivalTimer; 
    public float timeSurvived;
    public float timeSurvivedMinute;
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
            timeSurvived += Time.deltaTime; 
            int minutes = Mathf.FloorToInt(timeSurvived / 60);
            int seconds = Mathf.FloorToInt(timeSurvived % 60);
            SurvivalTimer.text = string.Format("TIME: {0}:{1:00}", minutes, seconds);
        }
    }
}
