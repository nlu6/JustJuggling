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
    public GameObject player;  
    public HandFlag handFlag;
    public Vector3 handPosition;
    public Vector3 objectPosition; 
    
    public void Catch() {

        // fetch a reference to the player
        player = GameObject.Find("Player");  

        // fetch the nearest juggling object
        nearestObject = FindNearestObject();

        // determine which hand needs to be moved – based on the hand flag that the nearest juggling object has
        // handFlag = nearestObject.handFlag;

        if(handFlag == HandFlag.LEFT) {
            
            jugglingHand = GameObject.Find("LeftHand");  
        }
        else if(handFlag == HandFlag.RIGHT) {

            jugglingHand = GameObject.Find("RightHand");  
        }

        // fetch the positional data from the juggling object
        objectPosition = nearestObject.transform.position;
        // objectDownwardVelocity = nearestObject.Rigidbody.velocity.z; // how fast the object is traveling downward

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
        Vector3 position = player.transform.position; // get the position of the player

        jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        // determine which juggling object is closest to the player
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
