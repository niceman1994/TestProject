using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleController : MonoBehaviour
{
    private Animator Anim;
	[SerializeField] private bool Check;

	//[SerializeField] private GameObject InventoryObject;

	private void Awake()
	{
        Anim = GetComponent<Animator>();
    }

	private void Start()
	{
		Check = false;
		Anim.SetBool("Check", Check);
	}

	public void ToggleButton()
	{
		SliderController.Instance.MoveCheck = SliderController.Instance.MoveCheck ? false : true;

		Anim.SetBool("Check", SliderController.Instance.MoveCheck);

		if (SliderController.Instance.MoveCheck)
			StartCoroutine(SliderController.Instance.SlideInCoroutine_01());
		else
			StartCoroutine(SliderController.Instance.SlideOutCoroutine_01());

		//InventoryObject.SetActive(!Check);
	}
}
