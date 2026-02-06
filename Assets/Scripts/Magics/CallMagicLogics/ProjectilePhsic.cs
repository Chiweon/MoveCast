using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectilePhsic : MonoBehaviour
{
    private Rigidbody rb;
    public float objectSpeed;
    public float objectDamage;
    public float limitTime = 2.0f;
    public float timeCounter;
    

    public void Init(float s, float d) 
    { 
        objectSpeed = s; 
        objectDamage = d;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * objectSpeed;
    }

    public void Update()
    {
        timeCounter += Time.deltaTime;
        if(timeCounter >= limitTime)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
