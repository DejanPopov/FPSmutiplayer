using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetupNetwork : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    Camera sceneCamera;

    private void Start()
    { 
        if (!isLocalPlayer)
        {
            DisableComponents();
            AsignRemoteLayer();
        }
        else   
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                //Disable camera if we are local player
                Camera.main.gameObject.SetActive(false);
            }
        }

        RegisterPlayer();
    }

    void RegisterPlayer()
    {
        //Dodajemo igracu ID tako sto cemo string konkatenirati sa komponentom
        string ID = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = ID;
    }

    void AsignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    //If we want to re-enable camera
    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

}
