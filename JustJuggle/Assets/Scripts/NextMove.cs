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

public class Next_Move : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("Juggling Object")]
    public GameObject jugglingObject;
    public String[] possibleInputs = {"a", "s", "d", "f", "g"};

    [Header("Dynamic")]
    [Tooltip("Input expected from juggling object, changes every throw")]
    String expectedInput = "";

    [Header("Static")]
    [Tooltip("Next_Move script")]
    public static Next_Move Script;

    // Start is called before the first frame update
    void Start()
    {
        // Get juggling object
        Script = this;
    }
    // Update is called once per frame
    public static void UPDATE_INPUT()
    {
        // Changes number inputted
        int inputIndex = UnityEngine.Random.Range(0, Script.possibleInputs.Length);
        Script.expectedInput = Script.possibleInputs[inputIndex];
    }
}
