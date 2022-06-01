using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookupTables : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum LayerMasks
    {
        Default = 0, TransparentFX = 1, IgnoreRaycast = 2, Ground = 3, Water = 4, UI = 5, PostProcessing = 6, NotInReflection = 7
    }
}
