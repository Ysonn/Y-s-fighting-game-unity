using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingGameCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform[] characterTransforms;

    private void Start()
    {
        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Character");
        characterTransforms = new Transform[allCharacters.Length];
        for(int i = 0; i < allCharacters.Length; i++)
        {
            characterTransforms[i] = allCharacters[i].transform;  
        }
    }

    public float yOffset = 2f;
    public float minDistance = 7.5f;

    private float xMin,xMax,yMin,yMax;

    private void LateUpdate() //late update so camera tracking follows object movement, otherwise found stutter. 
    {
        if(characterTransforms.Length == 0)    
        {
            Debug.Log("Have not found a anyone, make sure character tag on");
            return; 
        }
    

        xMin = xMax = characterTransforms[0].position.x;
        yMin = yMax = characterTransforms[0].position.x;
        for(int i = 1; i < characterTransforms.Length; i++)
        {
            if(characterTransforms[i].position.x < xMin)
                xMin = characterTransforms[i].position.x;

            if(characterTransforms[i].position.x > xMax)
                xMax = characterTransforms[i].position.x;

            if(characterTransforms[i].position.y < yMin)
                yMin = characterTransforms[i].position.y;

            if(characterTransforms[i].position.y > yMax)
                yMax = characterTransforms[i].position.y;
        }

        float xMiddle = (xMin + xMax) / 2;
        float yMiddle = (yMin + yMax) / 2;
        float distance = xMax - xMin;
        if(distance < minDistance)
            distance = minDistance;
        
            transform.position = new Vector3(xMiddle, 14, +distance + 20);

    }
}   
