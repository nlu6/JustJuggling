/*
 * Contributors: Ian Dennis
 * Last Modified: December 7th, 2023
 * 
 * Purpose: Start up script for the Just Juggling game. script handles
*           the spawning of objects, and the start of the game.
*           Should be called by the main script.
 * Binds With:  Main Camera
 * Modifies:    Juggling Objects,
 *              Crowd
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    // script
    public static StartUp Script;

    [Header("Juggling Objects")]
    [Tooltip("The object that will be juggled")]
    public GameObject jugglingObject;
    [Tooltip("Player Left Hand")]
    public GameObject leftHand;
    [Tooltip("Player Right Hand")]
    public GameObject rightHand;
    [Tooltip("The number of objects that will be juggled")]
    public int numObjects = NumBallsSlider.numBalls;
    [Tooltip("The hand that an object will start in")]
    public int hand = -1;
    [Tooltip("Spawn interval in seconds")]
    public float spawnInterval = 2f;
    [Tooltip("Array of materials that will be applied to the juggling objects")]
    public Material[] materials;

    void Awake()
    {
        Script = this;
    }

    public static void SPAWN_OBJECTS()
    {
        
        // spawn objects
        Script.StartCoroutine(Script.SpawnJugglingObjects());
    }

    IEnumerator SpawnJugglingObjects()
    {
        // spawn objects
        for (int i = 0; i < numObjects; i++)
        {
            // wait for spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // spawn object in either hand (-1 or 1 in x axis)
            GameObject obj = Instantiate(jugglingObject, new Vector3(hand, 4f, -1), Quaternion.identity);
            jugglingObject.GetComponent<JugglingObject>().ResetThrow( obj.transform.position );

            // get next material
            jugglingObject.GetComponent<Renderer>().material = materials[i % materials.Length];

            // Let object know which hand it's in, it will throw itself
            jugglingObject.GetComponent<JugglingObject>().throwingHand = hand;
        }
    }
}
