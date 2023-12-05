using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensor : MonoBehaviour
{
    [Header("Inscribed")]
    public bool objectNearHand = false;

    void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.tag == "Hand" )
        {
            Debug.Log("Bruh!");
            objectNearHand = true;
        }
        else
        {
            Debug.Log("Brah!");
            objectNearHand = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if( other.gameObject.tag == "Hand" )
        {
            Debug.Log("Bruh :(");
            objectNearHand = false;
        }
        else
        {
            Debug.Log("Brah :(");
            objectNearHand = true;
        }
    }
}
