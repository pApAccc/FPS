using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GameManager : ITest
    {
        public int Print()
        {
            Debug.Log("GameManager");
            return 10;
        }

        public void TestFunc()
        {
            Debug.Log("GameManager TestFunc");
        }
    }
}
