using UnityEngine;

public class muzzleflash : MonoBehaviour
{
   [SerializeField] private Light MuzzleLight;

   [SerializeField] private GameObject lightobj;
    public static float FlashIntensity;

    bool lastlightstate=false;

    private float FlashTimer=0.0f;

    bool FlashStatus=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Flash();
        if(PlayerController.shot==true && !lastlightstate)
        {
            lightobj.SetActive(true);
            FlashStatus=true;

            
            lastlightstate=true;
        }
        if(PlayerController.shot==false && lastlightstate)
        {
            
            lastlightstate=false;
        }
        
    }

    void Flash()
    {
        if(FlashStatus==true)
        {
            FlashTimer+=Time.deltaTime;
            if(FlashTimer>=0.25f)
            {
                FlashStatus=false;
                FlashTimer=0;
                lightobj.SetActive(false);
            }

        }
        
    }
}
