using UnityEngine;

public class NewWave : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CleanVars()
    {
        PlayerController.CritDMG=1.5f;
        PlayerController.CritChance=0.1f;
        PlayerController.StaminaLimit=100f;
        PlayerController.BonusDMG=0f;
        GameManager.WaveCount=1;
        GameManager.inWave=false;
        GameManager.IntermissionTime=0f;
        GunManager.isSniper=false;
        GunManager.isPistol=false;
        GunManager.ar=false;
        GunManager.Notpicked=true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.End=false;
        GameManager.inCabin=false;

    }
}
