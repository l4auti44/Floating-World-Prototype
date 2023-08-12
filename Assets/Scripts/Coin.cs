using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(StudioEventEmitter))]
public class Coin : MonoBehaviour
{
    private StudioEventEmitter emmiter;

    private void Start()
    {
        emmiter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.Rcoinapproach, this.gameObject);
        emmiter.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.coinCollected, transform.position);
        emmiter.Stop();
        Destroy(gameObject);

    }
}
