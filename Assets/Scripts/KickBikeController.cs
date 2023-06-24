using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBikeController : MonoBehaviour
{

    [SerializeField] private float maxMotorTorque = 50f;


    [SerializeField] private float maxSteeringAngle = 60f;

    [SerializeField] private float brakeForce = 10f;

    [SerializeField] private float revertTorque = 2000f;


    [SerializeField] List<AxleInfo> axleInfos;

    private float currentMotorTorque = 0f;

    private float currentRevertTorque = 0f;


    private Fixer fixer;



    // Start is called before the first frame update
    void Start()
    {
        fixer = GetComponentInParent<Fixer>();

    }

    public void Boost(float motorBoost)
    {
        foreach (var axleInfo in axleInfos)
        {

            float motor = motorBoost * maxMotorTorque;
            if (axleInfo.motor)
            {
                currentMotorTorque = motor;

            }
        }
    }

    public void Revert()
    {
        currentMotorTorque = 0;
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                currentRevertTorque = revertTorque;

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                currentMotorTorque -= (Time.deltaTime * brakeForce);
                currentRevertTorque -= (Time.deltaTime * brakeForce);
                axleInfo.wheel.motorTorque = MathF.Max(0, currentMotorTorque) - MathF.Max(0, currentRevertTorque);
                ApplyLocalPositionToVisuals(axleInfo.wheel);

            }
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

                var endAngle = currentHorizontalValue * maxSteeringAngle;
                axleInfo.steeringObject.localEulerAngles = new Vector3(0, endAngle, 0);
                axleInfo.wheel.steerAngle = endAngle;

            }
        }
    }


    public void SteerWithAngle(float angleValue)
    {
        var childTransform = transform.GetChild(0);
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.steeringObject != null)
            {

                var normalizedAngle = angleValue - maxSteeringAngle * Mathf.Floor((maxSteeringAngle + 180f) / maxSteeringAngle);
                axleInfo.steeringObject.localEulerAngles = new Vector3(0, normalizedAngle, 0);
                axleInfo.wheel.steerAngle = normalizedAngle;

            }
        }
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
