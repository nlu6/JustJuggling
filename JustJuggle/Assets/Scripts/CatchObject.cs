/*
 * Contributors: Nathan Underwood
 * Last Modified: Nov. 2, 2023
 * 
 * Purpose: This script fetches the information from a falling object and moves
 * the juggler's hands to the right position to catch the object.
 * 
 * Binds With:  Juggling Objects
 * Modifies:    Juggler's Hands (Position)
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum HandFlag {LEFT, RIGHT};

public class CatchObject : MonoBehaviour {

    public float objectDownwardVelocity;
    public GameObject jugglingHand;
    public GameObject jugglingObject;
    public HandFlag handFlag;
    public Vector3 handPosition;
    public Vector3 objectPosition; 
    
    void Start() {

        // fetch the nearest juggling object

        // fetch the positional data from the juggling object
        handFlag = jugglingObject.handFlag;
        objectPosition = jugglingObject.transform.position;
        objectDownwardVelocity = jugglingObject.Rigidbody.velocity.z; // how fast the object is traveling downward

        // determine which hand needs to be moved – based on the hand flag that an object has
        if(handFlag == LEFT) {
            
            jugglingHand = GameObject.Find("LeftHand");
            
        }
        else if(handFlag == RIGHT) {

            jugglingHand = GameObject.Find("RightHand");
            
        }

        // fetch the positional data of the hand
        handPosition = jugglingHand.transform.position;

    }

    // intercept the falling object once appropriate

    void InterceptObject() {

        float objectHandDelta; // how far away the object is from the juggler's hand
        float timeUntilIntercept; // how long until the ball reaches the hand
        Vector2 landingSpot; // the position on the xy plane where the object will land

        landingSpot.x = objectPosition.x;
        landingSpot.y = objectPosition.y;

        objectHandDelta = objectPosition.z - jugglingHand.transform.position.z;
        timeUntilIntercept = objectHandDelta / objectDownwardVelocity;

        // move the hand to the landing spot over the time until the intercept
        jugglingHand.transform.position = 
                    Vector3.Lerp(handPosition, landingSpot, timeUntilIntercept);

    }

}
