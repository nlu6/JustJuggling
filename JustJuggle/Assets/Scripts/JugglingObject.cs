/* Contributor: Jacob Shigeta
 * Date: November 3 2023
 * 
 * Purpose: This script is going to show the next move of the juggling objects (i.e input)
 * 
 * Works with: Juggling Objects, Music
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class JugglingObject : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("Juggling Object")]
    public GameObject jugglingObject;
    public new Rigidbody rigidbody;
    public TextMeshProUGUI inputText = null;
    public String[] possibleInputs = {"a", "s", "d", "f", "g"};
    public double xDeviation = 0.25;
    public double interceptHeight = 3.4;

    [Header("Dynamic")]
    [Tooltip("Input expected from juggling object, changes every throw")]
    public String expectedInput = "";
    public int throwingHand = 0;
    private String lastInput = "";
    private double destinationX = 0;
    private double gravity = 0;
    private double framesUntilIntercept = 0;
    private double timeUntilIntercept = 0;
    private const int fixedFPS = 50;



    [Header("Static")]
    [Tooltip("JugglingObject script")]
    public static JugglingObject Script;

    // Start is called before the first frame update
    void Start()
    {
        // Get juggling object
        int inputIndex = UnityEngine.Random.Range(0, possibleInputs.Length);
        expectedInput = possibleInputs[inputIndex];

        lastInput = expectedInput;

        // set input text
        inputText.text = expectedInput;

        gravity = Physics.gravity.magnitude;

        // get initial time until intercept
        timeUntilIntercept = SongManager.INTERCEPT_TIME();
        framesUntilIntercept = timeUntilIntercept * fixedFPS;
    }

    // listen for space input
    void FixedUpdate()
    {
        if( lastInput == expectedInput )
        {
            // we are still in same trajectory update velocity
            ThrowObject();

            // update time until intercept
            framesUntilIntercept--;
        }
        else
        {
            // we are in a new trajectory update input
            UpdateDestination();
        }
    }

    // Throw object
    public void ThrowObject()
    {
        // get position of juggling object
        Vector3 objectPos = jugglingObject.transform.position;
        double objectHeight = objectPos.y;
        double objectX = objectPos.x;

        // calculate trajectory (basic physics projectile motion)
        // https://www.omnicalculator.com/physics/projectile-motion
        // ====================
        // get velocity in x direction
        double handX = destinationX;
        double velocityX = (handX - objectX) / timeUntilIntercept;
        Debug.Log(handX);
        Debug.Log(objectX);
        Debug.Log(timeUntilIntercept);
        Debug.Log(velocityX);

        // get velocity in y direction
        double velocityY = Math.Abs(interceptHeight - objectHeight) / timeUntilIntercept - 0.5 * gravity / timeUntilIntercept;
        Debug.Log(interceptHeight);
        Debug.Log(objectHeight);
        Debug.Log(gravity);
        Debug.Log(velocityY);

        // get velocity in z (nothing we're planar )
        double velocityZ = 0;

        // NOTE: if object is not ball add rotations here
        
        // throw object
        // ============
        // set velocity of juggling object
        rigidbody.velocity = new Vector3((float)velocityX, (float)velocityY, (float)velocityZ);
        Debug.Log("New Velocity: " + rigidbody.velocity);

        // update time until intercept
        timeUntilIntercept = framesUntilIntercept / fixedFPS;
    }

    // Update destination
    void UpdateDestination()
    {
        // set throwing hand
        if( destinationX < 0 )
        {
            throwingHand = -1;
        }
        else
        {
            throwingHand = 1;
        }

        // randomly decide left or right hand (-1 or 1) to go to
        int hand = UnityEngine.Random.Range(-1, 1);

        // get x position of hand (hand location +/- deviation)
        // this will make the look of the juggling more natural since the hands will not always be in the same place
        double handX = hand + UnityEngine.Random.Range(-(float)xDeviation, (float)xDeviation);

        // log destination
        destinationX = handX;

        // get time object needs to land in hand next from music sync script
        timeUntilIntercept = SongManager.INTERCEPT_TIME();

        // log time intil intercept
        framesUntilIntercept = timeUntilIntercept * fixedFPS;
    }

    // Update is called once per frame
    public void UpdateInput()
    {
        // Changes number inputted
        int inputIndex = UnityEngine.Random.Range(0, possibleInputs.Length);
        expectedInput = possibleInputs[inputIndex];

        // update input display
        inputText.text = expectedInput;
    }
}
