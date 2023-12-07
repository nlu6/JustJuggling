/*
 * Contributors: Ian Dennis
 * Last Modified: Oct. 24th, 2023
 * 
 * Purpose: Script monitors the hands of the player and looks for
 *          both a player input and a juggling object input occurring
 *          at the same time. If both are present and input is correct
 *          it will update the physics of the juggling object to it's
 *          next throw trajectory. Script asks {MUSIC SYNC SCRIPT SOMEWHERE}
 *          how long to put the object in the air for.
 * 
 * Binds With:  Juggling Hands Objects
 * Modifies:    Juggling Object Physics,
 *              Player Score Value,
 *              etc.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Numerics;

public class HandleCatch : MonoBehaviour
{
    // public variables
    [Header("Scoring")]
    [Tooltip("Flag for if object has been thrown (used to prevent catching object and getting bonus input at same time)")]
    public bool objectNearHand = false;

    [Header("Objects")]

    [Tooltip("Player hand attached to this script")]
    public GameObject playerHand; 
    [Tooltip("List of all trigger objects in scene (found at runtime)")]
    public GameObject[] proximityObjects; 
    [Tooltip("Most recent collided object")]
    public List<GameObject> collidedObjects;
    [Tooltip("List of all juggling objects in scene (found at runtime)")]
    public GameObject[] jugglingObjects;
    public bool foundJugglingObjects = false;

    [Header("Physics")]
    [Tooltip("Max distance between hand and object for catch to register")]
    public double maxCatchDistance = 1.5;
    public double interceptHeight = 3.5;

    public int leftHandX = 1;
    public int rightHandX = -1;
    public double xDeviation = 0.25;

    [Header("Inputs")]
    // private variables
    [Tooltip("Input expected from juggling object")]
    string expectedInput = ""; 
    [Tooltip("Actual input from player")]
    string playerInput = ""; 

    // Update is called once per frame
    void Update()
    {
        if( foundJugglingObjects )
        {
            // Once there are objects on screen to 
            if( Input.anyKey )
            {
                string tempInput = Input.inputString.ToUpper();
                if( tempInput != ""  && tempInput != playerInput )
                {
                    playerInput = tempInput;
                    
                    Debug.Log("Detected input: " + playerInput);

                    CheckForObjectThrow();                }
            }
        }
        else
        {
            FindJugglingObjects();
        }
    }

    void CheckForObjectThrow()
    {
        // Throwing object
        // ===============
        // check for collision with juggling object
        for( int objectIndex = 0; objectIndex < jugglingObjects.Length; objectIndex++ )
        {
            // get juggling object
            GameObject jugglingObject = jugglingObjects[objectIndex];

            // if the object is going upwards ignore it
            if( jugglingObject.GetComponent<jugglingObject>().downwardTrajectory == false )
            {
                continue;
            }

            // otherwise check for collision with juggling object
            GameObject proximitySensor = proximityObjects[objectIndex];

            // check for trigger collision with juggling object
            if( objectNearHand && collidedObjects.Contains(proximitySensor) )
            {
                // get input juggling oject is expecting
                expectedInput = jugglingObject.GetComponent<JugglingObject>().expectedInput;
                Debug.Log("Expected input: " + expectedInput);

                // check if player input matches expected input
                if( playerInput == expectedInput)
                {
                    Debug.Log("Correct input: " + playerInput);
                    // get position of this hand
                    UnityEngine.Vector3 handPos = playerHand.transform.position;

                    // get position of juggling object
                    UnityEngine.Vector3 objectPos = jugglingObject.transform.position;

                    // update player score based on position of objects
                    // function: updateScore
                    UpdateScore( handPos, objectPos, false );

                    // reposistion juggling object to above hand (0.45 is ball radius plus hand radius)
                    jugglingObject.transform.position = new UnityEngine.Vector3(handPos.x, handPos.y + 1.5f, handPos.z);

                    // call for new input to throw object
                    jugglingObject.GetComponent<JugglingObject>().UpdateInput();
                }
                // otherwise subtract score to punish player
                else
                {
                    UpdateScore( playerHand.transform.position, jugglingObject.transform.position, true );
                }

        
                // reset player input
                playerInput = "";
            }
            // if no collision with juggling object do nothing
        }
    }

    void FindJugglingObjects()
    {
        // get expected number of objects
        int expectedObjects = Camera.main.GetComponent<StartUp>().numObjects;

        // get juggling objects
        jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        // get proximity sensors
        proximityObjects = GameObject.FindGameObjectsWithTag("Proximity");

        // check if all objects have been found
        if( jugglingObjects.Length == expectedObjects && proximityObjects.Length == expectedObjects )
        {
            // set flag
            foundJugglingObjects = true;

            Debug.Log("Found all objects");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // check if object is juggling object
        if( other.gameObject.tag == "Proximity" )
        {
            // Debug.Log("Object near hand");

            // lock out bonus input if object is near hand
            objectNearHand = true;
            collidedObjects.Add(other.gameObject);
            // Debug.Log("Collided Objects List: " + collidedObjects.Count);
        }
        else
        {
            // Debug.LogWarning("Object near hand but not juggling object");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // check if object is juggling object
        if( other.gameObject.tag == "Proximity" )
        {
            //Debug.Log("Object no longer near hand");

            // allow bonus input if object is no longer near hand
            objectNearHand = false;
            collidedObjects.Remove(other.gameObject);
            //Debug.Log("Collided Objects List: " + collidedObjects.Count);
        }
        else
        {
            //Debug.LogWarning("Object no longer near hand but not juggling object");
        }
    }

    // Update player score
    void UpdateScore( UnityEngine.Vector3 handPos = default, UnityEngine.Vector3 objectPos = default, bool wrongInput = false )
    {
        // get distance between hand and object
        int distance = (int)UnityEngine.Vector3.Distance(handPos, objectPos);
        float scoreMult = (float)maxCatchDistance - distance;

        // if input is wrong subtract score instead of adding
        if( wrongInput )
        {
            scoreMult *= -1;
        }

        // update score based on distance
        JustJugglingMain.OBJ_HIT(Math.Abs(scoreMult));
    }

    // End game
    void EndGame()
    {
        JustJugglingMain.GAME_END();
    }
}
