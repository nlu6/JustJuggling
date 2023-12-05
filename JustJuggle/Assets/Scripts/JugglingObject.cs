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
    public double xDeviation = 1;
    public double interceptHeight = 3.4;
    private const int fixedFPS = 50;
    private float dpi = 96;

    [Header("Dynamic")]
    [Tooltip("Input expected from juggling object, changes every throw")]
    public String expectedInput = "";
    public int throwingHand = 0;
    public int destinationHand = 0; // -1 for right, 1 for left
    private String lastInput = "";
    private double destinationX = 0;
    private double gravity = 0;
    private double framesUntilIntercept = 0;
    private double timeUntilIntercept = 0;
    private double xStep = 0;
    private double yStep = 0;
    private double maxHeight = 0;




    [Header("Static")]
    [Tooltip("JugglingObject script")]
    public static JugglingObject Script;

    // Start is called before the first frame update
    void Start()
    {
        // get dpi
        dpi = Screen.dpi;
        
        // conver dpi from in to m
        dpi /= (float)39.37;

        // update heigh
        interceptHeight *= dpi;

        // Get juggling object
        int inputIndex = UnityEngine.Random.Range(0, possibleInputs.Length);
        expectedInput = possibleInputs[inputIndex];

        // set input text
        inputText.text = expectedInput;

        gravity = Physics.gravity.magnitude * dpi / Math.Pow(fixedFPS, 2);  // gravity in pixel/frame^2

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
            MoveObject();

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
    public void MoveObject()
    {
        // get position of juggling object
        float objectHeight = jugglingObject.transform.position.y;
        float objectX = jugglingObject.transform.position.x;

        if( objectX <= destinationX )
        {
            xStep = -xStep;
        }
        if( objectHeight >= maxHeight )
        {
            yStep = -yStep;
        }


        // update position  
        jugglingObject.transform.position = jugglingObject.transform.position + new Vector3((float)xStep, (float)yStep, 0);
        // Debug.Log("Object Height: " + objectHeight + "\nOBject X Change: " + xStep + "\nObject Height Change: " + yStep); 
        Debug.Log("X Step: " + xStep + "\nY Step: " + yStep);
    }

    // Update destination
    void UpdateDestination()
    {
        
        // get time object needs to land in hand next from music sync script
        timeUntilIntercept = SongManager.INTERCEPT_TIME();

        // log time intil intercept
        framesUntilIntercept = timeUntilIntercept * fixedFPS;

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
        destinationX = hand + UnityEngine.Random.Range(-(float)xDeviation, (float)xDeviation); // convert to pixels

        // save step size
        double currentPost = jugglingObject.transform.position.x;
        xStep = Math.Abs(destinationX - currentPost) / framesUntilIntercept;

        // get initial velocity
        double initialY = jugglingObject.transform.position.y * dpi; // convert to pixels
        double initVel = (initialY - interceptHeight + 0.5 * gravity * Math.Pow(framesUntilIntercept, 2) ) / framesUntilIntercept;

        // get maximum height of throw
        maxHeight = Math.Pow(initVel, 2)/ (2 * gravity);

        // get y step size
        yStep = 2 * (maxHeight - initialY) / framesUntilIntercept;

        // update input
        lastInput = expectedInput;
    }

    // Update is called once per frame
    public void UpdateInput()
    {
        // Changes number inputted
        int inputIndex = UnityEngine.Random.Range(0, possibleInputs.Length);
        lastInput = expectedInput;
        expectedInput = possibleInputs[inputIndex];

        // update input display
        inputText.text = expectedInput;
    }
}
