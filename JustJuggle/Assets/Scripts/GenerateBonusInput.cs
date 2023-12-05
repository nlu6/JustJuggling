using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerateBonusInput : MonoBehaviour
{
    private static GenerateBonusInput Script;
    public TextMeshPro bonusText = null;
    public float bonusChance = 0.05f;
    public float bonusPersonChance = 0.0f;
    public bool isBonusPerson = false;
    public float bonusDuration = 5f;
    public static string[] bonusOptions = {"1", "2", "3", "4", "5", "6"};

    void Start()
    {
        // Script = this;

        // // point text at camera
        // bonusText.transform.LookAt(Camera.main.transform);
        
        // // roate 180 degrees so text is readable
        // bonusText.transform.Rotate(0, 180, 0);

        // bonusText.text = "";
        // //comment

        // // generate bonus person odds
        // if(Random.Range(0f, 1f) < bonusPersonChance)
        // {
        //     isBonusPerson = true;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // // check if bonus is already active (main)
        // if (isBonusPerson && !Camera.main.GetComponent<JustJugglingMain>().bonusActive)
        // {
        //     // check if bonus should be activated
        //     if (Random.Range(0f, 1f) < bonusChance)
        //     {
        //         // generate bonus
        //         string bonus = bonusOptions[Random.Range(0, bonusOptions.Length)];
        //         bonusText.text = bonus;
        //         Debug.Log("Bonus: " + bonusText.text + " activated");
                
        //         // activate bonus
        //         Camera.main.GetComponent<JustJugglingMain>().bonusActive = true;
        //         Camera.main.GetComponent<JustJugglingMain>().bonusText = bonus;

        //         // start timer
        //         StartCoroutine(DeactivateBonus());
        //     }
        // }
    }

    IEnumerator DeactivateBonus()
    {
        yield return new WaitForSeconds(bonusDuration);
        
        // deactivate bonus
        END_BONUS();
    }

    public static void END_BONUS()
    {
        Camera.main.GetComponent<JustJugglingMain>().bonusActive = false;
        Camera.main.GetComponent<JustJugglingMain>().bonusText = "";
    
        // deactivate bonus
        Script.bonusText.text = "";
    }
}
