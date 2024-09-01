using System.Collections;
using UnityEngine;

public class LeftPlayerManager : MonoBehaviour
{
    public GameObject walk1;
    public GameObject walk2;
    public GameObject idle1;
    public GameObject idle2;
    public GameObject punch;
    public GameObject block;
    public GameObject kick;

    private bool isWalk1Active = false;
    private bool isIdle1Active = true;
    public float speed = 2f;
    private Coroutine toggleCoroutine;

    private bool isOnCooldown = false;
    public float cooldownDuration = 0.01f;

    public enum PlayerState
    {
        Idling,
        Walking,
        Punching,
        Blocking,
        Kicking
    }

    public PlayerState currentState;

    void Start()
    {
        HideAllObjects();
        ShowIdlingObjects();
        currentState = PlayerState.Idling;
        SetInitialState();
    }

    void Update()
    {
        if (isOnCooldown) return; // Prevent input handling if in cooldown

        float moveX = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(0, 0, moveX) * speed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }

        // Check for actions
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlayerState.Punching);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeState(PlayerState.Blocking);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeState(PlayerState.Kicking);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            ChangeState(PlayerState.Walking);
        }
        else
        {
            ChangeState(PlayerState.Idling);
        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;

        // Stop the current coroutine if any
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            toggleCoroutine = null;
        }

        // Update the current state
        currentState = newState;

        // Handle game object visibility and coroutines based on the new state
        HideAllObjects(); // Hide all objects before showing new ones

        switch (newState)
        {
            case PlayerState.Walking:
                ShowWalkingObjects();
                toggleCoroutine = StartCoroutine(ToggleWalkAnim());
                break;
            case PlayerState.Idling:
                ShowIdlingObjects();
                toggleCoroutine = StartCoroutine(ToggleIdleAnim());
                break;
            case PlayerState.Punching:
                punch.SetActive(true);
                StartCoroutine(PerformActionWithCooldown(PlayerState.Punching));
                break;
            case PlayerState.Blocking:
                block.SetActive(true);
                StartCoroutine(PerformActionWithCooldown(PlayerState.Blocking));
                break;
            case PlayerState.Kicking:
                kick.SetActive(true);
                StartCoroutine(PerformActionWithCooldown(PlayerState.Kicking));
                break;
        }
    }

    private void SetInitialState()
    {
        ShowIdlingObjects();
    }

    private void ShowWalkingObjects()
    {
        walk1.SetActive(true);
        walk2.SetActive(true);
        idle1.SetActive(false);
        idle2.SetActive(false);
    }

    private void HideWalkingObjects()
    {
        walk1.SetActive(false);
        walk2.SetActive(false);
    }

    private void ShowIdlingObjects()
    {
        idle1.SetActive(true);
        idle2.SetActive(true);
        walk1.SetActive(false);
        walk2.SetActive(false);
    }

    private void HideAllObjects()
    {
        walk1.SetActive(false);
        walk2.SetActive(false);
        idle1.SetActive(false);
        idle2.SetActive(false);
        punch.SetActive(false);
        block.SetActive(false);
        kick.SetActive(false);
    }

    IEnumerator ToggleWalkAnim()
    {
        while (currentState == PlayerState.Walking)
        {
            walk1.SetActive(isWalk1Active);
            walk2.SetActive(!isWalk1Active);

            isWalk1Active = !isWalk1Active;

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator ToggleIdleAnim()
    {
        while (currentState == PlayerState.Idling)
        {
            idle1.SetActive(isIdle1Active);
            idle2.SetActive(!isIdle1Active);

            isIdle1Active = !isIdle1Active;

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator PerformActionWithCooldown(PlayerState actionState)
    {
        // Disable movement
        isOnCooldown = true;

        // Perform action
        yield return new WaitForSeconds(GetActionDuration(actionState));
        // Optionally, hide the action object if needed here
        // punch.SetActive(false); // This is not needed since it stays visible during cooldown

        // Handle cooldown
        yield return Cooldown();

        // Return to idle state or walking state based on movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            ChangeState(PlayerState.Walking);
        }
        else
        {
            ChangeState(PlayerState.Idling);
        }

        // Re-enable movement
        isOnCooldown = false;
    }

    private float GetActionDuration(PlayerState actionState)
    {
        switch (actionState)
        {
            case PlayerState.Punching:
                return 0.01f;
            case PlayerState.Blocking:
                return 0.2f;
            case PlayerState.Kicking:
                return 0.01f;
            default:
                return 0f;
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
    }
}