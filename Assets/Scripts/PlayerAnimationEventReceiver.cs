using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerAnimationEventReceiver : MonoBehaviour
{

    void OnKick()
    {
        GetComponentInParent<IKickable>().OnKick();
    }

    void OnKickEnded()
    {
        GetComponentInParent<IKickable>().OnKickEnded();
    }

    void OnReverse()
    {
        GetComponentInParent<IKickable>().OnReverse();

    }










}
