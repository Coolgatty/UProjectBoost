using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    private float alpha;
    private float offsetFactor;

    private Vector3 startingPos;

    [SerializeField] float offsetSpeed;
    [SerializeField] float alphaOffset;
    [SerializeField] float alphaFactor = 1.0f;
    [SerializeField] Vector3 offsetVector;

    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        offsetFactor = Mathf.Sin(alpha) * alphaFactor + alphaOffset;
        alpha += Mathf.PI / 180f * offsetSpeed * Time.deltaTime;
        transform.position = startingPos + offsetVector * offsetFactor;
    }
}
