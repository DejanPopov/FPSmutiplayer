using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    //Synhronizacija d aklijenti budu obavesteni 
    [SyncVar]
    private int currentHealth;

     void Awake()
    {
        SetDefaults();    
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log(transform.name + " now has " + currentHealth + "health!");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
