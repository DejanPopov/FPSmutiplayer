using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    //To see if player is dead
    [SyncVar]
    private bool isDeadB = false; 
    public bool isDead
    {
        get { return isDeadB; }
        protected set { isDeadB = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    //Synhronizacija da klijenti budu obavesteni 
    [SyncVar]
    private int currentHealth;

     void Awake()
    {
        SetDefaults();    
    }


    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isDead)
            return;
        currentHealth -= amount;

        Debug.Log(transform.name + " now has " + currentHealth + "health!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log(transform.name + "is Dead!");

    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
 