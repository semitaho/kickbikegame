using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableMover : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
{
    }

    

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
        
    }
}
