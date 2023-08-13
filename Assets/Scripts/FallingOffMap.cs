using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOffMap : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = respawnPoint.position;
    }
}
