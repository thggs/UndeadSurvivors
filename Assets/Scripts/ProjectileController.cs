using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;                // Speed of the projectile
    public float lifetime = 3f;              // Lifetime of the projectile
    public int damage = 1;                   // Damage inflicted by the projectile

    private Vector3 direction;               // Direction in which the projectile will move
    private Transform playerTransform;

    void Start()
    {
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Destroy(gameObject, lifetime);
        
        direction = (playerTransform.position - transform.position).normalized;

        // Rotate the projectile to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        // Move the projectile in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector3 dir)
    {
        // Set the direction of the projectile
        direction = dir;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        
    }
}