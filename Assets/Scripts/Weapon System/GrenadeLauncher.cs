using UnityEngine;

public class GrenadeLauncher : Gun
{
    public GameObject grenadePrefab; // Prefab for the grenade
    public Transform launchPoint; // Point from which the grenade is launched
    public float launchForce = 20f; // Initial force applied to the grenade

    public override void Shoot()
    {
        // Instantiate the grenade
        GameObject grenade = Instantiate(grenadePrefab, launchPoint.position, launchPoint.rotation);

        // Apply force to the grenade
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(launchPoint.forward * launchForce, ForceMode.Impulse);
        }

        // Play muzzle flash and sound
        muzzleFlash.Play();
        soundController.Play(gunData.shootSound);
    }
}
