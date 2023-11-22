using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerAnimationEventReceiver : MonoBehaviour
{

    void OnKick()
    {
    }

   
    void OnReverse()
    {
        GetComponentInParent<IKickable>().OnReverse();

    }










}
