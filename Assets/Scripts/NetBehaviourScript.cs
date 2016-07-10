using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetBehaviourScript : NetworkManager
{
    public GameObject playuh;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }



    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player;

        Debug.Log("Connection from: " + conn.address + " with ID: " + conn.connectionId);

        if (GetStartPosition() == null)
        {
            Debug.LogWarning("No start position! Spawning at 0 vector!");
            player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(), Quaternion.identity);

        }
        else
        {
            player = (GameObject)GameObject.Instantiate(playerPrefab, GetStartPosition().position, GetStartPosition().rotation);
        }


        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }




}
