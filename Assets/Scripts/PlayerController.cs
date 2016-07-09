using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System;

public class PlayerController : NetworkBehaviour
{
    // Sprite used for client's ship
    public Sprite localSprite;

    // Default torpedo
    public GameObject dumbTorp;

    // Transforms for front and rear of ship
    public Transform torpSpawn;
    public Transform back;

    // Torpedo variables
    public int reloadSpeed = 300;
    public float TorpSpeed = 20;

    [SyncVar]
    private int ammo = 8;
    public int maxAmmo = 8;


    // Network Identifier
    [SyncVar]
    public string nameNet;
    public NetworkIdentity netPlay;

    // Physics and movement
    public float linearDrag;
    public float angularDrag;
    private Rigidbody2D rb;

    public bool enableDebugging = true;

    public GameObject target;


    // Misc
    private int flux;

    private DebuggingClass debug;





    // Constructor For All
    void Start()
    {
        // Assigns rigidbody for torque
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = linearDrag;
        rb.angularDrag = angularDrag;

        // Assigns Network Identity to variable
        netPlay = GetComponent<NetworkIdentity>();

        // Checks if Network Identity is valid
        if (netPlay != null)
        {
            // Assigns name to nameNet and changes object name
            nameNet = netPlay.netId.ToString();

            // Assigns name to gameobject. This really only works for the host and is more for refrence in editor.
            gameObject.name = "Player " + nameNet;
        }

        debug = GetComponent<DebuggingClass>();

    }

    // Local Only Constructor
    public override void OnStartLocalPlayer()
    {

        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = localSprite;


        Camera.main.GetComponent<CamTrack>().Track(gameObject.transform);
    }

    // Called once every frame
    void Update()
    {
        // Returns if not a local player
        if (!isLocalPlayer)
        {
            return;
        }

        // Base Torpedo Fire Logic

        bool fireDown = Input.GetButtonDown("Fire1");
        bool fireHeld = Input.GetButton("Fire1");

        if (ammo > 0)
        {
            if (fireDown)
            {
                ammo--;
                CmdFireTorp();
            }
            
        }

        if (enableDebugging == true && debug != null)
        {
            if (Input.GetButtonDown("Jump") && target != null)
            {
                debug.CmdSpawnObject(target, Camera.main.ScreenToWorldPoint(Input.mousePosition), UnityEngine.Random.rotation);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                debug.CmdMove(gameObject, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.rotation);
            }
        }

    }

    // Called once per physics update (0.02s)
    [ClientCallback]
    void FixedUpdate()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        // Sanity check for a rigidbody.
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                Debug.LogError("No rigid body for " + gameObject.name + ". ");
                return;
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


        // Passes information to the torpedo. Mostly for collision logic.
        var script = torpA.GetComponent<torpScript>();
        script.owner = nameNet;

        // Add velocity to the bullet
        torpA.GetComponent<Rigidbody2D>().velocity = torpA.transform.up * TorpSpeed;

        // This will instantiate the object on the clients
        NetworkServer.Spawn(torpA);

        // Destroy the torp after 2 seconds.
        Destroy(torpA, 20.0f);
    }


    // Coroutine for ammo regen
    IEnumerator ammoRegen()
    {

        // Flux addition to reloadSpeed.
        // The more torpedos being reloaded, the slower the reload speed is.

        flux += reloadSpeed / 10;

        for (int a = 0; a < reloadSpeed; a++)
        {
            yield return null;
        }

        flux -= reloadSpeed / 10;

        ammo++;

        // Sanity Check
        if (ammo >= maxAmmo)
        {
            ammo = maxAmmo;
            flux = 0;
        }


    }





}