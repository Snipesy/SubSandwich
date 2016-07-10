using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{


    public NetworkManager net;



    NetworkClient myClient;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        bool canHost = true;

        if (canHost)
        {
            // Start server online (No local client)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                net.StartServer();
            }


            // Start Server and local client
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                net.StartHost();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            net.StartClient();
        }

        if (net.IsClientConnected())
        {

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                net.StopHost();
            }

        }


    }

    public void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected!");
    }
}
