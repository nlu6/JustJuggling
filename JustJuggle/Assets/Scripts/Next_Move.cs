/* Contributor: Jacob Shigeta
 * Date: November 3 2023
 * 
 * Purpose: This script is going to show the next move of the juggling objects with their timing
 * and key inputs.
 * 
 * Works with: Juggling Objects, Music
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_Move : MonoBehaviour
{
    public GameObject myPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        // Changes number inputted
        GameObject go = Instantiate(myPrefab);
        int number = (int)Random.Range(1, 5f);

        go.GetComponent<TextMesh>().text = number.ToString();
    }
}
