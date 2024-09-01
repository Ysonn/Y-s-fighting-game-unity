using System.Collections;
using System.Collections.Generic;
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

    private enum PlayerState
    {
        Idling,
        Walking,
        Punching,
        Blocking,
        Kicking
    }

    private PlayerState currentState;

    void Start()
    {
        // Initialize the state
        currentState = PlayerState.Idling;
        // Set initial visibility
        SetInitialState();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(0, 0, moveX) * speed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            ChangeState(PlayerState.Walking);
        }
        else if (Input.GetKey(KeyCode.Space)) // Example for punching
        {
            ChangeState(PlayerState.Punching);
        }
        else if (Input.GetKey(KeyCode.LeftShift)) // Example for blocking
        {
            ChangeState(PlayerState.Blocking);
        }
        else if (Input.GetKey(KeyCode.E)) // Example for kicking
        {
            ChangeState(PlayerState.Kicking);
        }
        else
        {
            ChangeState(PlayerState.Idling);
        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return; // No change needed if already in the desired state

        // Stop the current coroutine if any
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            toggleCoroutine = null;
        }

        // Update the current state
        currentState = newState;

        // Handle game object visibility and coroutines based on the new state
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
                HideAllObjects();
                StartCoroutine(PerformPunch());
                break;
            case PlayerState.Blocking:
                HideAllObjects();
                StartCoroutine(PerformBlock());
                break;
            case PlayerState.Kicking:
                HideAllObjects();
                StartCoroutine(PerformKick());
                break;
        }
    }

    private void SetInitialState()
    {
        // Start with idling state and hide walk game objects
        ShowIdlingObjects();
        HideWalkingObjects();
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

    IEnumerator PerformPunch()
    {
        punch.SetActive(true);
        yield return new WaitForSeconds(0.5f); // Assume punch animation duration is 0.5 seconds
        punch.SetActive(false);
        ChangeState(PlayerState.Idling); // Return to idle state after punch
    }

    IEnumerator PerformBlock()
    {
        block.SetActive(true);
        yield return new WaitForSeconds(1f); // Assume block duration is 1 second
        block.SetActive(false);
        ChangeState(PlayerState.Idling); // Return to idle state after blocking
    }

    IEnumerator PerformKick()
    {
        kick.SetActive(true);
        yield return new WaitForSeconds(0.7f); // Assume kick animation duration is 0.7 seconds
        kick.SetActive(false);
        ChangeState(PlayerState.Idling); // Return to idle state after kick
    }
}
