using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float rotationSpeed = 100f, jumpingForce = 5f;

    private Vector2 inputAxis;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");

        transform.Translate(followTarget.forward * inputAxis.y * speed * Time.deltaTime);
        transform.Translate(followTarget.right * inputAxis.x * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpingForce);
        }

    }

    
}
