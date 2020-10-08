using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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

    /*
    //Test if player dead,disable on compile in Unity
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999999);
        }
    }
    */

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

        //Disable components upon death
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log(transform.name + "is Dead!");

        //Respawn metoda
        StartCoroutine(Respawn());
    }


    private IEnumerator Respawn()
    {
        //Kada player umre,ceka 3 sekundi i krece respawn (preko MatchSettings)
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        //Kada krene respawn,setuje se Default
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log("Player respawn");
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

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

    }
}
 