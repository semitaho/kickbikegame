using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBikeController : MonoBehaviour
{

    [SerializeField] private float maxMotorTorque = 50f;


    [SerializeField] private float maxSteeringAngle = 60f;


    [SerializeField] List<AxleInfo> axleInfos;


    private readonly float maxspeed = 20f;


    private float currentPower = 0f;

    private Rigidbody playerRigidbody;


    private Fixer fixer;





    // Start is called before the first frame update
    void Start()
    {
        fixer = GetComponentInParent<Fixer>();
        playerRigidbody = GetComponentInParent<Rigidbody>();

    }

    public void Boost(float power)
    {
        currentPower = power;

       
    }

    public void Revert()
    {
        foreach (var axleInfo in axleInfos)
        {

            if (axleInfo.motor)
            {

                axleInfo.wheel.motorTorque = 0;
                //currentMotorTorque = motor;

            }
        }
        Boost(-0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

 foreach (var axleInfo in axleInfos)
        {

            float motor = currentPower * maxMotorTorque;
            if (axleInfo.motor)
            {
                axleInfo.wheel.motorTorque = motor;
            }
        }

        foreach (var axleInfo in axleInfos)
        {
            CheckGroundColliders(axleInfo);
        }
    }

    private void CheckGroundColliders(AxleInfo axleInfo)
    {
        WheelHit hit;
        if (axleInfo.wheel.GetGroundHit(out hit))
        {
            if (CheckSlowdownCollider(hit)) return;
            if (CheckRampCollider(hit)) return;
            if (CheckRataalue(hit)) return;
        }

    }

    private bool CheckSlowdownCollider(WheelHit hit)
    {
        var slowDown = hit.collider.GetComponent<SlowDown>();
        if (slowDown != null)
        {
            slowDown.Hitting(gameObject);
            var slowdownRatio = slowDown.GetSlowDownRatio();
            fixer.ToggleSlowDown(slowdownRatio, true);
            return true;
        }

        fixer.ToggleSlowDown(0f, false);
        return false;


    }

    private bool CheckRampCollider(WheelHit hit)
    {
        if (hit.collider.CompareTag("Ramp"))
        {
            fixer.ToggleSlowDown(0.2f, true);
            return true;
        }

        fixer.ToggleSlowDown(0f, false);
        return false;
    }

    private bool CheckRataalue(WheelHit hit)
    {
        if (hit.collider.CompareTag("Terrain"))
        {
            fixer.ToggleSlowDown(2f, true);
            return true;
        }

        fixer.ToggleSlowDown(0f, false);
        return false;
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;

    }


    public void Steer(float currentHorizontalValue)
    {

        var childTransform = transform.GetChild(0);
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.steeringObject != null)
            {
                var steeringSpeedFactor = GetSteeringSpeedFactor();

                var endAngle = currentHorizontalValue * maxSteeringAngle * steeringSpeedFactor; //* inversed;
                axleInfo.steeringObject.localEulerAngles = new Vector3(0, endAngle, 0);
                axleInfo.wheel.steerAngle = endAngle;

            }
        }
    }
    private float GetSteeringSpeedFactor()
    {
        var clampedValue = Mathf.Clamp(playerRigidbody.velocity.magnitude / maxspeed, 0.2f, 0.8f);
        return 1 - clampedValue;
    }


    public float GetMaxSteeringAngle()
    {
        return maxSteeringAngle;
    }

    public Transform GetSteeringAxleTransform()
    {
        return axleInfos[1].steeringObject.transform;
    }


}

[System.Serializable]
class AxleInfo
{
    [SerializeField] public WheelCollider wheel;
    public bool motor;

    public bool breaking;
    [SerializeField] public Transform steeringObject;

}
