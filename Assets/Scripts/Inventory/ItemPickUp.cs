using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public Gun prefab;

    public float PickUpRadius = 1f;

    private float bounceHeight = 0.1f;
    private float bounceSpeed = 3f;
    private Vector3 originalPosition;

    private SphereCollider myCollider;

    public WeaponsManager weaponsManager;

    private void Awake(){
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        /* Rotate and bob item up and down
         */
        transform.Rotate(0, 40 * Time.deltaTime, 0);
        float newY = originalPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }

    private void OnTriggerEnter(Collider other) {
        CharacterController controller = other.transform.GetComponent<CharacterController>();
        if (controller != null)
        {
            if (weaponsManager.AddGun(prefab)) Destroy(gameObject);
        }
    }
}
