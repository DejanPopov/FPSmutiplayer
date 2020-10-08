using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class GameManager : MonoBehaviour
{
    //Singleton pattern
    public static GameManager instance;

    public MatchSettings matchSettings;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More then one GameManager in scene!");

        }
        else
        {
            instance = this;
        }
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void UnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    //GUI to see what is going on
     void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerID in players.Keys)
        {
            GUILayout.Label(playerID + " - " + players[playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    #endregion
}
