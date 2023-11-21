/*
 * Contributors: Nathan Underwood
 * Last Modified: Nov. 21, 2023
 * 
 * Purpose: This script fetches the information from a falling object and moves
 * the juggler's hands to the correct position to catch the object. When
 * appropriate, the Catch function from this script should be called by the
 * game.
 * 
 * Binds With:  Juggling Objects
 * Modifies:    Juggler's Hands (Position)
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum HandFlag{LEFT, RIGHT};

public class CatchObject : MonoBehaviour {

    public float objectDownwardVelocity;
    public GameObject jugglingHand;
    public GameObject jugglingObject;
    public GameObject nearestObject;
    public HandFlag handFlag;
    public Vector3 handPosition;
    public Vector3 objectPosition; 
    
    public void Catch() {

        // fetch the nearest juggling object
        nearestObject = FindNearestObject();

        // fetch the positional data from the juggling object
        // handFlag = jugglingObject.handFlag;
        objectPosition = jugglingObject.transform.position;
        // objectDownwardVelocity = jugglingObject.Rigidbody.velocity.z; // how fast the object is traveling downward

        // determine which hand needs to be moved – based on the hand flag that an object has
        if(handFlag == HandFlag.LEFT) {
            
            jugglingHand = GameObject.Find("LeftHand");  
        }
        else if(handFlag == HandFlag.RIGHT) {

            jugglingHand = GameObject.Find("RightHand");  
        }

        // fetch the positional data of the hand
        handPosition = jugglingHand.transform.position;

        // intercept the juggling object
        InterceptObject();
    }

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

    GameObject FindNearestObject() {

        float distance = Mathf.Infinity; // distance used for comparisons
        GameObject nearestObject = null; // the nearest juggling object to the player
        GameObject[] jugglingObjects; // the list of all juggling objects
        Vector3 position = transform.position; // get the position of the hand

        jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        foreach(GameObject gameObject in jugglingObjects) {

            Vector3 delta = gameObject.transform.position - position;
            float currentDistance = delta.sqrMagnitude;
            if (currentDistance < distance) {

                nearestObject = gameObject;
                distance = currentDistance;
            }
        }
        return nearestObject;
    }
}
