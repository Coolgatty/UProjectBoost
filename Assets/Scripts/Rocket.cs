using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * 75);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(0, 0, 25);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(0, 0, -25);
        }
    }
}
