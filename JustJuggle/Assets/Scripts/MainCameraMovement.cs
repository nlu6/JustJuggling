using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    public enum CamState {still, moving}

    [Header("Inscribed")]
    public GameObject player;
    public float timeMoveCam = 4f;
    public bool focusPlayer = true;
    
    [Header("Camera Positions (Must have at least one)")]
    public List<Vector3> camPositions;
    
    [Header("Dynamic")]
    public Camera mainCam;
    public int camPosIndex;
    public float timeSinceSwitch;
    public CamState camState;
    
    public Vector3 startPos;
    public Vector3 endingPos;
    public Vector3 newRot;
    public float modTime;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
        timeSinceSwitch = 0f;
        camState = CamState.still;
        
        camPosIndex = -1;
        switchCameraNow();
    }

    // Update is called once per frame
    void Update()
    {
        // If camera is stationary
        if (camState == CamState.still)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                modTime = 0f;
                camState = CamState.moving;
                startPos = mainCam.transform.position;
                endingPos = camPositions[camPosIndex];
            }
        }
        
        // Otherwise, assume camera is moving
        else
        {
            // Make sure camera is facing the players
            if (focusPlayer)
            {
                mainCam.transform.LookAt(player.transform);
            }
            
            modTime += Time.deltaTime / timeMoveCam;
            mainCam.transform.position = Vector3.Lerp(startPos, endingPos, modTime);
            
            // Check to see if camera is at destination
            if (mainCam.transform.position == endingPos)
            {
                camState = CamState.still;
                camPosIndex++;
                
                // Check if index is out of bounds
                if (camPosIndex >= camPositions.Count || camPosIndex < 0)
                {
                    // Reset index value to 0
                    camPosIndex = 0;
                }
            }
        }
    }
    
    void switchCameraNow()
    {
        // Increase camera position index
        camPosIndex++;
        
        // Check if index is out of bounds
        if (camPosIndex >= camPositions.Count || camPosIndex < 0)
        {
            // Reset index value to 0
            camPosIndex = 0;
        }
        
        // Set main camera's position to new indexed position
        mainCam.transform.position = camPositions[camPosIndex];
        
        // Make sure camera is facing the player
        if (focusPlayer)
        {
            mainCam.transform.LookAt(player.transform);
        }
    }
}
