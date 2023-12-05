/*
 * Contributors: Nathan Underwood
 * Last Modified: Dec. 4, 2023
 * 
 * Purpose: This script fetches the information from a falling object and moves
 * the juggler's hands to the correct position to catch the object. The hand
 * constantly hovers below the ball it's meant to catch.
 * 
 * Binds With:  Juggling Objects
 * Modifies:    Juggler's Hands (Position)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignHand : MonoBehaviour {

    GameObject jugglingHand;
    GameObject nearestObject;
    GameObject player;
    JugglingObject jugglingObject;
    new Rigidbody rigidbody;
    Vector3 objectPosition; 

    public void FixedUpdate() {

        Catch(); // constantly hover the hand beneath the position the ball will fall on
    }
    
    void Catch() {

        // fetch a reference to the player
        player = GameObject.Find("Player");  

        // fetch the nearest juggling object
        nearestObject = FindNearestFallingObject();

        if(nearestObject != null) {

            // fetch data from the juggling object
            jugglingObject = nearestObject.GetComponent<JugglingObject>();
            rigidbody = nearestObject.GetComponent<Rigidbody>();

            // determine which hand needs to be moved – based on the identifier that the object has
            if(jugglingObject.destinationHand == -1) {
                
                jugglingHand = GameObject.Find("RightHand");  
            }
            else if(jugglingObject.destinationHand == 1) {

                jugglingHand = GameObject.Find("LeftHand");  
            }

            // intercept the juggling object
            InterceptObject();
        }
    }

    void InterceptObject() {

        Vector3 landingSpot; // the position on the xz plane where the object will land

        // fetch positional data
        objectPosition = nearestObject.transform.position;

        landingSpot.x = objectPosition.x;
        landingSpot.y = 3.4f; // constant value the hands stay at
        landingSpot.z = objectPosition.z;

        // define the maximum distance the hand can physically travel
        if(landingSpot.x > 2.5f) {

            landingSpot.x = 2.5f;
        }
        if(landingSpot.x < -2.5f) {

            landingSpot.x = -2.5f;
        }

        // move the hand to below the landing spot 
        jugglingHand.transform.position = landingSpot;
    }

    GameObject FindNearestFallingObject() {

        float distance = Mathf.Infinity; // distance used for comparisons
        float velocity; // the velocity of the juggling object
        GameObject nearestObject = null; // the nearest juggling object to the player
        Vector3 position = player.transform.position; // get the position of the player
        
        GameObject[] gameObjects; // the list of all juggling objects

        gameObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        // determine which juggling object is closest to the player
        foreach(GameObject gameObject in gameObjects) {

            jugglingObject = gameObject.GetComponent<JugglingObject>();
            rigidbody = gameObject.GetComponent<Rigidbody>();

            velocity = rigidbody.velocity.y;

            if(velocity < 0) { // check that the velocity is downward

                Vector3 delta = gameObject.transform.position - position;
                float currentDistance = delta.sqrMagnitude;
                if (currentDistance < distance) {

                    nearestObject = gameObject;
                    distance = currentDistance;
                }
            }
        }
        return nearestObject;
    }
}
