using UnityEngine;

public class GunEquip : MonoBehaviour
{
    public GameObject[] Guns;

    


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("S"))
    {
        GunManager.isPistol=false;
        GunManager.ar=false;
        GunManager.isSniper=true;
        System.Array.ForEach(Guns, Destroy);
       
    }

    if (other.CompareTag("P"))
    {
        GunManager.ar=false;
        GunManager.isSniper=false;
        GunManager.isPistol=true;
        System.Array.ForEach(Guns, Destroy);
        // Code for P
    }

    if (other.CompareTag("A"))
    {
        GunManager.isPistol=false;
        GunManager.isSniper=false;
        GunManager.ar=true;
        System.Array.ForEach(Guns, Destroy);
        // Code for A
    }
}



}
