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
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HandleCatch : MonoBehaviour
{
    // public variables
    [Header("Scoring")]

    [Tooltip("Player score")]
    int framesSinceBonus = 0; 
    [Tooltip("Flag for if object has been thrown (used to prevent catching object and getting bonus input at same time)")]
    bool objectThrown = false;

    [Header("Objects")]

    [Tooltip("Player hand attached to this script")]
    public GameObject playerHand; 
    [Tooltip("List of all juggling objects in scene (found at runtime)")]
    public GameObject[] jugglingObjects; 
    [Tooltip("Bonus object (i.e. extra popup inputs for player) also found at runtime")]
    public TextMeshPro bonusObject; 

    [Header("Physics")]
    [Tooltip("Max distance between hand and object for catch to register")]
    public int maxCatchDistance = 0;
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

    void Start()
    {
        // get list of all juggling objects in scene
        jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Throwing object
        // ===============
        // reset object thrown flag
        objectThrown = false;

        // check for collision with juggling object
        foreach( GameObject jugglingObject in jugglingObjects )
        {
            // check for collision, also check if object has been thrown
            
            if( jugglingObject.GetComponent<Collider>().bounds.Intersects(playerHand.GetComponent<Collider>().bounds)
                && jugglingObject.GetComponent<Rigidbody>().velocity != Vector3.zero )
            {
                // if collision with juggling object check for player input
                // this ensure inputs will first go to the juggling object
                if( Input.anyKeyDown )
                {
                    // get player input
                    playerInput = Input.inputString;

                    // get input juggling oject is expecting
                    // TODO: get input from juggling object script
                    expectedInput = "w";

                    // check if player input matches expected input
                    if( playerInput == expectedInput )
                    {
                        // trigger object thrown flag
                        objectThrown = true;

                        // get position of this hand
                        Vector3 handPos = playerHand.transform.position;

                        // get position of juggling object
                        Vector3 objectPos = jugglingObject.transform.position;

                        // update player score based on position of objects
                        // function: updateScore
                        UpdateScore( -1, handPos, objectPos );

                        // reposistion juggling object to above hand
                        jugglingObject.transform.position = new Vector3(handPos.x, handPos.y + 1, handPos.z);

                        // reset juggling object velocity
                        jugglingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                        // reset juggling object angular velocity
                        jugglingObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                        // call throw object script
                        ThrowObject(jugglingObject);
                    }
                    // otherwise player input does not match expected input
                    else
                    {
                        // load game over scene
                        EndGame();
                    }
            
                // reset player input
                playerInput = "";
                }
             }

            // if no collision with juggling object do nothing
        }
        
        // Bonus Catch
        // ===========
        // check if bonus input is available
        bonusObject = IsBonusAvailable();

        if( bonusObject != null )
        {
            // update frames since bonus input became available
            framesSinceBonus++;

            // if bonus input is available check for player input
            if( Input.anyKeyDown && !objectThrown )
            {
                // get player input
                playerInput = Input.inputString;

                // get expected bonus input (text value of TMP ojbect)
                expectedInput = bonusObject.GetComponent<TextMeshPro>().text;

                // check if player input matches expected input
                if( playerInput == expectedInput )
                {
                    // update player score based on bonus
                    // function: updateScore
                    UpdateScore( framesSinceBonus );

                    // delete bonus object
                    Destroy(bonusObject);
                }
            }
        }
        // otherwise bonus input is not available
        else
        {
            // reset frames since bonus input became available
            framesSinceBonus = 0;
        }
    }

    // Update player score
    void UpdateScore( int framesSinceBonus = -1, Vector3 handPos = default, Vector3 objectPos = default )
    {
        // if all values are default values, or if all values are not default values log warning, skip score update
        if( (framesSinceBonus == -1 && handPos == default && objectPos == default) 
            || (framesSinceBonus != -1 && (handPos != default|| objectPos != default))
            || (handPos != default && objectPos == default) || (handPos == default && objectPos != default) )
        {
            Debug.LogWarning("updateScore called with Bonus frames: "+ framesSinceBonus + "\nHand: " + handPos +"\nObject: "+ objectPos
                    + "\nExpected either bonus frames or cordinates to be default values (not both), no score update will occur.");
        }

        // else if bonus frames is -1 update based on normal scoring
        else if( framesSinceBonus == -1 )
        {
            // get distance between hand and object
            int distance = (int)Vector3.Distance(handPos, objectPos);
            int scoreMult = maxCatchDistance - distance;

            JustJugglingMain.OBJ_HIT(scoreMult);
        }

        // otherwise hand and object positions are zero update based on bonus scoring
        else
        {
            // update score based on frames
            int bonusMult = 1 + 1/framesSinceBonus;

            JustJugglingMain.OBJ_HIT(bonusMult);
        }

    }


    // Check if bonus input is available
    TextMeshPro IsBonusAvailable()
    {
        // get all elements with tag "Bonus"
        try
        {
            TextMeshPro[] bonusObjects = GameObject.FindWithTag("Bonus").GetComponentsInChildren<TextMeshPro>();GameObject.FindWithTag("Bonus");
                
            // return first bonus object found
            if( bonusObjects.Length > 0 )
            {
                return bonusObjects[0];
            }
            else
            {
                return null;
            }
        }
        catch( NullReferenceException )
        {
            return null;
        }
    }


    // Throw object
    void ThrowObject( GameObject jugglingObject )
    {
        // acquire data
        // ============
        // update object expected input
        Next_Move.UPDATE_INPUT();

        // get position of juggling object
        Vector3 objectPos = jugglingObject.transform.position;
        double objectHeight = objectPos.y;
        double objectX = objectPos.x;

        // get time object needs to land in hand next from music sync script
        double timeUntilIntercept = SongManager.INTERCEPT_TIME();

        // get gravity from juggling object
        double gravity = jugglingObject.GetComponent<Rigidbody>().useGravity ? Physics.gravity.y : 0;

        if( gravity == 0 )
        {
            Debug.LogWarning("Gravity is zero, object will not be thrown");
        }

        // randomly decide left or right hand (-1 or 1)
        int hand = UnityEngine.Random.Range(-1, 1);

        // get x position of hand (hand location +/- deviation)
        // this will make the look of the juggling more natural since the hands will not always be in the same place
        double x = hand + UnityEngine.Random.Range(-(float)xDeviation, (float)xDeviation);

        // calculate trajectory (basic physics projectile motion)
        // https://www.omnicalculator.com/physics/projectile-motion
        // ====================
        // get velocity in x direction
        double velocityX = (x - objectX) / timeUntilIntercept;

        // get velocity in y direction
        double velocityY = 0.5 * gravity / timeUntilIntercept - (interceptHeight - objectHeight) / timeUntilIntercept;

        // NOTE: if object is not ball add rotations here
        
        // throw object
        // ============
        // get rigidbody of juggling object
        Rigidbody rb = jugglingObject.GetComponent<Rigidbody>();

        // set velocity of juggling object
        rb.velocity = new Vector3((float)velocityX, (float)velocityY, 0);
    }

    // End game
    void EndGame()
    {
        // load game over scene over the course of 3 seconds
        SceneManager.LoadSceneAsync("GameOver", LoadSceneMode.Additive);

        // unload current scene
        SceneManager.UnloadSceneAsync("Game");
    }
}
