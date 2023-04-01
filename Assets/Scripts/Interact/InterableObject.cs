using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可交互物体，同时修改UI元素
/// </summary>

namespace FPS.Core
{
    public abstract class InterableObject : MonoBehaviour, IInterable
    {
        public string message;
        public abstract void Interact();
    }
}


