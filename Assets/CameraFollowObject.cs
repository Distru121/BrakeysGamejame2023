using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour //THIS SCRIPT IS USED ONLY FOR THE CAMERA IN THE CREDITS!!! IT FOLLOWS THE PLAYER PUPPET.
{
    public Rigidbody2D object_rb;
    public Transform objectPosition;
    public Vector3 offset;
    public Camera thisCamera;
    public float cameraDefaultSize;
    public float damping;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera.orthographicSize = cameraDefaultSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(objectPosition.position.y <= 0 && objectPosition.position.y > -380)
        {
        Vector3 movePos = objectPosition.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, damping * Time.fixedDeltaTime);

        //zooms out when increasing momentum
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, cameraDefaultSize + (Mathf.Max(0, object_rb.velocity.magnitude - 6.54f)*0.25f), (damping*0.75f) * Time.fixedDeltaTime);
        }
    }
}
