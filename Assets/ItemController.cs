using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private GameObject Obj;

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position; // 현재 좌표에 클릭한 좌표를 대입

		Debug.Log("OnDrag");
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("OnPointerDown");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("커서가 들어옴");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("커서가 나감");
	}

	public void OnPointerUp(PointerEventData eventData) // 아이템을 드래그해서 슬롯안에 들어가게 하기
	{
		//GameObject Obj = GameObject.Find("SkillSlot");
		float _width = Obj.GetComponent<RectTransform>().rect.width * 0.5f;

		if (eventData.pointerDrag.transform.position.x <= Obj.transform.position.x + _width
			|| eventData.pointerDrag.transform.position.x >=Obj.transform.position.x + _width)
			eventData.pointerDrag.transform.position = new Vector3(Obj.transform.position.x, transform.position.y, transform.position.z);

		Debug.Log("OnPointerUp");
	}

	void Start()
    {
        
    }

    void Update()
    {
		if (Obj != null)
			Obj = GameObject.Find("SkillSlot");
	}
}
