using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
	[SerializeField] private GameObject cameraView;
	[SerializeField] private GameObject cameraPos;
	[SerializeField] private float speed;

	private void LateUpdate()
	{
		gameObject.transform.position = Vector3.Lerp(transform.position, cameraPos.transform.position, Time.deltaTime * speed);
		gameObject.transform.LookAt(cameraView.transform);
	}
}
