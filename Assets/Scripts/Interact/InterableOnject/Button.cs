using UnityEngine;
/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
    public class Button : InterableObject
    {
        [SerializeField] private Door door;
        public override void Interact()
        {
            door.ToggleDoor();
        }
    }
}
