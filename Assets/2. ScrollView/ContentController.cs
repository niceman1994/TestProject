using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentController : MonoBehaviour
{
    [SerializeField] private GameObject HorizontalListPrefab;
    [SerializeField] private GameObject AddHorizontalButton;

    private RectTransform ContentTransform;

	private void Awake()
	{
        ContentTransform = GetComponent<RectTransform>();
	}

	void Start()
    {
        AddHorizontalList(0.0f, 120.0f);

        for (int i = 0; i < 5; ++i)
            AddHorizontalList(0.0f, 115.0f); // Content에 있는 Grid Layout Group 설정에서 spacing을 15로 설정해놓았으므로 슬롯의 높이 100에 15를 더한 값을 넣는다.
    }

    private void AddHorizontalList(float _x, float _y)
	{
        GameObject Obj = Instantiate(HorizontalListPrefab); // 한 개의 프리팹을 클론 생성

        ContentTransform.sizeDelta = new Vector2(
                ContentTransform.sizeDelta.x + _x,
                ContentTransform.sizeDelta.y + _y);

        Obj.transform.SetParent(transform);

        AddHorizontalButton.transform.SetAsLastSibling(); // SetAsLastSibling() 해당 자식 객체를 제일 마지막으로 이동시킴. 
    }

    public void HorizontalButton()
	{
        AddHorizontalList(0.0f, 115.0f);
    }
}
