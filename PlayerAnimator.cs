using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator PlayerAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerAnim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnim.SetBool("inMotion",PlayerController.inMotion);
        
    }
}
