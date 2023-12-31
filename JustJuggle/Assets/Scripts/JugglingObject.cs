/* Contributor: Ian Dennis
 * Date: December 7th, 2023
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
    public Rigidbody proximitySensor;
    public TextMeshPro inputText = null;
    public String[] possibleInputs = {"A", "S", "D", "F"};
    public double xDeviation = 0.3;
    public double interceptHeight = 3.4;
    public int camViewHeight = 12;
    private const int fixedFPS = 50;
    public float slowFactor = 3.5f;
    private float dpi = 96;
    [Tooltip("Cheat mode for debugging")]
    public bool cheatMode = false;
    public float throwHeightLimit = 16;
    private new Rigidbody rigidbody;

    [Header("Dynamic")]
    [Tooltip("Input expected from juggling object, changes every throw")]
    public String expectedInput = "";
    public bool downwardTrajectory = false;
    public int throwingHand = 0;
    public int destinationHand = 0; // -1 for right, 1 for left
    private String lastInput = "";
    public bool objectNearHand = false;
    public double destinationX = 0;
    private double gravity = 0;
    private double framesUntilIntercept = 0;
    private double timeUntilIntercept = 0;
    private double xStep = 0;
    private double yStep = 0;
    private double maxHeight = 0;
    private Vector3 oldPosition = new Vector3(0, 0, 0);
    private bool throwing = false;

    [Header("Static")]
    [Tooltip("JugglingObject script")]
    public static JugglingObject Script;

    // Start is called before the first frame update
    void Start()
    {
        // get dpi
        dpi = Screen.dpi;

        // if dpi is not found, set to default
        if( dpi == 0 )
        {
            dpi = 96;
        }
        
        // conver dpi from in to m
        dpi /= (float)39.37;

        // update height
        interceptHeight *= dpi;

        // get slow factor from user preferences
        float tempSlowFactor = PlayerPrefs.GetFloat("SlowFactor");

        if( tempSlowFactor >= 1 )
        {
            slowFactor = tempSlowFactor;
        }

        // Get juggling object
        int inputIndex = UnityEngine.Random.Range(0, possibleInputs.Length);
        expectedInput = possibleInputs[inputIndex];

        // set input text
        inputText.text = expectedInput;

        gravity = Physics.gravity.magnitude * dpi / Math.Pow(fixedFPS, 2);  // gravity in pixel/frame^2

        // get initial time until intercept
        timeUntilIntercept = SongManager.INTERCEPT_TIME();
        framesUntilIntercept = timeUntilIntercept * fixedFPS;
        
        // assign rigidbody
        rigidbody = GetComponent<Rigidbody>();
    }

    // listen for space input
    void FixedUpdate()
    {
        if( throwing )
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
        float objectX = jugglingObject.transform.position.x;
        float objectHeight = jugglingObject.transform.position.y*dpi;

        if( objectHeight >= maxHeight )
        {
            yStep = -yStep;
            downwardTrajectory = true;
        }
        
        // cheat mode for debugging, freezes object near hand
        if( cheatMode && downwardTrajectory && (Math.Abs(objectHeight - interceptHeight) < 0.1 * dpi
                        || objectHeight < interceptHeight ) )
        {
            jugglingObject.transform.position = new Vector3(objectX, (float)interceptHeight/dpi, -1);
            
            yStep = 0;
            
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
                                    RigidbodyConstraints.FreezePositionZ;
            rigidbody.useGravity = false;

            proximitySensor.constraints = RigidbodyConstraints.FreezePositionY | 
                                          RigidbodyConstraints.FreezePositionZ;
            proximitySensor.useGravity = false;
        }

        if( cheatMode && downwardTrajectory && Math.Abs(objectX - destinationX) < 0.1 * dpi )
        {
            xStep = 0;

            rigidbody.constraints = RigidbodyConstraints.FreezePositionX | 
                                    RigidbodyConstraints.FreezePositionZ;

            proximitySensor.constraints = RigidbodyConstraints.FreezePositionX |
                                          RigidbodyConstraints.FreezePositionZ;
        }

        // update position
        Vector3 step = new Vector3((float)xStep, (float)yStep, 0);
        jugglingObject.transform.position = oldPosition + step;
        oldPosition = jugglingObject.transform.position;
    }

    // Update destination
    void UpdateDestination()
    {
        
        // get time object needs to land in hand next from music sync script
        timeUntilIntercept = SongManager.INTERCEPT_TIME();

        // log time intil intercept
        framesUntilIntercept = timeUntilIntercept * fixedFPS;

        // get current position of juggling object
        double currentPos = jugglingObject.transform.position.x * dpi;
        double currentHeight = jugglingObject.transform.position.y * dpi;

        // set throwing hand
        if( currentPos <= 0 )
        {
            throwingHand = -1;
        }
        else
        {
            throwingHand = 1;
        }

        // get Y value (projectile motion)
        // get initial velocity
        double initVel =(currentHeight - interceptHeight + 0.5 * gravity * Math.Pow(framesUntilIntercept, 2) ) / framesUntilIntercept;

        // get maximum height of throw
        maxHeight = currentHeight + Math.Pow(initVel, 2)/ (2 * gravity);

        // correction for playability, if max height is greater than 16, set it to 13-16 randomly
        if( maxHeight > throwHeightLimit * dpi )
        {
            maxHeight = UnityEngine.Random.Range(throwHeightLimit - 3, throwHeightLimit) * dpi;
            
            // backsolve for initial velocity
            initVel = Math.Sqrt(2 * gravity * (maxHeight - currentHeight));

            // backsolve for frames until intercept (positive of quadratic formula)
            framesUntilIntercept = (initVel + Math.Sqrt(Math.Pow(-initVel, 2) - 2 * gravity * (currentHeight - interceptHeight))) / gravity;
        }

        // get y step size take the maximum vertical between the current height and the intercept height
        yStep = currentHeight > interceptHeight ? Math.Abs(maxHeight - currentHeight) / framesUntilIntercept : Math.Abs(maxHeight - interceptHeight) / framesUntilIntercept;
        
        // double step size to account for going up and down
        yStep *= 2;

        // get X value (simple time vs distance)
        // randomly decide left or right hand (-1 or 1) to go to
        destinationHand = GetHand();

        // get x position of hand (hand location +/- deviation)
        // this will make the look of the juggling more natural since the hands will not always be in the same place
        destinationX = (destinationHand + UnityEngine.Random.Range(-(float)xDeviation, (float)xDeviation)) * dpi;

        // save step size
        xStep = (destinationX - currentPos) / framesUntilIntercept;

        // slow down time
        xStep /= slowFactor;
        yStep /= slowFactor;

        // update input
        lastInput = expectedInput;

        // update position
        oldPosition = jugglingObject.transform.position;
        
        // throw
        throwing = true;
    }

    // Update is called once per frame
    public void UpdateInput()
    {
        // Changes number inputted
        int inputIndex = UnityEngine.Random.Range(0, possibleInputs.Length);
        lastInput = expectedInput;
        expectedInput = possibleInputs[inputIndex];

        // update input display
        inputText.text = expectedInput.ToUpper();

        // update physics if cheating
        if( cheatMode )
        {
            rigidbody.constraints = RigidbodyConstraints.None | 
                                    RigidbodyConstraints.FreezeRotation;

            proximitySensor.constraints = RigidbodyConstraints.None |
                                          RigidbodyConstraints.FreezeRotation;
        }
    }

    // Stop moving if requested by hand
    public void ResetThrow( Vector3 handPosition )
    {
        // stop moving
        xStep = 0;
        yStep = 0;

        // update position
        oldPosition = handPosition + new Vector3(0, 0.5f, 0);

        // throw again
        downwardTrajectory = false;
        throwing = false;
    }

    // Get hand to throw to
    public int GetHand()
    {
        // get list of all jugglin objects
        GameObject[] jugglingObjects = GameObject.FindGameObjectsWithTag("JugglingObject");

        // get list of all hands from other juggling objects
        List<int> leftHands = new List<int>();
        List<int> rightHands = new List<int>();

        foreach( GameObject jugglingObject in jugglingObjects )
        {
            // get throwing hand
            int destinationHand = jugglingObject.GetComponent<JugglingObject>().destinationHand;

            // add to list of hands
            if( destinationHand == -1 )
            {
                leftHands.Add(destinationHand);
            }
            else
            {
                rightHands.Add(destinationHand);
            }
        }

        // if more than 70% of objects are going to the same hand, go to the other hand
        if( leftHands.Count > 0 && leftHands.Count > 0.7 * jugglingObjects.Length )
        {
            return 1;
        }
        else if( rightHands.Count > 0 && rightHands.Count > 0.7 * jugglingObjects.Length )
        {
            return -1;
        }
        
        // otherwise randomly choose a hand
        return UnityEngine.Random.Range(0, 2) * 2 - 1;
    }

    public int VerifyTiming()
    {
        return 0; // stub return
    }
}
