using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playgame()
    {
        if(GunManager.Notpicked==false)
        {
            SceneManager.LoadScene("SampleScene");
        }
        
    }

    public void exit()
    {
        Application.Quit();
    }

    public void startsniper()
    {
        GunManager.Notpicked=false;
       GunManager.isPistol=false;
        GunManager.ar=false;
        GunManager.isSniper=true;
    }

    public void startpistol()
    {
        GunManager.Notpicked=false;
        GunManager.ar=false;
        GunManager.isSniper=false;
        GunManager.isPistol=true;
    }

    public void startar()
    {
        GunManager.Notpicked=false;
        GunManager.isPistol=false;
        GunManager.isSniper=false;
        GunManager.ar=true;
    }
}
