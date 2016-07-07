﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthManager : NetworkBehaviour
{

    public const int maxHealth = 100;

    // When changed...
    [SyncVar]
    public int health = maxHealth;


    // Use this for initialization
    void Start()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }


    public void Damage(int dmg)
    {
        // Checks if not server. Since health is a SyncVar.
        
        health -= dmg;

        Debug.Log("Current HP: " + health + "/" + maxHealth);

        OnChangeHealth();

    }


    void OnChangeHealth()
    {
        if (health <= 0)
        {
            CmdDeath();
        }
    }




    [Command]
    void CmdDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
}