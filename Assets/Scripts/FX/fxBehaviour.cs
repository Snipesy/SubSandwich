using UnityEngine;
using System.Collections;

public class fxBehaviour : MonoBehaviour
{


    private AudioSource source;

    public AudioClip fxclip;

    public float delay = 0;



    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();

        source.clip = fxclip;
        source.PlayDelayed(delay);

    }

    // Update is called once per frame
    void Update()
    {

        if (source.isPlaying == false)
            Destroy(gameObject);
    }
}
