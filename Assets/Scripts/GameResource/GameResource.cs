using FPS.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.FPSResource
{
    public class GameResource : MonoBehaviour
    {
        public AmmoSO ammoSO;

        private static GameResource instance;
        public static GameResource Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GameResource>("GameResource");
                }
                return instance;
            }
        }
    }
}
