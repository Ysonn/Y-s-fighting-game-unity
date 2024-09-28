using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHedManager : MonoBehaviour
{
    public GameObject head1;  
    public GameObject head2;  

    void Update()
    {
        // Check the value of the static selectedCharacter variable from da other script 
        if (LeftSelectManager.selectedCharacter == 1)
        {
            head1.SetActive(true);
            head2.SetActive(false);
        }
        else if (LeftSelectManager.selectedCharacter == 2)
        {
            head1.SetActive(false);
            head2.SetActive(true);
        }
    }
}
