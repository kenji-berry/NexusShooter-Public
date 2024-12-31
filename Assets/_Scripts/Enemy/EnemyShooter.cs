using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : Enemy
{
    public Transform shootPoint;
    public Transform gunPoint;
    public GameObject bulletPrefab;

    [Header("Gun")]
    public Vector3 spread = new Vector3(0.05f, 0.05f, 0.05f);
    public float bulletSpeed = 200f;
    public TrailRenderer bulletTrail;

    void Awake()
    {
        damage = 15;
        attackRange = 20f;
        attackCooldown = 0.5f;
    }

    public override void Attack()
    {
        Vector3 direction = GetDirection();
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, direction, out hit))
        {
            Debug.DrawLine(shootPoint.position, shootPoint.position + direction * 10f, Color.red, 1f);

            HealthController healthController = hit.collider.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.TakeDamage(damage);
            }

            TrailRenderer trail = Instantiate(bulletTrail, gunPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = (player.transform.position - shootPoint.position).normalized;

        direction += new Vector3(
            UnityEngine.Random.Range(-spread.x, spread.x),
            UnityEngine.Random.Range(-spread.y, spread.y),
            UnityEngine.Random.Range(-spread.z, spread.z)
        );
        direction.Normalize();
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0f;
        Vector3 startPosition = trail.transform.position;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;

        Destroy(trail.gameObject, trail.time);
    }
}