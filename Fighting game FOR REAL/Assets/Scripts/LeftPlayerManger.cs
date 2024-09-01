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

    
    private List<PlayerState> playerStates;
    private PlayerState currentState;

    void Start()
    {
        
        playerStates = new List<PlayerState>
        {
            PlayerState.Idling,
            PlayerState.Walking,
            PlayerState.Punching,
            PlayerState.Blocking,
            PlayerState.Kicking
        };
        currentState = PlayerState.Idling; 
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
        else if (Input.GetKey(KeyCode.Space)) 
        {
            ChangeState(PlayerState.Punching);
        }
        else if (Input.GetKey(KeyCode.LeftShift)) 
        {
            ChangeState(PlayerState.Blocking);
        }
        else if (Input.GetKey(KeyCode.E)) 
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
        if (currentState == newState) return; 

        // Stop the current coroutine if any
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            toggleCoroutine = null;
        }

        currentState = newState;

        // Start a new coroutine based on the new state
        switch (newState)
        {
            case PlayerState.Walking:
                toggleCoroutine = StartCoroutine(ToggleWalkAnim());
                break;
            case PlayerState.Idling:
                toggleCoroutine = StartCoroutine(ToggleIdleAnim());
                break;
            case PlayerState.Punching:
                StartCoroutine(PerformPunch());
                break;
            case PlayerState.Blocking:
                StartCoroutine(PerformBlock());
                break;
            case PlayerState.Kicking:
                StartCoroutine(PerformKick());
                break;
        }
    }

    IEnumerator ToggleWalkAnim()
    {
        while (true)
        {
            walk1.SetActive(isWalk1Active);
            walk2.SetActive(!isWalk1Active);
            
            isWalk1Active = !isWalk1Active;

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator ToggleIdleAnim()
    {
        while (true)
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
