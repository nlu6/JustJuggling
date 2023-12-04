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
using UnityEngine;

public class Start_Up : MonoBehaviour
{
    // script
    public static Start_Up Script;

    [Header("Juggling Objects")]
    [Tooltip("The object that will be juggled")]
    public GameObject jugglingObject;
    [Tooltip("One of the crowd members")]
    public int numObjects;
    [Tooltip("Array of materials that will be applied to the juggling objects")]
    public Material[] materials;


    [Header("Crowd")]
    [Tooltip("The number of crowd members")]
    public GameObject crowd;
    [Tooltip("The number of objects that will be juggled")]
    public int crowdSize;
    [Tooltip("Array of materials that will be applied to the crowd")]
    public Material[] crowdMaterials;

    void Awake()
    {
        Script = this;
    }
    public static void SPAWN_OBJECTS()
    {
        // spawn objects
        Script.SpawnJugglingObjects();

        // spawn crowd
        Script.SpawnCrowd();
    }

    void SpawnJugglingObjects()
    {
        for (int i = 0; i < numObjects; i++)
        {
            // spawn object
            Instantiate(jugglingObject, new Vector3(0, 0, 0), Quaternion.identity);

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
        }
    }

    void SpawnCrowd()
    {
        // TODO: spawn crowd
        // NOTE: if less materials than crowd randomize materials slightly from what's available
    }
}
