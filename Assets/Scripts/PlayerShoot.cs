using UnityEngine.Networking;
using UnityEngine;
using Mirror;
using System.Xml.Serialization;

[RequireComponent(typeof(WeaponMnaager))]
public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";
  

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponMnaager weaponManager;

    private void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponMnaager>();

        //Weapon layer on second camera
       //weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        if (currentWeapon.fireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            //Fast firing, machine gun effect
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }


    [Client]
    void Shoot()
    {
        //Check if shooting
        Debug.Log("Shooting!");

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position,
            cam.transform.forward, out hit,
            currentWeapon.range, mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string playerID, int damage)
    {
        Debug.Log(playerID + "has been shot!");
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
    }
}