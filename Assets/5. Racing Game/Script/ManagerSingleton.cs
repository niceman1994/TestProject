using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static object _Lock = new object(); // ���� ������ ���� ������ ����ó���� ���� ����
	private static T instance;

	public static T Instance
	{
		get
		{
			lock (_Lock)
			{
				if (instance == null)
				{
					GameObject obj;
					obj = GameObject.Find(typeof(T).Name); // ���ӿ�����Ʈ�� �̸����� ã�Ƽ� �ִ´�.

					if (obj == null)
					{
						obj = new GameObject(typeof(T).Name);
						instance = obj.AddComponent<T>(); // ��� ã�� ������Ʈ�� ������Ʈ �߰�
					}
					else
					{
						instance = obj.GetComponent<T>(); // ������Ʈ�� ���� ��� �ش� ������Ʈ�� ������Ʈ�� �����´�.
					}
				}
				return instance;
			}
		}
	}

	public void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}