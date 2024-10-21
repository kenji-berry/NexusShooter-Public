using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float bulletVelocity = 30f;
    public float bulletLifeTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // left click
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            print("SHOOT!");
            FireWeapon();
        }
        
    }

    private void FireWeapon(){
        // GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        // Rigidbody rb = bullet.GetComponent<Rigidbody>();
        // rb.velocity = bulletSpawn.forward * bulletVelocity;
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        Destroy(bullet, bulletLifeTime);
    }
}
