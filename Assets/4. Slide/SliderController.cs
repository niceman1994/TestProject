using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	[SerializeField] private GameObject TargetUI;
	[SerializeField] private RectTransform TargetUITransform;
	[SerializeField] private Button ToggleButton;

	[SerializeField] private RectTransform StartPoint;
	[SerializeField] private RectTransform EndPoint;

	[Range(0.01f, 1.0f)]
	[SerializeField] private float time;

	private void Start()
	{
		TargetUITransform = TargetUI.GetComponent<RectTransform>();
		TargetUI.GetComponent<RectTransform>().position = StartPoint.position;
	}

	private void Update()
	{
		
	}

	public void Slide_01()
	{
		StartCoroutine(SliderCoroutine_01());
	}

	IEnumerator SliderCoroutine_01()
	{
		// 기본 슬라이드
		float time = 0;

		while (ToggleButton.animator.GetBool("Check"))
		{
			time += Time.deltaTime;
			TargetUITransform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, time);
			yield return null;
		}
	}

	public void Slide_02()
	{
		
	}

	public void Slide_03()
	{

	}

	public void Slide_04()
	{

	}

	public void Slide_05()
	{

	}

	public void Slide_06()
	{

	}

	public void Slide_07()
	{

	}
}
