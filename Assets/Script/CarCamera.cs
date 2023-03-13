using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
	[SerializeField] private GameObject[] haveCamera = new GameObject[2];
	[SerializeField] private GameObject cameraView;

	private void Update()
	{
		ViewConversion();
	}

	void ViewConversion()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (haveCamera[0].activeInHierarchy == true)
			{
				haveCamera[0].SetActive(false);
				haveCamera[1].SetActive(true);
			}
			else if (haveCamera[1].activeInHierarchy == true)
			{
				haveCamera[1].SetActive(false);
				haveCamera[0].SetActive(true);
			}
		}
	}
}
