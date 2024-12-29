using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private float playerDamageMultiplier = 1f; 
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        AdjustEnemyHealth();
    }

    private void AdjustEnemyHealth()
    {
        EnemyHealthController[] enemies = FindObjectsByType<EnemyHealthController>(FindObjectsSortMode.None);
        foreach (EnemyHealthController enemy in enemies)
        {
            enemy.maxHealth = enemyHealth;
            enemy.currentHealth = enemyHealth;
        }
    }

    public float GetDamageMultiplier()
    {
        return playerDamageMultiplier;
    }
}