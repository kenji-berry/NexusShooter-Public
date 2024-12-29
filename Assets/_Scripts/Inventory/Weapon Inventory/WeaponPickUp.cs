using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class WeaponPickUp : MonoBehaviour
{
    public Gun prefab;

    public float PickUpRadius = 1f;

    private SphereCollider myCollider;

    public WeaponsManager weaponsManager;

    private void Awake(){
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    void Update()
    {
        transform.Rotate(0, 40 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other) {
        CharacterController controller = other.transform.GetComponent<CharacterController>();
        if (controller != null)
        {
            if (weaponsManager.AddGun(prefab)) Destroy(gameObject);
        }
    }
}
