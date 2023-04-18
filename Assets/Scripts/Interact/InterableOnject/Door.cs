using FPS.Settings;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
    public class Door : MonoBehaviour
    {
        private Animator animator;
        private bool isOpen = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void ToggleDoor()
        {
            if (!isOpen)
            {
                animator.SetTrigger(Setting.open);
            }
            else
            {
                animator.SetTrigger(Setting.close);
            }
            isOpen = !isOpen;
        }
    }
}
