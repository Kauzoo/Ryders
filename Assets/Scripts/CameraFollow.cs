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

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //offset = target.transform.position - mainCam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //FollowPlayer();
        //RotateCam();
    }

    private void FixedUpdate()
    {
        //FollowPlayer();
    }

    private void LateUpdate()
    {
        //RotateCam();
    }

    private void FollowPlayer()
    {
        Vector3 nCamPos = new Vector3(target.position.x + camXOffset, target.position.y + camYOffset, target.position.z + camZOffset);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, nCamPos, 0.5f);
        //mainCam.transform.localRotation = Quaternion.
    }

    private void RotateCam()
    {
        float currentAngle = mainCam.transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * rotationDamping);

        Vector3 nCamPos = new Vector3(target.position.x + camXOffset, target.position.y + camYOffset, target.position.z + camZOffset);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * offset);

        //mainCam.transform.LookAt(target.transform, target.up);
    }
}
