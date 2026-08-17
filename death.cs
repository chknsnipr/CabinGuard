using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class death : MonoBehaviour
{
    private float clock=0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clock+=Time.deltaTime;
        if(clock>=2f)
        {
            clock=0f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
