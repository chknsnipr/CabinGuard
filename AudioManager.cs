using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour


{ 

   public AudioClip ARFoley;

   public AudioClip PFoley;

   public AudioClip SniperFoley; 


    
    public static bool GunAudio=false;
    public AudioClip GunShot;
    
    public AudioSource AudioHandler;

    public AudioClip EnemyFootsteps;

    public AudioClip SniperShot;

    public AudioClip ReverbShot;

    public AudioClip ARShot;

    public AudioClip CritSound;



    

    

    public AudioClip[] suspenseClips;

    
    public AudioClip IdleAmbience;
    

    public float Clock=0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioHandler.PlayOneShot(IdleAmbience);
        Foley();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Clock+=Time.deltaTime;
        wavestart();
        //enemywalk();
        //enemyalert();
        //idle();
        //prowl();
        ShootAudio();
       
    }
    

    // private void enemyalert()
    // {
    //     if(visualdetection.detection==true && visualdetection.AlertAudioReady==true)
    //     {
    //         AudioHandler.PlayOneShot(EnemyAlerted);
    //     }
    // }

    void idle()
    {

        if(Clock>=30f)
        {
            
        
        int randomIndex = Random.Range(0, suspenseClips.Length);

        AudioHandler.PlayOneShot(suspenseClips[randomIndex]);
        Clock=0f;
        }
    }

    
    void ShootAudio()
    {
        if(GunAudio==true)
        {
            if(GunManager.isPistol)
            {
                AudioHandler.PlayOneShot(GunShot);
                GunAudio=false;
                
            }
            if(GunManager.ar)
            {
                AudioHandler.PlayOneShot(ARShot);
                GunAudio=false;
                
            }
            if(GunManager.isSniper)
            {
                AudioHandler.PlayOneShot(SniperShot);
                GunAudio=false;
            }

            // PlayerController.shot=false;
            
        }
    }

    void wavestart()
    {
        if(GameManager.PlayWaveAudio==true)
        {
            int randomIndex = Random.Range(0, suspenseClips.Length);

        AudioHandler.PlayOneShot(suspenseClips[randomIndex]);
        GameManager.PlayWaveAudio=false;
            
        }
    }

    public void criticalhitaudio()
    {
       
            AudioHandler.PlayOneShot(CritSound);
            Debug.Log("crit");
        
    }

    void Foley()
    {
        if(GunManager.isSniper)
        {
            AudioHandler.PlayOneShot(SniperFoley);
        }
        else if(GunManager.isPistol)
        {
            AudioHandler.PlayOneShot(PFoley);
        }
        else if(GunManager.ar)
        {
            AudioHandler.PlayOneShot(ARFoley);
        }
    }
    void reverbaudio()
    {
        
    }

    
}

