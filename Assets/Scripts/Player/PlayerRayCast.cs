using System;
using UnityEngine;

/// <summary>
/// 玩家射线
/// </summary>

namespace FPS.Core
{
    public class PlayerRayCast : MonoBehaviour
    {
        public event EventHandler<OnInterableChangedEventArgs> OnInterableChanged;
        //射线打中的物体
        private InterableObject interable;
        private IInterable interableInterface;

        [SerializeField] private Transform cameraRoot;
        [SerializeField] private float rayCastDistance = 3;
        [SerializeField] private LayerMask layerMask;

        private void Start()
        {
            GameInput.Instance.OnInteract += GameInput_OnInteract;
        }

        private void GameInput_OnInteract(object sender, EventArgs e)
        {
            interableInterface?.Interact();
        }

        private void Update()
        {
            //打中物体
            if (Physics.Raycast(cameraRoot.position, cameraRoot.forward, out RaycastHit hit, rayCastDistance, layerMask))
            {
                InterableObject hitInterable = hit.transform.GetComponent<InterableObject>();
                interableInterface = hit.transform.GetComponent<IInterable>();

                //打中新物体
                if (interable != hitInterable)
                {
                    interable = hitInterable;
                    OnInterableChanged?.Invoke(this, new OnInterableChangedEventArgs
                    {
                        message = interable.message,
                        isHit = true
                    });
                }
            }
            //没打中物体
            else
            {
                if (interable != null || interableInterface != null)
                {
                    interable = null;
                    interableInterface = null;

                    OnInterableChanged?.Invoke(this, new OnInterableChangedEventArgs
                    {
                        message = "",
                        isHit = false
                    });
                }
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(cameraRoot.position, cameraRoot.position + cameraRoot.forward * rayCastDistance);
        }
    }

    public class OnInterableChangedEventArgs : EventArgs
    {
        public string message;
        public bool isHit;
    }

}