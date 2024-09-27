using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeftPivotManager : MonoBehaviour
{
    public Vector3 playerPosition; 
    private float speed = 0.004f;

    private int leftPlayerHealth = 100;

    

    public bool isWalking = false;

    public TextMeshProUGUI healthText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A) && (isWalking = true))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face left  
            transform.Translate (speed,0,0);
        }
        else if (Input.GetKey(KeyCode.D) && (isWalking = true))
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); // Face right 
            transform.Translate (speed,0,0); 
        }

        int enemiesAttackingLeft = EnemyMoveManager.amountAttackingLeft;

        leftPlayerHealth -= (int)(enemiesAttackingLeft * Time.deltaTime);
    
        healthText.text = Mathf.Clamp(leftPlayerHealth, 0,1000).ToString();
        Debug.Log("Enemies attacking left: " + enemiesAttackingLeft);
        Debug.Log("Current Health After: " + leftPlayerHealth);
    }

    

    void Die()
    {
        // Handle player death
        Debug.Log("Player died.");
    }
        
}
