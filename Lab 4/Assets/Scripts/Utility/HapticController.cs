using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HapticController : MonoBehaviour
{
    public SteamVR_Action_Vibration ControllerHaptic;

    public void Pulse(float duration, float frequency, float amplitude, HandController controller)
    {
        SteamVR_Input_Sources ctrl = (controller.name == "Controller (left)") ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
        ControllerHaptic.Execute(0, duration, frequency, amplitude, ctrl);
    }
}
