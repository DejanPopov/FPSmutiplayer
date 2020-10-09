using Mirror;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetupNetwork : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerLane = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

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

            //Disable player graphics for local player
            //Metoda koja zove sama sebe (recursive method)
            SetLayerRucursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerLane));

            //Create playerUI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }

        //Zovemo metodu iz Player klase
        GetComponent<Player>().Setup();
    }

    //This method calls itself (recursive method)
    void SetLayerRucursively (GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRucursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        //When player enters a game
        GameManager.RegisterPlayer(netID, player);
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
        Destroy(playerUIInstance);

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        //Disconnect player when dies 
        GameManager.UnRegisterPlayer(transform.name);
    }

}
