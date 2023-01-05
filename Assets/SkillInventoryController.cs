using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInventoryController : MonoBehaviour
{
    [SerializeField] private GameObject HorizontalListPrefab;

    void Start()
    {
        for (int i = 0; i < 5; ++i)
		{
            GameObject Obj = Instantiate(HorizontalListPrefab);
            Obj.transform.SetParent(transform);
            Obj.name = "HorizontalList";
        }
    }
}
