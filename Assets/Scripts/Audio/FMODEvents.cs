using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Coin SFX")]
    [field: SerializeField] public EventReference coinCollected { get; private set; }

    public static FMODEvents instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one FMOD Events instance in this scene.");
        }
        instance = this;
    }
}
