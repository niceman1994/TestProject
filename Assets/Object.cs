using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour, Interface
{
	protected string Key;
	protected int Value;
	public GameObject _Object;

	// 순수 가상 함수 Initialize(), Progress(), Release()
	public abstract void Initialize();
	public abstract void Progress();
	public abstract void Release();
}
