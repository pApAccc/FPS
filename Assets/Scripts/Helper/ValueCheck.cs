using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Helper
{
    public static class ValueCheck
    {
        /// <summary>
        /// 检测数组是不是空的，是不是有空值
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="array"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckIEnumable(Object thisObject, IEnumerable array, string name)
        {
            int index = 0;
            int count = 0;
            bool error = false;

            if (array == null)
            {
                Debug.Log($"在{thisObject.name}中{name}是空的");

                error = true;
                return error;
            }

            foreach (var item in array)
            {
                if (item == null)
                {
                    Debug.Log($"在{thisObject.name}中，{name}的第{index}个是空的");
                    count++;
                    error = true;
                }
                else
                {
                    count++;
                }
                index++;
            }

            if (count == 0)
            {
                Debug.Log($"在{thisObject.name}中{name}数组中没有元素");
                error = true;
            }

            return error;
        }
    }
}
