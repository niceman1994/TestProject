using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotController : MonoBehaviour
{
    [SerializeField] private GameObject SkillSlotPrefab;

    void Start()
    {
        for (int i = 0; i < 6; ++i)
        {
            GameObject Obj = Instantiate(SkillSlotPrefab);
            Obj.transform.SetParent(transform);
            Obj.name = "SkillSlot";
        }
    }
}
