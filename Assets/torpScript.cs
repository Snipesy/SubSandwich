using UnityEngine;
using System.Collections;

public class torpScript : MonoBehaviour
{


    public int damage = 30;
    public float armTime = 2;
    private float initializationTime;
    public string owner;


    // Use this for initialization
    void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D col)
    {
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
            Destroy(gameObject);
            return;
        }

        var hpmanage = col.gameObject.GetComponent<HealthManager>();

        // Check if object has a health manager
        if (hpmanage == null)
        {
            return;
        }

        // hp stuff
        hpmanage.damage(damage);


        Destroy(gameObject);



    }


}
