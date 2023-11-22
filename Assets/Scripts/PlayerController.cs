using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;
using Cinemachine;
using System;

public class PlayerController : IActivator, IKickable
{

    enum MovingState
    {
        Idling,
        Accelerating,
        Breaking
    }
    [SerializeField] private InputAction accelerateAction;

    [SerializeField] private InputAction turnAction;

    [SerializeField] private InputAction goBackAction;

    [SerializeField] private InputAction quitGameAction;


    [SerializeField] private Transform hipsTransform;

    [SerializeField] private WaypointController roadPath;


    [SerializeField][Range(0, 10)] private float turningAccelerationSpeed = 3;

    [SerializeField][Range(0, 1)] private float kickAccelerationTime = 5;

    [SerializeField] private Slider slider;

    [SerializeField] private float slownessSpeed = 0.5f;

    private KickBikeController kickBikeController;

    private float maxPower = 2f;

    private Animator kickingAnimator;
    private Animator playerAnimator;

    private Rigidbody rb;


    private Transform steeringAxleTransform;

    private float currentHorizontalValue = 0;

    private MovingState movingState = MovingState.Idling;


    float currentPower = 0;
    private void OnEnable()
    {
        EnableInputActions();
        accelerateAction.started += OnAccelerate;
        accelerateAction.performed += OnAccelerate;
        accelerateAction.canceled += OnAccelerate;
        goBackAction.started += OnGoBack;
        goBackAction.performed += OnGoBack;
        goBackAction.canceled += OnGoBack;
        quitGameAction.performed += OnQuit;

    }

    private void EnableInputActions()
    {
        accelerateAction.Enable();
        goBackAction.Enable();
        turnAction.Enable();
        quitGameAction.Enable();
    }

    private void DisableInputActions()
    {

        accelerateAction.Disable();
        goBackAction.Disable();
        turnAction.Disable();
        quitGameAction.Disable();
    }

    private void OnDisable()
    {
        accelerateAction.started -= OnAccelerate;
        accelerateAction.performed -= OnAccelerate;
        accelerateAction.canceled -= OnAccelerate;
        goBackAction.started -= OnGoBack;
        goBackAction.performed -= OnGoBack;
        goBackAction.canceled -= OnGoBack;
        DisableInputActions();
    }
    // Start is called before the first frame update

    private void Awake()
    {
        kickBikeController = transform.GetComponentInChildren<KickBikeController>();
        rb = GetComponent<Rigidbody>();
        UpgradePowerAndSlider(0);

    }
    void Start()
    {
        var animations = GetComponentsInChildren<Animator>(true);
        playerAnimator = animations[0];
        kickingAnimator = animations[1];
        steeringAxleTransform = kickBikeController.GetSteeringAxleTransform();


    }

    static float ScaleClamp(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // Ensure the value is within the original range
        float clampedValue = Mathf.Clamp(value, fromMin, fromMax);

        // Calculate the normalized value within the original range
        float normalizedValue = (clampedValue - fromMin) / (fromMax - fromMin);

        // Scale the normalized value to the target range
        float scaledValue = normalizedValue * (toMax - toMin) + toMin;

        // Ensure the result is within the target range
        return Mathf.Clamp(scaledValue, toMin, toMax);
    }
    private void Update()
    {
        switch (movingState)
        {
            case MovingState.Accelerating:
                currentPower += Time.deltaTime * kickAccelerationTime;
                currentPower = Mathf.Clamp(currentPower, 0, maxPower);
                break;
            case MovingState.Breaking:
                currentPower -= Time.deltaTime * kickAccelerationTime;
                currentPower = Mathf.Max(currentPower, -1);
                break;
            default:
                break;
        }

        var scaled = ScaleClamp(rb.velocity.sqrMagnitude, 0.01f, 30f, 0.5f, 1.5f);
        Debug.Log("rb: " + scaled);
        UpgradePower(scaled);
    }

    private void FixedUpdate()
    {


        var axisValue = turnAction.ReadValue<float>();
        if (axisValue == 1 || axisValue == -1)
        {
            UpgradeHorizontalValue(axisValue);
        }
        else
        {
            currentHorizontalValue = Mathf.Lerp(currentHorizontalValue, 0, Time.deltaTime * 10);
        }


        kickBikeController.Steer(currentHorizontalValue);

    }

    private void UpgradeHorizontalValue(float axisValue)
    {
        currentHorizontalValue = Mathf.Lerp(currentHorizontalValue, axisValue, turningAccelerationSpeed * Time.deltaTime);

    }


    private void UpgradePower(float multiplier)
    {

        slider.value = currentPower;
        kickingAnimator.speed = 1 * multiplier;
        //kickingAnimator.SetFloat("Speeding", multiplier);
        kickBikeController.Boost(currentPower);
    }



    private void LateUpdate()
    {
        Steer(currentHorizontalValue);

    }

    private void OnTriggerEnter(Collider other)
    {
        CheckWaypointCollision(other);
        if (CheckPickableCollision(other)) return;
        if (CheckBombCollision(other)) return;
    }

    private void CheckWaypointCollision(Collider other)
    {
        if (other.gameObject.tag == "Waypoint")
        {
            roadPath.ChangeWaypoint();
        }
    }

    private bool CheckPickableCollision(Collider other)
    {
        if (other.gameObject.tag == "Pickable")
        {
            FindObjectOfType<UIManager>().AddScores(1);
            return true;
        }
        return false;
    }

    private bool CheckBombCollision(Collider other)
    {
        if (other.CompareTag("Bomb"))
        {
            kickingAnimator.enabled = false;
            playerAnimator.enabled = true;
            DisableInputActions();
            return true;
        }
        return false;

    }

    public void AfterHit()
    {
        playerAnimator.enabled = false;
        kickingAnimator.enabled = true;
        EnableInputActions();

    }

    private void Steer(float horizontalValue)
    {
        Rotate(hipsTransform, horizontalValue);

    }

    private void Rotate(Transform transform, float horizontalValue)
    {
        var steeringAngle = kickBikeController.GetMaxSteeringAngle();

        transform.RotateAround(steeringAxleTransform.position, Vector3.up, steeringAngle * horizontalValue);



    }

    public void OnReverse()
    {
        kickBikeController.Revert();
    }



    void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            kickingAnimator.SetTrigger("Kicking");
            kickingAnimator.SetBool("Mirroring", false);
            movingState = MovingState.Accelerating;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            kickingAnimator.SetTrigger("BackToIdle");
            movingState = MovingState.Idling;
        }

    }

    void OnGoBack(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            kickingAnimator.SetTrigger("Kicking");
            kickingAnimator.SetBool("Mirroring", false);
            movingState = MovingState.Breaking;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            kickingAnimator.SetTrigger("BackToIdle");

            movingState = MovingState.Idling;
        }
    }

    private void UpgradePowerAndSlider(float powerValue)
    {
        this.currentPower = powerValue;
        slider.value = powerValue;
    }



    void OnQuit(InputAction.CallbackContext context)
    {

        Debug.Log("QUIRRING..");
        Application.Quit();
    }

}
