using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Header("Wall Settings")]
    public float wallHealth = 20f;
    public GameObject brokenWallPrefab;

    public void TakeDamage(float damageAmount)
    {
        wallHealth -= damageAmount;
        if (wallHealth <= 0f)
        {
            BreakWall();
        }
    }

    private void BreakWall()
    {
        if (brokenWallPrefab != null)
        {
            Instantiate(brokenWallPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}