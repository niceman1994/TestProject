using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
	[SerializeField] private GameObject[] Camera = new GameObject[2];
	[SerializeField] private GameObject cameraView;

	private void Update()
	{
		ViewConversion();
	}

	void ViewConversion()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (Camera[0].activeInHierarchy == true)
			{
				Camera[0].SetActive(false);
				Camera[1].SetActive(true);
			}
			else if (Camera[1].activeInHierarchy == true)
			{
				Camera[1].SetActive(false);
				Camera[0].SetActive(true);
			}
		}
	}
}
