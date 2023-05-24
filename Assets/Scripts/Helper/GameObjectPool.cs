using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Helper
{
	public class GameObjectPool : SingletonMonoBehaviour<GameObjectPool>
	{
		private Dictionary<int, Queue<Component>> objectPool;
		[SerializeField] private List<GameObjectInfo> gameObjectInfoList;

		protected override void Awake()
		{
			base.Awake();
			objectPool = new Dictionary<int, Queue<Component>>();

			//创建对象池
			if (gameObjectInfoList.Count > 0)
			{
				foreach (var gameObjectInfo in gameObjectInfoList)
				{
					if (!objectPool.ContainsKey(gameObjectInfo.prefab.GetInstanceID()))
					{
						//初始化，设置对象池物体的Anchor
						Queue<Component> poolObjectQueue = new Queue<Component>();
						GameObject poolObjectParent = new GameObject(gameObjectInfo.prefab.name + "Anchor");
						poolObjectParent.transform.parent = transform;

						//创建对象池对象
						for (int i = 0; i < gameObjectInfo.count; i++)
						{
							GameObject poolObject = Instantiate(gameObjectInfo.prefab, poolObjectParent.transform);
							poolObject.SetActive(false);
							poolObjectQueue.Enqueue(poolObject.GetComponent(gameObjectInfo.reuseComponentName));
						}
						objectPool.Add(gameObjectInfo.prefab.GetInstanceID(), poolObjectQueue);
					}
				}
			}
		}

		/// <summary>
		/// 重用物体
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="position"></param>
		/// <param name="rotation"></param>
		/// <returns></returns>
		public Component GetComponentFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			int poolKey = prefab.GetInstanceID();

			//如果存在此物体
			if (objectPool.ContainsKey(poolKey))
			{
				Component objectFromPool = objectPool[poolKey].Dequeue();
				objectPool[poolKey].Enqueue(objectFromPool);
				//重置
				if (objectFromPool.gameObject.activeSelf == true)
				{
					objectFromPool.gameObject.SetActive(false);
				}
				//重置位置和旋转
				objectFromPool.transform.position = position;
				objectFromPool.transform.rotation = rotation;

				return objectFromPool;
			}
			return null;
		}
	}

	[Serializable]
	public struct GameObjectInfo
	{
		public GameObject prefab;
		public int count;
		public string reuseComponentName;
	}

}
