using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour
{
    public Sprite localSprite;
    public GameObject dumbTorp;
    public Transform torpSpawn;
    public int reloadSpeed = 300;
    public float TorpSpeed = 20;

    [SyncVar]
    private int ammo = 8;



    // Constructor when first active
    void start()
    {
       
    }

    // Local Only Constructor
    public override void OnStartLocalPlayer()
    {

        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = localSprite;
        //var sr = GetComponent<SpriteRenderer>();
        //sr.sprite = localSprite;
    }


    void Update()
    {
        // Returns if not a local player
        if (!isLocalPlayer)
        {
            return;
        }
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, 0, x);
        transform.Translate(0, z, 0);



        bool fireDown = Input.GetButtonDown("Fire1");
        bool fireHeld = Input.GetButton("Fire1");

        if (ammo != 0)
        {


            int a = 0;


            if (fireDown)
            {
                ammo--;
                CmdFireTorp();
            }
            else if (fireHeld)
            {
                a++;
                if (a >= 50)
                {
                    ammo--;
                    CmdFireTorp();
                    a = 0;
                }
            }
            else
            {
                a = 0;
            }
        }



    }

    // Client calls but server controls
    [Command]
    void CmdFireTorp()
    {
        StartCoroutine("ammoRegen");
        GameObject torpA;

        // Create the Bullet from the Bullet Prefab
        torpA = (GameObject)Instantiate(
            dumbTorp,
            torpSpawn.position,
            torpSpawn.rotation);

        // Add velocity to the bullet
        torpA.GetComponent<Rigidbody2D>().AddForce(transform.up * TorpSpeed, ForceMode2D.Force);

        NetworkServer.Spawn(torpA);

        // Destroy the bullet after 2 seconds
        Destroy(torpA, 20.0f);
    }

    IEnumerator ammoRegen()
    {

        for (int a = 0; a < reloadSpeed; a++)
        {
            yield return null;
        }
    
        ammo++;
    }
}