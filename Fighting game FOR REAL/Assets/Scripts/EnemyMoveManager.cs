using System.Collections;
using UnityEngine;

public class EnemyMoveManager : MonoBehaviour
{
    public Transform leftPlayer;      
    public Transform rightPlayer;      
    public float moveSpeed = 2f;   
    public float attackRange = 0.5f; // engagement distance 
    private bool isWalk1Active = false;

    private Coroutine toggleCoroutine;
    private Coroutine attackCoroutine;

    public GameObject walk1;
    public GameObject walk2;

    public GameObject punch;
    public GameObject kick;

    public static int amountAttackingLeft = 0;
    public static int amountAttackingRight = 0;

    public EnemyState currentState;

    public static Transform attackingPlayer = null;

    public enum EnemyState
    {
        Walking,
        Attacking
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
                //reset if someone is closer
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

            yield return new WaitForSeconds(0.4f);
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
            amountAttackingLeft++;
        }
        else if (attackingPlayer == rightPlayer)
        {
            amountAttackingRight++;
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

            yield return new WaitForSeconds(0.3f);
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
            amountAttackingLeft--;
        }
        else if (attackingPlayer == rightPlayer)
        {
            amountAttackingRight--;
        }


        // Reset the attacking player
        attackingPlayer = null;
    }
}
