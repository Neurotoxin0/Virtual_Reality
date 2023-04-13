using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// on PlayerFramework

public class HapticController : MonoBehaviour
{
    public SteamVR_Action_Vibration ControllerHaptic;

    public void Pulse(float duration, float frequency, float amplitude, Hand hand)
    {
        //Debug.Log("Pulse: " + duration + ", " + frequency + ", " + amplitude + ", " + hand + ", " + hand.name);
        if (hand == null) return;
        SteamVR_Input_Sources ctrl = (hand.name == "LeftHand") ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
        ControllerHaptic.Execute(0, duration, frequency, amplitude, ctrl);
    }

    public void ShortPulse(Hand hand, int _) 
    { 
        Pulse(0.5f, 150, 0.5f, hand); 
    }

    public void LongPulse(Hand hand, int _)
    {
        Pulse(1.5f, 150, 0.5f, hand); 
    }
}
