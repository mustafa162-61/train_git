using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MoreMountains.NiceVibrations;

public class Vibration_Controller : MonoBehaviour
{
   
    public void Light_vib()
    {
       // MMVibrationManager.Haptic(HapticTypes.LightImpact, false, true, this);
    }
    public void Medium_vib()
    {
        // MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true, this);
        Handheld.Vibrate();
    }
    public void Soft_vib()
    {
       // MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
    }
}