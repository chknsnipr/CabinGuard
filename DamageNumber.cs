using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
     public float floatSpeed = 1f;
    public float lifetime = 0.8f;

    private TextMeshPro text;
    private float timer;
    private Color startColor;
    private Transform cam;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        startColor = text.color;
        cam = Camera.main.transform;
    }

    public void Setup(float damageAmount)
    {
        text.text = damageAmount.ToString("0");
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(startColor.a, 0f, timer / lifetime);
        text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

     void LateUpdate()
     {
         transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
     }
}
