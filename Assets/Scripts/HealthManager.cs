using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthManager : NetworkBehaviour
{

    public const int maxHealth = 100;

    public RectTransform hpUI;

    // When changed...
    [SyncVar(hook = "OnChangeHealth")]
    public int health = maxHealth;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        return;
    }


    public void damage(int dmg)
    {
        // Checks if not server.
        if (!isServer)
        {
            return;
        }

        health -= dmg;





        Debug.Log("Current HP: " + health + "/" + maxHealth);
    }


    void OnChangeHealth(int health)
    {
        if (hpUI != null)
        {
            hpUI.sizeDelta = new Vector2(0, hpUI.sizeDelta.y);
            

        }
        if (health <= 0)
        {
            CmdDeath();
        }
    }

    [Command]
    void CmdDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
}