using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private GameObject SkillList;
	[SerializeField] private List<RectTransform> Slots;

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
		Slots.Clear();

		for (int i = 0; i < SkillList.transform.childCount; ++i)
		{
			for (int j = 0; j < SkillList.transform.GetChild(i).childCount; ++j)
				Slots.Add(SkillList.transform.GetChild(i).GetChild(j).GetComponent<RectTransform>());
		}

		foreach(RectTransform element in Slots)
        {
			float _width = element.rect.width;

			if (element.transform.position.x + _width >= transform.position.x ||
				element.transform.position.x - _width <= transform.position.x)
				transform.position = new Vector2(element.transform.position.x, transform.position.y);
        }

		Debug.Log("OnPointerUp");
	}

    void Update()
    {
		
	}
}
