using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;                // Speed of the projectile
    public float lifetime = 5f;              // Lifetime of the projectile
    public float damage = 50;                   // Damage inflicted by the projectile

    private Vector3 direction;               // Direction in which the projectile will move
    public Transform playerTransform;
    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();  
        direction = (playerTransform.position - transform.position).normalized;      
        transform.rotation = Quaternion.FromToRotation(new Vector3(1,0,0), direction);    
    }

    public void Shoot()
    {
        rb.AddForce(rb.transform.right * 10, ForceMode2D.Impulse);
    }
}