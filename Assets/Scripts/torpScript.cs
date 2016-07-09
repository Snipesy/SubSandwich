using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class torpScript : NetworkBehaviour
{


    public int damage = 30;
    public float armTime = 2;
    private float initializationTime;

    [HideInInspector]
    public string owner;

    public GameObject dudFX;
    public GameObject hitFX;


    // Use this for initialization
    void Start()
    {
        // Records Start Time
        initializationTime = Time.timeSinceLevelLoad;

    }

    // Update is called once per frame
    void Update()
    {
    }

    // Called whenever an object ENTERS a collision trigger.
    void OnTriggerEnter2D(Collider2D col)
    {
        // Return if not the server
        if (!isServer)
        {
            return;
        }

        // Checks if the object is a valid collision via tag.
        if (col.tag != "Player" && col.tag != "CanHit")
            return;

        // Arm Time Logic and anti FF
        if (Time.timeSinceLevelLoad - initializationTime <= armTime)
        {
            var x = col.gameObject.GetComponent<PlayerController>();
            if (x != null)
            {
                if (x.nameNet == owner)
                    return;
            }

            SpawnFX(dudFX);

            Destroy(gameObject);
            return;
        }

        // Assigns Healthmanager to variable
        var hpmanage = col.gameObject.GetComponent<HealthManager>();

        // Check if object has a health manager. Returns if null
        if (hpmanage == null)
        {
            return;
        }

        // Call damage method in HealthManager
        hpmanage.Damage(damage);
        SpawnFX(hitFX);

        Destroy(gameObject);



    }

    
    public void SpawnFX(GameObject fx)
    {
        var fx2 = (GameObject)Instantiate(
            fx,
            gameObject.transform.position,
            Quaternion.identity);

        NetworkServer.Spawn(fx2);

    }


}
