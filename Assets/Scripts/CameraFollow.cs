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
    public float camDistance;
    public float camXAngle;
    public float camZAngle;
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
        Vector3 targetPosition = new Vector3(target.position.x + camXOffset, target.position.y + camYOffset, target.position.z + camZOffset);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, targetPosition + target.forward * (-1) * camDistance, followDamping);
    }

    private void RotateCam()
    {
        Vector3 targetRotation = new Vector3(camXAngle, target.rotation.eulerAngles.y, camZAngle);
        mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, Quaternion.Euler(targetRotation), 0.1f);
    }
}
