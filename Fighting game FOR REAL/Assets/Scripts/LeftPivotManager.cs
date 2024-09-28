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

    private Coroutine damageCoroutine;

    public int enemiesAttackingLeft;

    void Start()
    {
        // Initialize health text
        healthText.text = leftPlayerHealth.ToString();
    }

    void Update()
    {
        // Handle player movement
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face left  
            transform.Translate(speed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); // Face right 
            transform.Translate(speed, 0, 0);
        }

        // Start or stop the damage coroutine based on whether the enemies attacking
        

        if (enemiesAttackingLeft > 0 && damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(ApplyDamageOverTime(enemiesAttackingLeft));
        }
        else if (enemiesAttackingLeft == 0 && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        // Update health text
        healthText.text = leftPlayerHealth.ToString();

        // Handle player death
        if (leftPlayerHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ApplyDamageOverTime(int enemiesAttackingLeft)
    {
        while (true)
        {
            leftPlayerHealth -= enemiesAttackingLeft * 5;
            Debug.Log("Damage applied. Health: " + leftPlayerHealth);
            yield return new WaitForSeconds(1f);  // Apply damage every second
        }
    }

    void Die()
    {
        // Handle player death
        Debug.Log("Player died.");
    }
}
