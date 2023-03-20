using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    void Update()
    {
        ActiveFalse();
    }

    void ActiveFalse()
    {
        if (gameObject.activeInHierarchy == true)
        {
            if (Input.GetKey(KeyCode.Return))
                gameObject.SetActive(false);
        }
    }
}
