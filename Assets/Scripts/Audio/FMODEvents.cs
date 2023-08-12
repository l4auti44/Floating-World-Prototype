using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }


    [field: Header("Coin SFX")]
    [field: SerializeField] public EventReference coinCollected { get; private set; }

    [field: Header("Rcollected")]
    [field: SerializeField] public EventReference Rcollected { get; private set; }
    
    [field: Header("Rfootsteps")]
    [field: SerializeField] public EventReference Rfootsteps { get; private set; }

    [field: Header("Rcoinapproach")]
    [field: SerializeField] public EventReference Rcoinapproach { get; private set; }
    
    [field: Header("Rambience")]
    [field: SerializeField] public EventReference Rambience { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one FMOD Events instance in this scene.");
        }
        instance = this;
    }
}
