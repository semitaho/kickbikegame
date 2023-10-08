using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixer : MonoBehaviour
{

    [SerializeField] private int dragReturnRatio = 2;
    private float maxXAngle;
    private float originalDrag;

    private float targetDrag;

    private Rigidbody rb;

    





    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxXAngle = GetComponentInChildren<KickBikeController>().GetMaxSteeringAngle();
        originalDrag = rb.drag;
        targetDrag = originalDrag;
    }

    private void FixedUpdate()
    {

        transform.localEulerAngles = new Vector3(
            ClampAngle(transform.localEulerAngles.x, -maxXAngle, maxXAngle), 
        transform.localEulerAngles.y, 0);
        FixDrag();      
    }

    private void FixDrag()
    {
        rb.drag = Mathf.Lerp(rb.drag, targetDrag, Time.deltaTime * dragReturnRatio);

    }

    private void CheckOffTheRoad()
    {
        var rayStart = transform.position;
        var hits = Physics.SphereCastAll(rayStart, 1f, Vector3.down);
        foreach (var hit in hits)
        {        
        }

    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 90 || angle > 270)
        {       // if angle in the critic region...
            if (angle > 180) angle -= 360;  // convert all angles to -180..+180
            if (max > 180) max -= 360;
            if (min > 180) min -= 360;
        }
        angle = Mathf.Clamp(angle, min, max);
        if (angle < 0) angle += 360;  // if angle negative, convert to 0..360
        return angle;
    }

    public void ToggleSlowDown(float dragRatio, bool slowDown)
    {
        if (slowDown)
        {
            targetDrag = originalDrag * dragRatio;

        }
        else
        {
            targetDrag = originalDrag;
        }
    }


}
