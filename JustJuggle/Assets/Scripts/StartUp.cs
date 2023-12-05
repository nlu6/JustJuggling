/*
 * Contributors:
 * Last Modified: Oct. 24th, 2023
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
    public int numObjects;
    [Tooltip("Array of materials that will be applied to the juggling objects")]
    public Material[] materials;


    [Header("Crowd")]
    [Tooltip("The number of crowd members")]
    public GameObject crowd;
    [Tooltip("One of the crowd members")]
    public int crowdSize;
    [Tooltip("Array of materials that will be applied to the crowd")]
    public Material[] crowdMaterials;

    void Awake()
    {
        Script = this;
    }

    public static void SPAWN_OBJECTS()
    {
        // spawn crowd
        Script.SpawnCrowd();

        // spawn objects
        Script.SpawnJugglingObjects();
    }

    void SpawnJugglingObjects()
    {
        int hand = -1;
        for (int i = 0; i < numObjects; i++)
        {
            // spawn object in either hand (-1 or 1 in x axis)
            Instantiate(jugglingObject, new Vector3(hand, (float)3.5, -1), Quaternion.identity);
            jugglingObject.transform.position = new Vector3(hand, (float)3.5, -1);

            // apply material
            if( i < materials.Length )
            {
                // use given materials if available
                jugglingObject.GetComponent<Renderer>().material = materials[i];
            }
            else
            {
                // get a random material from the array
                Material material = materials[Random.Range(0, materials.Length)];

                // alter material color slightly from current color
                Color color = material.color;
                color.r += Random.Range(-0.1f, 0.1f);
                color.g += Random.Range(-0.1f, 0.1f);
                color.b += Random.Range(-0.1f, 0.1f);
                material.color = color;

                // apply material
                jugglingObject.GetComponent<Renderer>().material = material;
            }

            // Let object know which hand it's in, it will throw itself
            jugglingObject.GetComponent<JugglingObject>().throwingHand = hand;

            // switch hands
            hand *= -1;
        }
    }

    void SpawnCrowd()
    {
        // TODO: spawn crowd
        // NOTE: if less materials than crowd randomize materials slightly from what's available
    }
}
