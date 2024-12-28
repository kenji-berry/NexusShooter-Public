using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 100;
    
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
}