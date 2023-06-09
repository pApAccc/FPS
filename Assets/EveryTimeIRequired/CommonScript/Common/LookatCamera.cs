using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI看向Camera
/// </summary>
namespace Common
{
    public class LookatCamera : MonoBehaviour
    {
        private enum Mode
        {
            CameraForward,
            CameraForwardInterval
        }

        [SerializeField] private Mode mode;
        private void LateUpdate()
        {
            switch (mode)
            {
                case Mode.CameraForward:
                    transform.forward = Camera.main.transform.forward;
                    break;
                case Mode.CameraForwardInterval:
                    transform.forward = -Camera.main.transform.forward;
                    break;
            }


        }
    }
}
