using Unity.Mathematics;
using UnityEngine;


public class mouselook : MonoBehaviour
{
    public float sensitivity=100f;
    public float xRotation=0f;

    public Transform playerBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX=Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation-=mouseY;
        xRotation= Mathf.Clamp(xRotation,-90f,90f);

        transform.localRotation= Quaternion.Euler(xRotation,0f,0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
