using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [Header("Inscribed")]
    public float songBpm;
    public float firstBeatOffset;
    public float pitchIncr = 0.05f;
    public float songStartDelay = 1f;
    public float songLoopDelay = 2f;
    
    [Header("Audio Information")]
    public AudioSource musicSource;
    public TextAsset timestampFile;
    
    // Ball prefab for testing
    //public GameObject ballPrefab;
    
    [Header("Dynamic")]
    public float songLength;
    public float secPerBeat;
    public float songPosition;
    public float dspSongTime;
    public float beatsPerLoop;
    public int completedLoops;

    [Header("Static")]
    public static SongManager Script;
    
    [Header("Juggle Timestamp Information")]
    public List<float> juggleTimes;
    public int numTStmps;
    
    // Private variables used in script
    private float nextTStmp;
    private float songBpmAct;
    private int nextTStmpIndex;
    private int lastAssignedIndex; // used to assign timestamps to balls
    private bool jugglingEnd;
    private bool songPlaying;
    
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        songLength = musicSource.clip.length;
        songBpmAct = songBpm;
        
        ResetSongVars();
        completedLoops = 0;
        
        numTStmps = 0;

        lastAssignedIndex = 0;
        
        if (timestampFile != null)
        {
            GetTimestampListFile();
        }
        else
        {
            GetTimestampListMusic();
        }
        
        ResetJuggVars();
        
        Invoke("PlaySong", songStartDelay);

        Script = this;
    }
    
    void FixedUpdate()
    {
        if (songPlaying)
        {
            songPosition = (float) (AudioSettings.dspTime - dspSongTime - 
                                firstBeatOffset);
                       
            if (songPosition >= songLength)
            {
                StopSong();
                songPosition = 0f;
                completedLoops++;
            
                numTStmps = 0;
                juggleTimes.Clear();
            
                IncreaseTempo();
                ResetJuggVars();
            
                Invoke("PlaySong", songLoopDelay);
            }
        
            if (!jugglingEnd && songPosition >= nextTStmp)
            {
                // Ball Test with timestamp
                /*GameObject newBall = Instantiate(ballPrefab);
                newBall.transform.position = new Vector3(0f, 5.8f, -2f);
                Debug.Log(nextTStmp);*/
                nextTStmpIndex++;
            
                if (nextTStmpIndex == numTStmps)
                {
                    jugglingEnd = true;
                }
                else
                {
                    nextTStmp = juggleTimes[nextTStmpIndex];
                }
            }
        }
    }
    
    public bool SongPlaying
    {
        get{ return songPlaying; }
    }
    
    public float TStmpDiff
    {
        get{ return nextTStmp - songPosition; }
    }
    
    public void PlaySong()
    {
        if (!songPlaying)
        {
            dspSongTime = (float) AudioSettings.dspTime;
            musicSource.Play();
            songPlaying = true;
        }
    }
    
    public void StopSong()
    {
        if (songPlaying)
        {
            musicSource.Stop();
            songPlaying = false;
        }
    }
    
    private void ResetSongVars()
    {
        secPerBeat = 60f / songBpmAct;
        beatsPerLoop = secPerBeat * songLength;
    }
    
    private void ResetJuggVars()
    {
        nextTStmpIndex = 0;
        nextTStmp = juggleTimes[0];
        jugglingEnd = false;
    }
    
    private void IncreaseTempo()
    {
        musicSource.pitch += pitchIncr;
        songBpmAct = songBpm * musicSource.pitch;
        songLength = musicSource.clip.length / musicSource.pitch;
        ResetSongVars();
        
        if (timestampFile != null)
        {
            GetTimestampListFile();
        }
        else
        {
            GetTimestampListMusic();
        }
    }
    
    private void GetTimestampListMusic()
    {
        float curTimestamp = firstBeatOffset;
        int curBeat = 0;
        
        while (curTimestamp < songLength)
        {
            juggleTimes.Add(curTimestamp);
            numTStmps++;
            curBeat++;
            curTimestamp = curBeat * secPerBeat + firstBeatOffset;
        }
    }
    
    private void GetTimestampListFile()
    {
        string fileContents = timestampFile.text;
        string curTimestampStr = "";
        float curPitch = musicSource.pitch;
        float curTimestamp;
        
        foreach (char c in fileContents)
        {
            if (c == '\n')
            {
                curTimestamp = float.Parse(curTimestampStr) / curPitch;
                juggleTimes.Add(curTimestamp);
                numTStmps++;
                curTimestampStr = "";
            }
            else
            {
                curTimestampStr += c;
            }
        }
    }

    // Returns next timestamp, as a float, that a juggling object should land in the hand
    public static double INTERCEPT_TIME()
    {
        double timeUntilIntercept = Script.juggleTimes[Script.lastAssignedIndex] - Script.songPosition;

        Script.lastAssignedIndex = (Script.lastAssignedIndex + 1) % Script.numTStmps;

        return timeUntilIntercept;
    }
}
