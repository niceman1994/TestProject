using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
	[SerializeField] private RectTransform SlotList;
	[SerializeField] private GameObject Parent;
	private Animator animator;

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position; // 현재 좌표에 클릭한 좌표를 대입

		Debug.Log("OnDrag");
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		transform.SetParent(Parent.transform);
		transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

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
		SlotinItem();

		Debug.Log("OnPointerUp");
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("OnEndDrag");
	}

	void Start()
    {
		animator = GetComponent<Animator>();
	}

	private void SlotinItem()
	{
		for (int i = 0; i < SlotList.transform.childCount - 1; ++i)
		{
			for (int j = 0; j < SlotList.transform.GetChild(i).childCount; ++j)
			{
				Vector3 _position = SlotList.transform.GetChild(i).GetChild(j).position;
				float Distance = Vector3.Distance(SlotList.transform.GetChild(i).GetChild(j).position, transform.position);

				if (Distance <= 50.0f)
				{
					transform.SetParent(SlotList.transform.GetChild(i).GetChild(j));
					transform.position = _position;
				}
				else
				{
					transform.localScale = new Vector3(1, 1, 1);
				}
			}
		}
	}
}
