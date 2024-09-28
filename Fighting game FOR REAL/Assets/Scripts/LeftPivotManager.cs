using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeftPivotManager : MonoBehaviour
{
    public Vector3 playerPosition;
    private float speed = 0.004f;

    private int leftPlayerHealth = 100;

    public bool isWalking = false;

    public TextMeshProUGUI healthText;

    public GameObject loseText;

    private Coroutine damageCoroutine;

    public int enemiesAttackingLeft;

    void Start()
    {
        // Initialize health text
        healthText.text = leftPlayerHealth.ToString();
        loseText.SetActive(false);
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
        
        while (leftPlayerHealth > 0) // Continue while health is above 0
        {
            leftPlayerHealth -= enemiesAttackingLeft * 5;
            leftPlayerHealth = Mathf.Max(leftPlayerHealth, 0); // Clamp health to 0
            Debug.Log(enemiesAttackingLeft);
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
