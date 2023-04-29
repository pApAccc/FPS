using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{
    public class EnemyMovePathRoot : MonoBehaviour
    {
        private List<Vector3> path;

        private void Awake()
        {
            path = new List<Vector3>();

            for (int i = 0; i < transform.childCount; i++)
            {
                path.Add(transform.GetChild(i).position);
            }
        }

        public List<Vector3> GetPath() => path;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.DrawLine(transform.GetChild(i).position,
                                transform.GetChild(i + 1).position);
            }
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position,
                               transform.GetChild(0).position);
        }
    }

}
