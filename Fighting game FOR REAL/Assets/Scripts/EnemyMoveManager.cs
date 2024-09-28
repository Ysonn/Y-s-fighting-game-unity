using System.Collections;
using UnityEngine;


public class EnemyMoveManager : MonoBehaviour
{
    public Transform leftPlayer;      
    public Transform rightPlayer;      
    private float moveSpeed; 
    public float attackRange = 0.5f; // engagement distance 
    private bool isWalk1Active = false;

    private Coroutine toggleCoroutine;
    private Coroutine attackCoroutine;

    private int health = 3;

    public GameObject walk1;
    public GameObject walk2;

    public GameObject punch;
    public GameObject kick;

    public LeftPivotManager leftPivotManager;
    public RightPivotManager rightPivotManager;

    public EnemyState currentState;

    public static Transform attackingPlayer = null;

    public enum EnemyState
    {
        Walking,
        Attacking
    }

    void Start()
    {
        leftPlayer = GameObject.Find("LeftPivotPointParent").transform;
        rightPlayer = GameObject.Find("RightPivotPointParent").transform;

        // Get the LeftPivotManager and RightPivotManager components from the GameObjects
        leftPivotManager = leftPlayer.GetComponent<LeftPivotManager>();
        rightPivotManager = rightPlayer.GetComponent<RightPivotManager>(); 

        // Set moveSpeed to a random value between 1.5 and 2.5
        moveSpeed = Random.Range(1.5f, 2.5f);
    }

    void Update()
    {
        // Find the closest player
        Transform closestPlayer = FindClosestPlayer();

        if (closestPlayer != null)
        {
            // Calculate the distance to the closest player
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            
            // Move towards the closest player if not in attack range
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer(closestPlayer);
            }
            else if (currentState != EnemyState.Attacking)
            {
                // Start attacking if in range and not already attacking
                AttackPlayer(closestPlayer);
            }
            else if (attackingPlayer != closestPlayer)
            {
                // Reset if someone is closer
                StopAttacking();
                MoveTowardsPlayer(closestPlayer);
            }
        }
    }

    Transform FindClosestPlayer()
    {
        float distanceToLeftPlayer = Vector3.Distance(transform.position, leftPlayer.position);
        float distanceToRightPlayer = Vector3.Distance(transform.position, rightPlayer.position);

        // Return the closest player
        return distanceToLeftPlayer < distanceToRightPlayer ? leftPlayer : rightPlayer;
    }

    void MoveTowardsPlayer(Transform player)
    {
        // If currently attacking, stop the attack coroutine
        if (currentState == EnemyState.Attacking)
        {
            StopAttacking();
        }

        // Switch to Walking state
        currentState = EnemyState.Walking;

        // Find direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the enemy towards the specified player
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Start the walking animation coroutine if not already running
        if (toggleCoroutine == null)
        {
            toggleCoroutine = StartCoroutine(ToggleEnemyWalkAnim());
        }

        // Rotate to face the player based on x position
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
    }

    IEnumerator ToggleEnemyWalkAnim()
    {
        while (currentState == EnemyState.Walking)
        {
            walk1.SetActive(isWalk1Active);
            walk2.SetActive(!isWalk1Active);

            isWalk1Active = !isWalk1Active;

            float randomWalkDelay = Random.Range(0.25f, 0.6f);
            yield return new WaitForSeconds(randomWalkDelay);
        }

        // Reset the coroutine reference when done
        toggleCoroutine = null;
    }

    void AttackPlayer(Transform closestPlayer)
    {
        // Switch to Attacking state
        currentState = EnemyState.Attacking;

        // Set the attacking player to the closest player
        attackingPlayer = closestPlayer;

        // Increment the corresponding attacking counter
        if (attackingPlayer == leftPlayer)
        {
            leftPivotManager.enemiesAttackingLeft += 1;
        }
        else if (attackingPlayer == rightPlayer)
        {
            rightPivotManager.enemiesAttackingRight += 1;
        }

        // Stop the walking animation coroutine
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            toggleCoroutine = null;

            // Disable both walk animations
            walk1.SetActive(false);
            walk2.SetActive(false);
        }

        // Start the attack animation coroutine if not already running
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(ToggleAttackAnim());
        }

        // Attack logic here
        
        Debug.Log("Currently attacking: " + attackingPlayer.name);
    }

    IEnumerator ToggleAttackAnim()
    {
        bool isPunchActive = true;

        while (currentState == EnemyState.Attacking)
        {
            punch.SetActive(isPunchActive);
            kick.SetActive(!isPunchActive);

            isPunchActive = !isPunchActive;

            float randomAttackDelay = Random.Range(0.25f, 0.6f);
            yield return new WaitForSeconds(randomAttackDelay);
        }

        // Reset the coroutine reference when done
        attackCoroutine = null;
    }

    void StopAttacking()
    {
        // Stop the attack animation coroutine
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;

            // Disable both attack animations
            punch.SetActive(false);
            kick.SetActive(false);
        }

        if (attackingPlayer == leftPlayer)
        {
            leftPivotManager.enemiesAttackingLeft -= 1;
        }
        else if (attackingPlayer == rightPlayer)
        {
            rightPivotManager.enemiesAttackingRight -= 1;
        }

        // Reset the attacking player
        attackingPlayer = null;
    }

    // Handle collision with sphere collider
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Collided with: " + other.gameObject.name); // Log the name of the colliding object
    // Check if the collider is a sphere collider (e.g., a player's attack)
    if (other.CompareTag("Attack"))
    {
        TakeDamage();
    }
}

    void TakeDamage()
    {
        health--; // Reduce health by 1
        Debug.Log("Current Health: " + health);
        if (health <= 0)
        {
            StopAttacking();
            Destroy(gameObject); // Destroy the enemy when health reaches 0

        }
    }
}
