using UnityEngine;
using System.Collections;
using UnityEngine.Networking;



public class DebuggingClass : NetworkBehaviour
{




    PlayerController controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
    // Debugging
    [Command]
    public void CmdSpawnObject(GameObject tospawn, Vector3 position, Quaternion rotation)
    {
        if (controller.enableDebugging == false || controller == null)
        {
            return;
        }
        
        var a = (GameObject)Instantiate(tospawn, new Vector3(position.x, position.y, 0), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0.0f, 360.0f)));

        NetworkServer.Spawn(a);
    }


    [Command]
    public void CmdMove(GameObject tospawn, Vector3 position, Quaternion rotation)
    {
        if (controller.enableDebugging == false || controller == null)
        {
            return;
        }

        RpcMoveThis(tospawn, position, rotation);

    }

    // Move
    [ClientRpc]
    private void RpcMoveThis(GameObject toMove, Vector3 position, Quaternion rotation)
    {
        if (toMove == null)
            return;
        transform.position = new Vector3(position.x,position.y,0);
        transform.rotation = rotation;
    }

}
