using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IsVoiceOverOn : MonoBehaviour {

#if UNITY_IPHONE
    [DllImport ("__Internal")]
    private static extern bool _isVoiceOverOn();
#endif

    public static bool isVoiceOverOn() {
        bool isOn = false; 

#if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isOn = _isVoiceOverOn();
        }
#endif

        return isOn;
    } 
}
