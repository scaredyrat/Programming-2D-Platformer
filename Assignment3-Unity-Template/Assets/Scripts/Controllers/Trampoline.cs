using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounceApexHeight;
    public float timeToApexInSeconds;

    private float bouncePower;

    void Awake()
    {
        bouncePower = 2 * bounceApexHeight / timeToApexInSeconds;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Call TrampolineBounce() to take care of velocity
            PlayerController playerScript = col.gameObject.GetComponent<PlayerController>();
            playerScript.TrampolineBounce(bouncePower);
        }
    }
}
