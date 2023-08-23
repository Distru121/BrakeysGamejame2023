using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public CharacterController2D playerController;
    public Transform playerPosition;
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
        Vector3 movePos = playerPosition.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, damping * Time.fixedDeltaTime);

        //zooms out when increasing momentum
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, cameraDefaultSize + (Mathf.Max(0, playerController.momentum - 6.54f)*0.25f), (damping*0.75f) * Time.fixedDeltaTime);
    }
}
