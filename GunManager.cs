using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static bool isSniper=false;
    public GameObject Sniper;

    public static bool ar=true;

    public GameObject AR;

    public static bool isPistol=false;

    public GameObject Pistol;

    public static float CurrentLoad;

    public static float DMG;

    public static bool Notpicked=true;

    private bool lastPistol, lastSniper, lastAr;
    private bool initialized=false;

    void Update()
    {
        bool changed = (isPistol != lastPistol) || (isSniper != lastSniper) || (ar != lastAr);

        if (!initialized || changed)
        {
            if (isPistol==true)
            {
                Pistol.SetActive(true);
                Sniper.SetActive(false);
                AR.SetActive(false);
                ar=false;
                isSniper=false;
                CurrentLoad=0.6f;
                DMG=29f;
                PlayerController.CritDMG=2f;
                PlayerController.recoilKickback=.5f;
                muzzleflash.FlashIntensity=10f;
            }
            else if (isSniper==true)
            {
                Pistol.SetActive(false);
                Sniper.SetActive(true);
                AR.SetActive(false);
                isPistol=false;
                ar=false;
                CurrentLoad=3f;
                DMG=140f;
                PlayerController.CritChance=0f;
                PlayerController.recoilKickback=2f;
                muzzleflash.FlashIntensity=15f;
            }
            else if (ar==true)
            {
                Pistol.SetActive(false);
                Sniper.SetActive(false);
                AR.SetActive(true);
                CurrentLoad=.27f;
                DMG=15f;
                isPistol=false;
                isSniper=false;
                PlayerController.CritChance=0.25f;
                PlayerController.recoilKickback=.3f;
                muzzleflash.FlashIntensity=6f;
            }

            Notpicked=false;

            lastPistol=isPistol;
            lastSniper=isSniper;
            lastAr=ar;
            initialized=true;
        }
    }
}