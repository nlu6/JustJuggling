/*
 * Contributors: Ian Dennis
 * Last Modified: Oct. 24th, 2023
 * 
 * Purpose: Main script for the Just Juggling game. script handles
 *         the score, and ending the game.
 * 
 * Binds With:  Juggling Hands Objects
 * Modifies:    Juggling Object Physics,
 *              Player Score Value,
 *              etc.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JustJugglingMain : MonoBehaviour
{
    static private JustJugglingMain Script;
    
    [Header("Inscribed")]
    public AudioSource musicSource;
    public int scoreAdd = 100;
    
    private SongManager SongManagerScript;
    
    [Header("Dynamic")]
    public int playerScore;
    public int objJuggled;

    void Awake()
    {
        Script = this;
    }
    
    void Start()
    {
        SongManagerScript = musicSource.GetComponent<SongManager>();
        
        playerScore = 0;
        objJuggled = 0;

        // wait 5 seconds before calling start up
        Invoke("StartUp", 5);
        
        // call start up
        StartUp.SPAWN_OBJECTS();
    }
    
    // Score multiplier is based in here since it changes
    // based on player performance and whether it was a bonus
    // or normal score
    // check HandleCatch.UpdateScore for muliplier calculations
    static public void OBJ_HIT( float scoreMult )
    {
        Script.playerScore += (int)(Script.scoreAdd * scoreMult);
        Script.objJuggled++;
    }
    
    static public void GAME_END()
    {
        Script.SongManagerScript.StopSong();
        
        // Update score
        PlayerPrefs.SetInt("Score", Script.playerScore);

        // TODO: fade to end screen instead of jumping
        SceneManager.LoadScene("GameOver_Screen");
    }
}
