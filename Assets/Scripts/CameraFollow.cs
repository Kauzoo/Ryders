using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCam;
    public Transform target;

    public float camXOffset;
    public float camYOffset;
    public float camZOffset;
    public float followDamping;
    public float rotationDamping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        RotateCam();
    }

    private void LateUpdate()
    {
        
    }

    private void FollowPlayer()
    {
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, target.position + target.forward * (-1) * 5, followDamping);
    }

    private void RotateCam()
    {
        mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, target.rotation, 1);
    }
}
