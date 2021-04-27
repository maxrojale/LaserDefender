using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Player")]
    [Range(1f,100f)][SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 1000;
    [SerializeField] int maxHealth = 1000;
    [SerializeField] int healthRecoveryRate = 1;
    [SerializeField] float recoveryDelay = 10f;
    [SerializeField] AudioClip deathSFX;
    [Range(0.1f, 10f)][SerializeField] float deathSFXVolume = 1f;

    [Header("Projectile")]
    [Range(1f,100f)][SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] AudioClip projectileSFX;
    [Range(0.1f, 10f)][SerializeField] float projectileSFXVolume = 1f;

    Coroutine firingCoroutine;
    Coroutine recoveryCoroutine; //Check Fire Method


    float xMin, xMax, yMin, yMax;

    void Start()
    {
        SetupMoveBoundaries();
        recoveryCoroutine = StartCoroutine(HealthRecovery());
    } 

    void Update()
    {
        Move();
        Fire();   
    }

        private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           
           firingCoroutine = StartCoroutine(FireContinuously());
            if (recoveryCoroutine != null)
            {
                StopCoroutine(recoveryCoroutine);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
            recoveryCoroutine = StartCoroutine(HealthRecovery());

        }
    }
    IEnumerator HealthRecovery()
    {
        while (true)
        {
            if (health < maxHealth)
            {
                health += healthRecoveryRate;
            }
            yield return new UnityEngine.WaitForSeconds(recoveryDelay);
        }
        
    }

    IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject laser = Instantiate(laserPrefab, new Vector3(transform.position.x - 0.5f, transform.position.y), Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, projectileSpeed);
            AudioSource.PlayClipAtPoint(projectileSFX, Camera.main.transform.position, projectileSFXVolume);
            GameObject laser2 = Instantiate(laserPrefab, new Vector3(transform.position.x + 0.5f, transform.position.y), Quaternion.identity) as GameObject;
            laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, projectileSpeed);
            GameObject laser3 = Instantiate(laserPrefab, new Vector3(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
            laser3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new UnityEngine.WaitForSeconds(projectileFiringPeriod);
        }
     }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPosition = Mathf.Clamp(transform.position.x + deltaX, xMin + padding, xMax - padding);
        var newYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin + padding, yMax - padding);

        transform.position = new Vector2(newXPosition, newYPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
    }

    private void SetupMoveBoundaries()
    {
        Camera gamecamera = Camera.main;
        xMin = gamecamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = gamecamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = gamecamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gamecamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    public int getHealth()
    {
        if (health < 0)
        {
            health = 0;
        }
        return health;
    }
}
