using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetupNetwork : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    private void Start()
    {
        //Ovde proveravamo da li kontrolisemo igraca.Ako ga ne kontrolisemo onda ce on da ugasi sve 
        //komponente (u smislu da se prikljucio drugi igrac,da se ne kopiraju potezi)
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
    }
}
