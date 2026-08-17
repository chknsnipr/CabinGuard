using MimicSpace;
using UnityEngine;

public class MimicWeak : MonoBehaviour
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject weakMimic = GameObject.FindGameObjectWithTag("MimicWeak");

if (weakMimic == null)
{
    Movement.heal=false;
    SelfDestruct();
}
void SelfDestruct()
{
    // TODO: play death animation, VFX, sound, drop loot, etc. before this fires
    Destroy(gameObject);
}
        
    }
}
