using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class HUD : MonoBehaviour
{

    private int Cwave;

 public TextMeshProUGUI hudText;
    

    void Update()
    {
        Cwave=GameManager.WaveCount-1;
        if(GameManager.inWave)
        {
            inwave();
        }
        else if(!GameManager.inWave)
        {
            if(GameManager.End==true)
            {
                End();
            }
            else
            {
                intermission();
            }
                
            
            
        }
        
       

    }

    void inwave()
    {
         hudText.text =
            "Stamina: " + PlayerController.stamina.ToString("F0") +
            "\nDamage Bonus: " + PlayerController.BonusDMG+
            "\nCrit Chance: " + PlayerController.CritChance +
            "\nCrit Mult: " + PlayerController.CritDMG+
            "\nWave: "+ Cwave;
            
    }
    void intermission()
    {
        hudText.text = "Stamina: " + PlayerController.stamina.ToString("F0") +
            "\nDamage Bonus: " + PlayerController.BonusDMG+
            "\nCrit Chance: " + PlayerController.CritChance +
            "\nCrit Mult: " + PlayerController.CritDMG +
            "\nIntermission: "+ GameManager.IntermissionTime.ToString("F0");

    }
    void End()
    {
        hudText.text = "FINISHED";
    }
}

