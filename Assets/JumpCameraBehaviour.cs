using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCameraBehaviour : MonoBehaviour
{
    public GameObject player;
    public GameObject rampContainer;

    /**
     * Settings
     */
    public float xDistance;
    public float yDistance;
    public float zDistance;
    public float degrees;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttachToPlayer(GameObject playerIn)
    {
        player = playerIn;
        gameObject.transform.parent = player.transform;
        gameObject.transform.localPosition = new Vector3(xDistance, yDistance, zDistance);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        //gameObject.transform.parent = rampContainer.transform;
    }
}
