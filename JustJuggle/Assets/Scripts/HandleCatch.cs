/*
 * Contributors: Ian Dennis
 * Last Modified: December 7th, 2023
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
    [Header("Objects")]

    [Tooltip("Player hand attached to this script")]
    public GameObject playerHand; 
    [Tooltip("Current juggling object colliding with hand")]
    public List<GameObject> collidedObjects;
    [Tooltip("List of all juggling objects in scene (found at runtime)")]
    public GameObject[] jugglingObjects;
    public bool foundJugglingObjects = false;

    [Header("Physics")]
    [Tooltip("Max distance between hand and object for catch to register")]
    public double maxCatchDistance = 1.75;
    public double interceptHeight = 3.5;

    public int leftHandX = 1;
    public int rightHandX = -1;
    private int thisHandX = 0;
    public double xDeviation = 0.25;

    [Header("Inputs")]
    // private variables
    [Tooltip("Input expected from juggling object")]
    string expectedInput = ""; 
    [Tooltip("Actual input from player")]
    string playerInput = ""; 

    // Start is called before the first frame update
    void Start()
    {
        // get this hand's x position
        if( playerHand.transform.position.x > 0 )
        {
            thisHandX = leftHandX;
        }
        else
        {
            thisHandX = rightHandX;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if( foundJugglingObjects )
        {
            // Once there are objects on screen to 
            if( Input.anyKey )
            {
                string tempInput = Input.inputString.ToUpper();
                if( tempInput != "" )
                {
                    playerInput = tempInput;

                    CheckForObjectThrow();
                }
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
        foreach( GameObject jugglingObject in jugglingObjects )
        {
            // if the object is going upwards ignore it for speed
            if( jugglingObject.GetComponent<JugglingObject>().downwardTrajectory == false )
            {
                continue;
            }

            // check for trigger collision with juggling object
            if( collidedObjects.Contains(jugglingObject) )
            {
                // get input juggling oject is expecting
                expectedInput = jugglingObject.GetComponent<JugglingObject>().expectedInput;

                // check if player input matches expected input
                if( playerInput == expectedInput)
                {
                    // get position of juggling object
                    UnityEngine.Vector3 objectPos = jugglingObject.transform.position;

                    // get position of this hand
                    UnityEngine.Vector3 handPos = playerHand.transform.position;

                    // reset object to hand position
                    jugglingObject.GetComponent<JugglingObject>().ResetThrow( handPos );

                    // call for new input to throw object
                    jugglingObject.GetComponent<JugglingObject>().UpdateInput();

                    // update player score based on position of objects
                    // function: updateScore
                    UpdateScore( handPos, objectPos, false );
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

        // check if all objects have been found
        if( jugglingObjects.Length == expectedObjects )
        {
            // set flag
            foundJugglingObjects = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // check if object is juggling object going to this hand
        if( other.gameObject.tag == "Proximity" &&
            other.gameObject.transform.parent.gameObject.GetComponent<JugglingObject>().destinationHand == thisHandX )
        {
            collidedObjects.Add(other.gameObject.transform.parent.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // remove object from collided objects list
        collidedObjects.Remove(other.gameObject.transform.parent.gameObject);
    }

    // Update player score
    void UpdateScore( UnityEngine.Vector3 handPos, UnityEngine.Vector3 objectPos, bool wrongInput )
    {
        // get distance between hand and object
        float distance = (float)UnityEngine.Vector3.Distance(handPos, objectPos);
        float scoreMult = (float)Math.Abs(maxCatchDistance - distance);

        // if input is wrong subtract score instead of adding
        if( wrongInput )
        {
            scoreMult = -scoreMult;
        }

        // update score based on distance
        JustJugglingMain.OBJ_HIT(scoreMult);
    }

    // End game
    void EndGame()
    {
        JustJugglingMain.GAME_END();
    }
}
