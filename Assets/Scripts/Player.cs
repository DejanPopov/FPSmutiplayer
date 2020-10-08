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

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

     public void Setup()
    {
        //Loop da se iskljuce komponente kada player is dead
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
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
        //Loop da se ukljuce componente
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
    }
}
 