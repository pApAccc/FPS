using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace Common
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance;
		//是否由get方法初始化
		private static bool initializedByGet = false;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();
					initializedByGet = true;
				}
				return instance;
			}

			private set
			{

			}
		}

		protected virtual void Awake()
		{
			if (Instance == null)
			{
				Instance = this as T;
			}
			else if (initializedByGet)
			{
				initializedByGet = false;
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}

