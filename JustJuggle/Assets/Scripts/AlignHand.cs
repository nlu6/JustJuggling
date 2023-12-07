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

    [Header("Inscribed")]
    GameObject jugglingHand = null; // the hand used to catch the ball
    GameObject nearestObject = null; 
    GameObject player = null;
    GameObject[] jugglingObjects; // a list of all juggling objects

    JugglingObject jugglingObject; // a reference to the properties of the juggling object
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
            jugglingHand = GameObject.Find("LeftHand"); // set the default hand to the left hand

            if(jugglingObject.destinationHand == -1) {
                
                jugglingHand = GameObject.Find("RightHand"); // switch hands to the right hand
            }

            // intercept the juggling object
            InterceptObject();
        }
    }

    void InterceptObject() {

        Vector3 landingSpot = Vector3.zero; // the position on the x axis where the object will land

        // fetch positional data
        objectPosition = nearestObject.transform.position;

        landingSpot.x = objectPosition.x;
        landingSpot.y = 3.4f; 
        landingSpot.z = -1; // yz plane position is constant

        // define the maximum distance the hand can physically travel
        // if left hand space is [0, 2] and right hand space is [-2, 0]
        if( jugglingHand.name == "LeftHand" ) {

            if( landingSpot.x > 2 ) {

                landingSpot.x = 2;
            }
            else if( landingSpot.x < 0 ) {

                landingSpot.x = 0;
            }
        }
        else {

            if( landingSpot.x < -2 ) {

                landingSpot.x = -2;
            }
            else if( landingSpot.x > 0 ) {

                landingSpot.x = 0;
            }
        }

        // move the hand to below the landing spot 
        jugglingHand.transform.position = landingSpot;
    }

    GameObject FindNearestFallingObject() {

        float distance = Mathf.Infinity; // distance used for comparisons
        float velocity; // the velocity of the juggling object
        GameObject nearestObject = null; // the nearest juggling object to the player
        Vector3 position = player.transform.position; // get the position of the player

        // find all of the juggling objects
        jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        // determine which juggling object is closest to the player
        foreach(GameObject gameObject in jugglingObjects) {

            jugglingObject = gameObject.GetComponent<JugglingObject>();
            rigidbody = gameObject.GetComponent<Rigidbody>();

            velocity = rigidbody.velocity.y;
            int hand = jugglingObject.destinationHand;

            // if(velocity < 0) { // check that the object is falling

                Vector3 delta = gameObject.transform.position - position;
                float currentDistance = delta.sqrMagnitude;
                if (currentDistance < distance) {

                    nearestObject = gameObject;
                    distance = currentDistance;
                }
            // }
        }
        return nearestObject;
    }
}
