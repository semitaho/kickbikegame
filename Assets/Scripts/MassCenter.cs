using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCenter : MonoBehaviour
{

    [SerializeField] private Rigidbody centerOfMassRigidBody;
    // Start is called before the first frame update

    void Awake()
    {
        centerOfMassRigidBody.centerOfMass = transform.localPosition;
    }


}
