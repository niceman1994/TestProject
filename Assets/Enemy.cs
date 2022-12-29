using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Object
{
	// 추상 클래스의 함수를 상속 받아서(override) 재정의
	public override void Initialize()
	{
		// 부모 클래스에서 선언된 변수를 사용할때는 base.를 붙여줘야한다.
		base.Value = 10;
		base.Key = "Enemy";

		Debug.Log(base.Key + " : " + base.Key);
	}

	public override void Progress()
	{
		
	}

	public override void Release()
	{
		
	}
}
