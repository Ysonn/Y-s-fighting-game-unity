using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RightPivotManager : MonoBehaviour
{
    public Vector3 playerPosition;
    private float speed = 0.004f;

    private int rightPlayerHealth = 100;

    public bool isWalking = false;

    public TextMeshProUGUI healthText;

    public GameObject loseText;

    public int enemiesAttackingRight;

    private Coroutine damageCoroutine;

    void Start()
    {
        // Initialize health text
        healthText.text = rightPlayerHealth.ToString();
        loseText.SetActive(false);

        if (RightSelectManager.selectedCharacter == 1)
        {
            speed = 0.006f;
        }

        if (RightSelectManager.selectedCharacter == 2)
        {
            rightPlayerHealth = 110;
        }
        
        
    }

    void Update()
    {
        // Handle player movement
        if (Input.GetKey(KeyCode.J))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face left  
            transform.Translate(speed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); // Face right 
            transform.Translate(speed, 0, 0);
        }

        // Start or stop the damage coroutine based on whether the enemies attacking
        

        if (enemiesAttackingRight > 0 && damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(ApplyDamageOverTime(enemiesAttackingRight));
        }
        else if (enemiesAttackingRight == 0 && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        // Update health text
        healthText.text = rightPlayerHealth.ToString();

        // Handle player death
        if (rightPlayerHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ApplyDamageOverTime(int enemiesAttackingRight)
    {
        while (rightPlayerHealth > 0) // Continue while health is above 0
        {
            rightPlayerHealth -= enemiesAttackingRight * 5;
            rightPlayerHealth = Mathf.Max(rightPlayerHealth, 0); // Clamp health to 0
            Debug.Log(enemiesAttackingRight);
            yield return new WaitForSeconds(1f);  // Apply damage every second
        }
    }

    void Die()
    {
        // Handle player death
        Debug.Log("Player died.");
        // Pause the game
        Time.timeScale = 0;

        // Start the coroutine to wait and load the MainMenu scene
        StartCoroutine(WaitAndLoadMainMenu());
        
    }
    private IEnumerator WaitAndLoadMainMenu()
    {
        loseText.SetActive(true);
        // Wait for 3 seconds
        yield return new WaitForSecondsRealtime(3f); // Use WaitForSecondsRealtime to ignore time scale

        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }
}
