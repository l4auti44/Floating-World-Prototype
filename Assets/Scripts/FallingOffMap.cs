using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOffMap : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        other.transform.position = respawnPoint.position;
    }
}
