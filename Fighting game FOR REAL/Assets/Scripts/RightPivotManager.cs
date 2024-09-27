using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPivotManager : MonoBehaviour
{
    public Vector3 playerPosition; 
    private float speed = 0.004f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.J))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face left  
            transform.Translate(speed, 0, 0); // Move left
        }
    
        else if (Input.GetKey(KeyCode.L))
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); // Face right 
            transform.Translate(speed, 0, 0); // Move right
        }  
        
    }
}
