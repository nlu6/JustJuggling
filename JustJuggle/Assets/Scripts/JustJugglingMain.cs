using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JustJugglingMain : MonoBehaviour
{
    static private JustJugglingMain S;
    
    [Header("Inscribed")]
    public AudioSource musicSource;
    public int scoreAdd = 100;
    public int scoreMult = 1;
    
    private SongManager SongManagerScript;
    
    [Header("Dynamic")]
    public int playerScore;
    public int objJuggled;

    void Awake()
    {
        S = this;
    }
    
    void Start()
    {
        SongManagerScript = musicSource.GetComponent<SongManager>();
        
        playerScore = 0;
        objJuggled = 0;
    }
    
    static public void OBJ_HIT()
    {
        S.playerScore += S.scoreAdd * S.scoreMult;
        S.objJuggled++;
    }
    
    static public void GAME_END()
    {
        S.SongManagerScript.StopSong();
        
        // TODO: Load main menu scene
    }
}
