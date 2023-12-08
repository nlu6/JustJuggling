/*
 * Contributors: Ian Dennis, Dalton Tippings, Jacob Shigeta
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
using TMPro;

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
    public TMP_Text uitScore;
    public TMP_Text uitJuggleCount;
    public string bonusText = "";
    public bool bonusActive = false;
    private int highScore;
    private int highJuggles;

    void Awake()
    {
        Script = this;

        // call start up
        //Start_Up.SPAWN_OBJECTS();
    }
    
    void Start()
    {
        SongManagerScript = musicSource.GetComponent<SongManager>();
        
        playerScore = 0;
        objJuggled = 0;
        
        // call start up
        StartUp.SPAWN_OBJECTS();

        // get high score and high juggles from player prefs
        highScore = PlayerPrefs.GetInt("HighScore");
        highJuggles = PlayerPrefs.GetInt("HighJuggles");

    }
    
    // Score multiplier is based in here since it changes
    // based on player performance and whether it was a bonus
    // or normal score
    // check HandleCatch.UpdateScore for muliplier calculations
    static public void OBJ_HIT( float scoreMult )
    {

        if( scoreMult >= 0 )
        {
            Script.playerScore += (int)(Script.scoreAdd * (1 + scoreMult));
            Script.objJuggled++;
            Script.uitJuggleCount.text = "Objects Juggled: " + Script.objJuggled;
         
        }
        // subtractive scoring for failed inputs
        else
        {
            Script.playerScore += (int)(Script.scoreAdd * scoreMult);
        }

        Script.uitScore.text = "Score: " + Script.playerScore;

        // update high score and high juggles
        if( Script.playerScore > Script.highScore )
        {
            Script.highScore = Script.playerScore;
            PlayerPrefs.SetInt("HighScore", Script.highScore);
        }
        if( Script.objJuggled > Script.highJuggles )
        {
            Script.highJuggles = Script.objJuggled;
            PlayerPrefs.SetInt("HighJuggles", Script.highJuggles);
        }
    }
    
    static public void GAME_END()
    {
        Script.SongManagerScript.StopSong();
        
        // Update score
        PlayerPrefs.SetInt("Score", Script.playerScore);

        SceneManager.LoadScene("GameOver_Screen");
    }
}
