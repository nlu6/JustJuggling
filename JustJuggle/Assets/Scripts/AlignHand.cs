/*
 * Contributors: Nathan Underwood
 * Last Modified: Dec. 1, 2023
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

public class AlignHand : MonoBehaviour {

    public double objectDownwardVelocity;
    public GameObject jugglingHand;
    public GameObject jugglingObject;
    public GameObject nearestObject;
    public GameObject player;
    public JugglingObject jugglingScript;
    public new Rigidbody rigidbody;
    public Vector3 handPosition;
    public Vector3 objectPosition; 

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
            jugglingScript = nearestObject.GetComponent<JugglingObject>();
            rigidbody = nearestObject.GetComponent<Rigidbody>();

            // determine which hand needs to be moved – based on the identifier that the object has
            if(jugglingScript.destinationHand == -1) {
                
                jugglingHand = GameObject.Find("RightHand");  
            }
            else if(jugglingScript.destinationHand == 1) {

                jugglingHand = GameObject.Find("LeftHand");  
            }

            // fetch the positional data from the juggling object
            objectPosition = nearestObject.transform.position;
            objectDownwardVelocity = rigidbody.velocity.y; // how fast the object is traveling downward

            // fetch the positional data of the hand
            handPosition = jugglingHand.transform.position;

            // intercept the juggling object
            InterceptObject();
        }
    }

    void InterceptObject() {

        double objectHandDelta; // how far away the object is from the juggler's hand
        double timeUntilIntercept; // how long until the ball reaches the hand
        Vector2 landingSpot; // the position on the xy plane where the object will land

        landingSpot.x = objectPosition.x;
        landingSpot.y = objectPosition.y;

        objectHandDelta = objectPosition.z - jugglingHand.transform.position.z;
        timeUntilIntercept = objectHandDelta / objectDownwardVelocity;

        // move the hand to the landing spot over the time until the intercept
        jugglingHand.transform.position = 
             Vector3.Lerp(handPosition, landingSpot, (float)timeUntilIntercept);
    }

    GameObject FindNearestFallingObject() {

        double downwardVelocity;
        float distance = Mathf.Infinity; // distance used for comparisons
        GameObject nearestObject = null; // the nearest juggling object to the player
        Vector3 position = player.transform.position; // get the position of the player
        
        GameObject[] gameObjects; // the list of all juggling objects

        gameObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        // determine which juggling object is closest to the player
        foreach(GameObject gameObject in gameObjects) {

            jugglingScript = gameObject.GetComponent<JugglingObject>();
            rigidbody = gameObject.GetComponent<Rigidbody>();

            downwardVelocity = rigidbody.velocity.y;

            if(downwardVelocity < 0) { // ensure that the downward velocity is indeed downward

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
