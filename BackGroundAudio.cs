using UnityEngine;

public class BackGroundAudio : MonoBehaviour
{
    public AudioSource BGSource;

    public AudioClip BG;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BGSource.clip=BG;

        
    }

    // Update is called once per frame
    void Update()
    {
    
        BGSource.loop=true;
        BGSource.Play();
        
    }
}
