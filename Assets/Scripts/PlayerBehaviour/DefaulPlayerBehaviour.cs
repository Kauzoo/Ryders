using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the default case for an ExtremeGear.
/// All movement is calculated here. The character only contributes stats, not custom code.
/// </summary>
public class DefaulPlayerBehaviour : MonoBehaviour
{
    public GameObject InputModule;

    /**
     * MOVEMENT VARS
     */
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// MasterMethod for Movement. Here the player is actually moved.
    /// </summary>
    public virtual void MasterMove()
    {

    }

    public virtual void Drift()
    {

    }

    public virtual void Turn()
    {

    }

    public virtual void Move()
    {

    }

    public virtual void Accelerate()
    {

    }

    public virtual void Breake()
    {

    }

    public virtual void Jump()
    {

    }

}
