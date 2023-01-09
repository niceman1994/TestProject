using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	static public SliderController Instance = null;

	[SerializeField] private RectTransform TargetUITransform;
	[SerializeField] private RectTransform StartPoint;
	[SerializeField] private RectTransform EndPoint;
	
	public bool MoveCheck = false;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	private void Start()
	{
		TargetUITransform.position = StartPoint.position;

		MoveCheck = false; // 토글 버튼을 누르기 전까지는 움직이지 않았으니 초기 MoveCheck는 false로 설정한다. TargetUI(ScrollView)는 Canvas 바깥쪽에 위치해있음.
	}

	float SetTime()
	{
		float distance = Vector3.Distance(StartPoint.position, EndPoint.position);  // 시작 지점과 끝 지점 사이의 거리를 구함.
		// MoveCheck의 값을 확인해서 true면 객체의 좌표와 끝지점 사이의 값을 Offset에 대입하고, false라면 객체의 좌표와 시작지점 사이의 값을 Offset에 대입
		float Offset = MoveCheck ? Vector3.Distance(TargetUITransform.position, EndPoint.position) : Vector3.Distance(TargetUITransform.position, StartPoint.position);   

		return  1 - (Offset / distance); // 선형 보간을 사용하기 위해(0 ~ 1 사이), 1에서 OffsetPoint/distance의 값을 뺀다.
	}

	public IEnumerator SlideInCoroutine_01() // 안으로 들이는 코루틴 함수
	{
		// 기본 슬라이드
		float time = SetTime();

		while (time <= 1.0f && MoveCheck) // 1초 이하이고 지정해놓은 MoveCheck의 값과 일치할 때
		{
			time += Time.deltaTime; // while문 안에 있는 time에 한 프레임 당 실행되는 시간을 계속 더해준다.
			TargetUITransform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, time); // 시작점과 끝점을 가진 객체에 1초 이하까지 더해지는 time 변수를 사용해 이동시킨다.
			yield return null;
		}
	}

	public IEnumerator SlideOutCoroutine_01() // 바깥으로 내보내는 코루틴 함수
	{
		// 기본 슬라이드
		float time = SetTime();

		while (time <= 1.0f && !MoveCheck) // 1초 이하이고 지정해놓은 MoveCheck의 값과 불일치할 때
		{
			time += Time.deltaTime; // while문 안에 있는 time에 한 프레임 당 실행되는 시간을 계속 더해준다.
			TargetUITransform.position = Vector3.Lerp(EndPoint.position, StartPoint.position, time); // 끝점과 시작점을 가진 객체에 1초 이하까지 더해지는 time 변수를 사용해 이동시킨다.
			yield return null;
		}
	}
}
