using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {


    public NetworkManager hello;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        bool z = Input.GetButtonDown("Fire1");

        if (z)
        {
            return;
        }


    }
}
