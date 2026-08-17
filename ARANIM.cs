using UnityEngine;

public class ARANIM : MonoBehaviour
{

    [SerializeField] private Animator ARANIMATOR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ARANIMATOR.SetBool("isShooting",PlayerController.shot);
    }
}
