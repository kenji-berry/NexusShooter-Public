using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    void Start()
    {
        // Destroy this object after 2 seconds
        Destroy(gameObject, 2f);
    }
}