using UnityEngine;

public class PouncingEnemy : MonoBehaviour
{
    public float detectionRange = 10f;
    public float pounceSpeed = 10f;
    public float pounceHeight = 3.5f;
    public float cooldownTime = 2f;
    public int damage = 20; 
    private Transform player;
    private Rigidbody rb;
    private bool isPouncing = false;
    private bool hasDealtDamage = false; // Flag to track if damage has been dealt during the current pounce
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null && !isPouncing) 
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position); 

            if (distanceToPlayer <= detectionRange && cooldownTimer <= 0f)
            {
                Pounce();
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isPouncing = false; // Reset pouncing state after cooldown
                hasDealtDamage = false; // Reset damage flag after cooldown
            }
        }
    }

    void Pounce()
    {
        isPouncing = true;
        Debug.Log("Pouncing");

        Vector3 direction = (player.position - transform.position).normalized; // Calculate the direction to pounce towards
        Vector3 pounceVelocity = direction * pounceSpeed + Vector3.up * pounceHeight; // Calculate the pounce velocity

        rb.velocity = pounceVelocity; // Apply the pounce velocity to the rigidbody

        cooldownTimer = cooldownTime; 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasDealtDamage)
        {
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.TakeDamage(damage); 
                hasDealtDamage = true; // Prevent further damage, as sometimes procs twice
            }
            isPouncing = false; // Reset pouncing state 
        }
    }
}