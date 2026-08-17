using MimicSpace;
using Unity.VisualScripting;
using UnityEngine;

public class shootmechanic : MonoBehaviour
{
    public static bool isInAim=false;

    

    

    private Camera cam;
    private bool lastShotState = false;

    public static bool Crit;
    public AudioManager AM;

    

    void Start()
    {
        cam = Camera.main;
       
    }

    void Update()
    {
        HandleShootRaycast();
    }

    void HandleShootRaycast()
    {
        
        if (PlayerController.shot == true && lastShotState == false)
        {
            FireRaycast();
        }
        lastShotState = PlayerController.shot;
    }

    void FireRaycast()
    {
        RaycastHit hit;
        


        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("enemy") | hit.collider.CompareTag("MimicWeak") | hit.collider.CompareTag("DEATH"))
            {
                visualdetection enemy = hit.collider.GetComponentInParent<visualdetection>();
                if (enemy != null)
                {
                    float roll = UnityEngine.Random.value;
                    Crit = roll < PlayerController.CritChance;
                    if(Crit)
                    {
                        AM.criticalhitaudio();

                        enemy.damagemech((GunManager.DMG + PlayerController.BonusDMG)* PlayerController.CritDMG);
                    }
                    else
                    {
                        enemy.damagemech(GunManager.DMG + PlayerController.BonusDMG);
                    }

                    
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("enemy"))
        {
            isInAim=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("enemy"))
        {
            isInAim=false;
        }
    }
}