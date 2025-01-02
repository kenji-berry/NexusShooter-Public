using UnityEngine;

public class EnemyPouncer : Enemy
{
    [SerializeField] private GameObject deathParticlesPrefab; // Add at top with other fields
    public float detectionRange = 10f;
    public float pounceSpeed = 10f;
    public float pounceHeight = 3.5f;
    public float cooldownTime = 2f;
    public float rotationSpeed = 5f;
    private Transform player;
    private Rigidbody rb;
    private bool isPouncing = false;
    private bool hasDealtDamage = false; // Flag to track if damage has been dealt during the current pounce
    private float cooldownTimer = 0f;
    private SoundController soundController2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        soundController2 = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    void Update()
    {
        if (player != null) 
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position); 
            
            // Only rotate to face player during cooldown
            if (cooldownTimer > 0f)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                float targetYRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                // Added Z rotation of 90f to face player
                Quaternion targetRotation = Quaternion.Euler(-90f, targetYRotation, 90f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            if (distanceToPlayer <= detectionRange && cooldownTimer <= 0f && !isPouncing)
            {
                Pounce();
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isPouncing = false;
                hasDealtDamage = false;
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

    public override void Die()
    {
        // Spawn particle effect
        if (deathParticlesPrefab != null)
        {
            GameObject particles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        // Disable mesh renderer to make model disappear
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
        soundController2.Play(soundController2.splat, 0.5f);
        // Delay final destruction
        Destroy(gameObject, 2f);
    }

    public override void Attack()
    {
        return;
    }
}