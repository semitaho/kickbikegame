using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBikeController : MonoBehaviour
{

    [SerializeField] private float maxMotorTorque = 50f;


    [SerializeField] private float maxSteeringAngle = 60f;

    [SerializeField] private float slowdownRate = 10f;

    [SerializeField] List<AxleInfo> axleInfos;

    [SerializeField] private float maxBoostTime = 2f;


    private float originalMotorTorque;


    private bool boosting = false;
    private float currentBoostTime = 0f;


    private Fixer fixer;



    // Start is called before the first frame update
    void Start()
    {
        fixer = GetComponentInParent<Fixer>();

    }

    public void Boost(float motorBoost)
    {
        boosting = true;

        foreach (var axleInfo in axleInfos)
        {

            float motor = motorBoost * maxMotorTorque;
            if (axleInfo.motor)
            {
                originalMotorTorque = axleInfo.wheel.motorTorque;
                axleInfo.wheel.motorTorque += motor;
                //currentMotorTorque = motor;

            }
        }
        currentBoostTime = 0;
    }

    public void Revert()
    {
        Boost(-0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (boosting)
        {
            currentBoostTime += Time.deltaTime;
            if (currentBoostTime >= maxBoostTime)
            {
                StopBoosting();
            }
        }
        foreach (var axleInfo in axleInfos)
        {
            Debug.Log("motor torque: " + axleInfo.wheel.motorTorque);
            CheckGroundColliders(axleInfo);
        }
    }

    private void StopBoosting()
    {
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                float currentTorque = axleInfo.wheel.motorTorque;
                float newTorque = Mathf.Lerp(currentTorque, 0f, slowdownRate * Time.deltaTime); // Reduce torque gradually
                axleInfo.wheel.motorTorque = newTorque; //originalMotorTorque; // currentMotorTorque; //MathF.Max(0, currentMotorTorque) - MathF.Max(0, currentRevertTorque);
                ApplyLocalPositionToVisuals(axleInfo.wheel);

            }
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
