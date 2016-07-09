using UnityEngine;
using System.Collections;

public class CamTrack : MonoBehaviour {

    //Public
    public float trackingSpeed;
    public float defaultDistance = -10;
    public float minSize = 3;
    public float maxSize = 20;
    public Camera cam;
    

    //Private 
    private float camDistance = -10;
    private bool isTracking = false;
    private Transform target = null;
    private bool zoomEnabled = true;


    // Use this for initialization
    void Start () {
        camDistance = defaultDistance;

 	}
	
	// Update is called once per frame
	void Update () {


        if(this.zoomEnabled == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && cam.orthographicSize > minSize)
                cam.orthographicSize -= 1;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && cam.orthographicSize < maxSize)
                cam.orthographicSize += 1;
        }

        
        if (this.isTracking && this.target != null)
        {
            Vector2 newPosition;
            float calc = Vector2.Distance(target.position, transform.position);
            float snapDistance = cam.orthographicSize * .1f;
            if (calc < snapDistance + cam.orthographicSize)
            {
                newPosition = Vector2.Lerp(transform.position, target.position, 
                Time.deltaTime * (trackingSpeed / cam.orthographicSize));
            }
            else
            {
                newPosition = Vector2.Lerp(transform.position, target.position, calc *
                Time.deltaTime * (1f / cam.orthographicSize));
            }
            transform.position = new Vector3(newPosition.x,newPosition.y, camDistance);
   
        }

        // Creates background and keeps it in line

	}

    //Make camera track object.
    //Returns true if successful and false if failed
    //Parameters:
    //  target - game object to be targeted
    //  startTracking - Whether or not to begin tracking immediately
    public bool Track(Transform target, bool startTracking = false)
    {
        if (target == null)
        {
            Debug.LogError("Camera was passed null tracking argument");
            return false;
        }

        this.target = target;
        this.isTracking = true;

        return true;
    }


    //Untrack camera.
    //Returns true if successful
    public bool Untrack()
    {
        this.isTracking = false;
        this.target = null;
        return true;
    }


    //Begin tracking an object if target is assigned
    //Returns true if successful or false if failed.
    public bool StartTracking()
    {
        if (target == null)
            return false;

        this.isTracking = true;
        return true;
    }
}
