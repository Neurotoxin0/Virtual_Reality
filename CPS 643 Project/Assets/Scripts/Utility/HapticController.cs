using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HapticController : MonoBehaviour
{
    public SteamVR_Action_Vibration ControllerHaptic;

    public void Pulse(float duration, float frequency, float amplitude, Hand hand)
    {
        SteamVR_Input_Sources ctrl = (hand.name == "LeftHand") ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
        ControllerHaptic.Execute(0, duration, frequency, amplitude, ctrl);
    }
}
