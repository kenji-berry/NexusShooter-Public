using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InventoryItemPickUp : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public GameController gameController;

    private float bounceHeight = 0.1f;
    private float bounceSpeed = 3f;
    private Vector3 originalPosition;

    private SphereCollider myCollider;

    private void Awake()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        gameController = FindFirstObjectByType<GameController>();

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
    }

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        float newY = originalPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController controller = other.transform.GetComponent<CharacterController>();
        if (controller != null)
        {
            if (inventoryManager.PickUpItem(gameObject.GetComponent<ItemInstance>()))
            {
                gameController.DisplayPickupMessage(gameObject.GetComponent<ItemInstance>());
            }
        }
    }
}
