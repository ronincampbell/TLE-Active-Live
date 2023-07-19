using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTurret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 10f;
    public Vector2 direction;
    private float fireTimer;
    private bool isFiring;
    public bool isActive;

    private void Start()
    {
        // Ensure the direction vector is normalized
        isActive = true;
        direction.Normalize();
    }

    // Update is called once per frame
    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate && !isFiring && isActive)
        {
            this.GetComponent<Animator>().SetTrigger("FireTurret");
            isFiring = true;
            fireTimer = 0f;
        }
    }

    private void Fire()
    {
        // Create a new projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the direction of the projectile
        OrbProjectile projectileScript = projectile.GetComponent<OrbProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
        isFiring = false;
    }

    public void WaitForAnim()
    {
        Fire();
    }

    public void TurretReset()
    {
        //Debug.Log("Turret reset");
        isFiring = false;
        fireTimer = 0f;
        isActive = true;
        this.GetComponent<Animator>().Play("OrbTurretIdle");

        // Find all projectiles
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        // Destroy each projectile
        foreach(GameObject OrbProjectile in projectiles)
        {
            Destroy(OrbProjectile);
        }
    }
}
