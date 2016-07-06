using UnityEngine;
using System.Collections;

public class CamTrack : MonoBehaviour {

    public Transform target;
    public float trackingSpeed = 1;
    public float defaultDistance = -10;
    public float minSize = 3;
    public float maxSize = 20;
    private float camDistance = -10;
    public Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        camDistance = defaultDistance;

    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && cam.orthographicSize > minSize)
            {
                cam.orthographicSize -= 1;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && cam.orthographicSize < maxSize)
            {
                cam.orthographicSize += 1;
            }

            var newPosition = Vector2.Lerp(transform.position, target.position, Time.deltaTime * (trackingSpeed / cam.orthographicSize));
            transform.position = new Vector3(newPosition.x, newPosition.y, camDistance);

        }

        // Creates background and keeps it in line


    }
}
