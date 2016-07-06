using UnityEngine;
using System.Collections;

public class camTrack : MonoBehaviour {

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
    private 

    // Use this for initialization
    void Start () {
        camDistance = defaultDistance;

 	}
	
	// Update is called once per frame
	void Update () {


        if(self.rotateEnabled == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && cam.orthographicSize > minSize)
                cam.orthographicSize -= 1;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && cam.orthographicSize < maxSize);
                cam.orthographicSize += 1;
        }

        
        if (this.isTracking && this.target != null)
        {


            var newPosition = Vector2.Lerp(transform.position, target.position, Time.deltaTime * (trackingSpeed/cam.orthographicSize));
            transform.position = new Vector3(newPosition.x,newPosition.y, camDistance);
   
        }

        // Creates background and keeps it in line

	}


    public bool Track(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Camera was passed null tracking argument");
            return false;
        }

        this.target = target;
        this.is_tracking = true;

    }

    public bool Untrack()
    {
        this.isTracking = false;
        this.target = null;
    }
}
