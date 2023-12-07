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

    [Header("Object References")]
    public GameObject jugglingHand = null; // the hand used to catch the ball
    public GameObject nearestObject = null; 
    public GameObject player = null;
    public GameObject[] jugglingObjects; // a list of all juggling objects
    public JugglingObject jugglingObject; // a reference to the properties of the juggling object

    public void FixedUpdate() {

        Catch(); // constantly hover the hand beneath the position the ball will fall on
    }
    
    public void Catch() {

        // fetch a reference to the player
        player = GameObject.Find("Player");  

        // fetch the nearest juggling object
        nearestObject = FindNearestFallingObject();

        jugglingHand = GameObject.Find("RightHand");

        if(nearestObject != null) {

            if(jugglingObject.destinationHand == 1) {

                jugglingHand = GameObject.Find("LeftHand"); // set the default hand to the left hand
            }
            else if(jugglingObject.destinationHand == -1) {
                
                jugglingHand = GameObject.Find("RightHand"); // switch hands to the right hand if applicable
            }

            // intercept the juggling object
            InterceptObject();
        }
    }

    void InterceptObject() {

        Vector3 landingSpot = new Vector3(0, 3.4f, -1); // the position on the yz plane is constant
        Vector3 objectPosition = nearestObject.transform.position; // the position of the ball

        // landingSpot.x = (float)jugglingObject.destinationX;
        
        landingSpot.x = objectPosition.x;

        // define the maximum distance the hand can physically travel 
        // left hand space is [0, 2] and right hand space is [-2, 0]
        if(jugglingHand.name == "LeftHand") {

            if(landingSpot.x > 2) {

                landingSpot.x = 2;
            }
            else if(landingSpot.x < 0) {

                landingSpot.x = 0;
            }
        }
        else {

            if(landingSpot.x < -2) {

                landingSpot.x = -2;
            }
            else if(landingSpot.x > 0) {

                landingSpot.x = 0;
            }
        }

        // move the hand to below the landing spot 
        jugglingHand.transform.position = landingSpot;

        // jugglingHand.transform.position = Vector3.Lerp(jugglingHand.transform.position, landingSpot, 1);
    }

    GameObject FindNearestFallingObject() {

        bool downwardTrajectory; // the trajectory of the juggling object
        float distance = Mathf.Infinity; // distance used for comparisons
        GameObject nearestObject = null; // the nearest juggling object to the player
        Vector3 position = player.transform.position; // get the position of the player

        // find all of the juggling objects
        jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        // determine which juggling object is closest to the player
        foreach(GameObject gameObject in jugglingObjects) {

            jugglingObject = gameObject.GetComponent<JugglingObject>();
            downwardTrajectory = jugglingObject.downwardTrajectory;

            if(downwardTrajectory) {

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
