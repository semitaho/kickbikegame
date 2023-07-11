using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;
using System;

public class PlayerController : IActivator, IKickable
{
    [SerializeField] private InputAction accelerateAction;

    [SerializeField] private InputAction turnAction;

    [SerializeField] private InputAction goBackAction;

    [SerializeField] private Transform hipsTransform;


    [SerializeField][Range(0, 3)] private float turningAccelerationSpeed = 3;

    [SerializeField][Range(0, 3)] private float kickAccelerationTime = 5;

    [SerializeField] private Slider slider;


    private KickBikeController kickBikeController;

    private Animator kickingAnimator;
    private Animator playerAnimator;


    private Transform steeringAxleTransform;

    private float currentHorizontalValue = 0;





    bool powering = false;

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

    }

    private void EnableInputActions()
    {
        accelerateAction.Enable();
        goBackAction.Enable();
        turnAction.Enable();
    }

    private void DisableInputActions()
    {
        accelerateAction.Disable();
        goBackAction.Disable();
        turnAction.Disable();
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
        UpgradePowerAndSlider(0);

    }
    void Start()
    {
        var animations = GetComponentsInChildren<Animator>(true);
        playerAnimator = animations[0];
        kickingAnimator = animations[1];
        steeringAxleTransform = kickBikeController.GetSteeringAxleTransform();


    }

    private void FixedUpdate()
    {

        if (powering)
        {
            UpgradePower();
        }
        var axisValue = turnAction.ReadValue<float>();

        var turningSpeed = Time.deltaTime * turningAccelerationSpeed;


        if (axisValue == 1 || axisValue == -1)
        {
            UpgradeHorizontalValue(axisValue, turningSpeed);
        }
        else
        {
            currentHorizontalValue = Mathf.Lerp(currentHorizontalValue, 0, Time.deltaTime * 10);
        }



        kickBikeController.Steer(currentHorizontalValue);

    }

    private void UpgradeHorizontalValue(float axisValue, float turningSpeed)
    {
        currentHorizontalValue = Mathf.Lerp(currentHorizontalValue, axisValue, turningSpeed);

    }

    private void UpgradePower()
    {
        currentPower += Time.deltaTime * kickAccelerationTime;
        currentPower = Mathf.Clamp01(currentPower);
        slider.value = currentPower;
    }



    private void LateUpdate()
    {
        Steer(currentHorizontalValue);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (CheckPickableCollision(other)) return;
        if (CheckBombCollision(other)) return;
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
        //transform.localEulerAngles = new Vector3( transform.localEulerAngles.x,-maxXAngle, maxXAngle), transform.localEulerAngles.y, 0);

        //virtualCamera.Follow = transform;
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

    public void OnKick()
    {
        kickBikeController.Boost(currentPower);
        currentPower = 0;
    }

    public void OnReverse()
    {
        Debug.Log("potku taakse!");
        kickBikeController.Revert();
    }

    public void OnKickEnded()
    {

    }


    void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            powering = true;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            powering = false;
            slider.value = 0;
            kickingAnimator.SetTrigger("Kicking");
        }

    }

    private void UpgradePowerAndSlider(float powerValue)
    {
        this.currentPower = powerValue;
        slider.value = powerValue;
    }

    void OnGoBack(InputAction.CallbackContext context)
    {
        kickingAnimator.SetTrigger("Reversing");
    }

}
