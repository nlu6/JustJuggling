/*
 * Contributors:
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

public class HandleCatch : MonoBehaviour
{
    // public variables
    public int scoreAddition = 0; // score added to player score for catching object
    public int playerScore = 0; // player score

    public int maxCatchDistance = 0; // maximum distance where player can catch object

    public GameObject playerHand; // player hand attached to this script
    public GameObject[] jugglingObjects; // list of all juggling objects in scene

    // private variables
    string expectedInput = ""; // input expected from juggling object
    string playerInput = ""; // input from player

    int framesSinceBonus = 0; // frames since bonus input became an option

    void Start()
    {
        // get list of all juggling objects in scene
        jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // check for collision with juggling object
        foreach( GameObject jugglingObject in jugglingObjects )
        {
            if( jugglingObject.GetComponent<Collider>().bounds.Intersects(playerHand.GetComponent<Collider>().bounds) )
            {
                // get input juggling oject is expecting
                // TODO: get input from juggling object script
                expectedInput = "test";

                // get key input from player if any

                // check for player input
                if( Input.GetKeyDown(KeyCode.Space) )
                {
                    // get position of this hand

                    // get position of juggling object

                    // update player score based on position of objects
                        // function: updateScore

                    // call throw object script
                }
            }
        }

        // next check for bonus input active
            // if so increment frame counter

            // check for player input
                // call updateScore

        // otherwise
            // reset frame counter
    }

    // Update player score
    void updateScore( int framesSinceBonus = -1, Vector3 handPos = default, Vector3 objectPos = default )
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

            // update score based on distance
            playerScore += scoreAddition * (maxCatchDistance - distance);

        }

        // otherwise hand and object positions are zero update based on bonus scoring
        else
        {
            // update score based on frames
            playerScore += scoreAddition * (1 + 1/framesSinceBonus);

        }

    }
}
