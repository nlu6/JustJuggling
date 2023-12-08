/*
 * Contributors: Ian Dennis
 * Last Modified: November 29th, 2023
 * 
 * Purpose: Checks if juggling object collides with stage
            Triggers end game if so
 * 
 * Binds With:  Stage
 * Modifies:    Main Scene
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMonitor : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "JugglingObject")
        {
            JustJugglingMain.GAME_END();
        }
    }
}
