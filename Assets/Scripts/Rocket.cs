using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private AudioSource source;

    private Vector3 initialPos;

    [SerializeField] private float thrustForce = 120f;
    [SerializeField] private float rotationTorque = 25f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Thrust();
        Rotate();
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrustForce);
            if (!source.isPlaying)
            {
                source.Play();
            }

        }
        else
        {
            source.Stop();
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(0, 0, rotationTorque);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(0, 0, -rotationTorque);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                print("Win");
                break;
            default:
                transform.position = initialPos;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.rotation = Quaternion.Euler(Vector3.zero);
                break;
        }
    }
}
