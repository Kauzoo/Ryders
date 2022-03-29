using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [System.Serializable]
    public class LinearMovement
    {
        public float Acceleration;
        public float CruisingSpeed;
        public float BoostSpeed;
    }

    [System.Serializable]
    public class Turning
    {
        public float TurnAcceleration;
        public float TurnMaxRadius;
        public float TurnSpeedLoss;
    }

    [System.Serializable]
    public class Drift
    {

    }

    [System.Serializable]
    public class Misc
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
