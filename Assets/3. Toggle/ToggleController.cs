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
		Check = Check ? false : true;

		Anim.SetBool("Check", Check);

		//InventoryObject.SetActive(!Check);
	}

	public bool GetBoolCheck(bool check)
	{
		check = Check;
		return check;
	}
}
