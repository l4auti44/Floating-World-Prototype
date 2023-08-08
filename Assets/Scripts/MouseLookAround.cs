using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    public Vector2 turn;
    public float sensitivity = .5f;
    public Vector3 deltaMove;
    public float speed = 1;
    [SerializeField] private Transform player;
    private float offsetY = 3f;
    private float inputX, inputY;
    private void Awake()
    {
        offsetY = transform.position.y;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        /*
        inputX = Input.GetAxis("Mouse X");
        inputY = Input.GetAxis("Mouse Y");

        turn.y += inputY * sensitivity;
        turn.x += inputX * sensitivity;
        
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
        */
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, offsetY, player.position.z);
    }
}
