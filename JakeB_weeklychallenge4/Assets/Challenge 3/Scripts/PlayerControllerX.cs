﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    private float bounceForce = 5f;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        playerRb = GetComponent<Rigidbody>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
        Bounds();
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;

            // Stop the background scrolling
            RepeatBackgroundX[] backgroundScripts = FindObjectsOfType<RepeatBackgroundX>();
            foreach (RepeatBackgroundX background in backgroundScripts) {
                background.moveSpeed = 0;
            }

            Debug.Log("Game Over!");
            Destroy(other.gameObject);
            Destroy(gameObject, 1f);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
    }


    void Bounds() {
        if (transform.position.y >= 14) {
            transform.position = new Vector3(transform.position.x, 14, transform.position.z);
        }
        if (transform.position.y <= 1.5f) {
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
    }
}
