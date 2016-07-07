using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System;

public class PlayerController : NetworkBehaviour
{
    public Sprite localSprite;
    public GameObject dumbTorp;

    public Transform torpSpawn;
    public Transform back;



    public int reloadSpeed = 300;
    public float TorpSpeed = 20;

    [SyncVar]
    private int ammo = 8;
    public int maxAmmo = 8;

    [SyncVar]
    public string nameNet;

    public NetworkIdentity netPlay;






    public float linearDrag;
    public float angularDrag;
    private Rigidbody2D rb;

    private int flux;




    // Constructor when first active.
    void Start()
    {
        // Assigns rigidbody
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = linearDrag;
        rb.angularDrag = angularDrag;


        netPlay = GetComponent<NetworkIdentity>();

        if (netPlay != null)
        {
            nameNet = netPlay.netId.ToString();
            gameObject.name = "Player " + nameNet;
        }


    }

    // Local Only Constructor
    public override void OnStartLocalPlayer()
    {

        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = localSprite;

        // Temporary Name Changer

        Camera.main.GetComponent<CamTrack>().Track(gameObject.transform);
    }


    void Update()
    {
        // Returns if not a local player
        if (!isLocalPlayer)
        {
            return;
        }

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


    void FixedUpdate()
    {

        if (!isLocalPlayer)
        {
            return;
        }


        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                throw new Exception("No rigid body for " + gameObject.name + ". ");
            }
        }

        rb.drag = linearDrag * rb.velocity.magnitude + .2f;
        rb.angularDrag = (linearDrag * linearDrag / (rb.drag + 1)) * angularDrag * Mathf.Abs(rb.angularVelocity + .2f);

        if (Input.GetAxis("Vertical") > 0)
        {
            applyForce(back, 1);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            applyForce(back, -.3f);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddTorque((-.01f * linearDrag), ForceMode2D.Impulse);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddTorque((.01f * linearDrag), ForceMode2D.Impulse);
        }

    }

    void applyForce(Transform t, float power)
    {
        Vector2 direction = transform.position - t.position;
        rb.AddForceAtPosition(direction.normalized * power, t.position);
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
        
        var script = torpA.GetComponent<torpScript>();
        script.owner = nameNet;

        torpA.GetComponent<Rigidbody2D>().velocity = torpA.transform.up * TorpSpeed;
  

        NetworkServer.Spawn(torpA);

        // Destroy the bullet after 2 seconds.
        Destroy(torpA, 20.0f);
    }

    IEnumerator ammoRegen()
    {
        flux += reloadSpeed / 10;

        for (int a = 0; a < reloadSpeed; a++)
        {
            yield return null;
        }

        flux -= reloadSpeed / 10;

        ammo++;

        if (ammo >= maxAmmo)
        {
            ammo = maxAmmo;
            flux = 0;
        }
    }

  
}