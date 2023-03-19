using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private RectTransform Arrow;
    [SerializeField] float MinArrowAngle = 0;
    [SerializeField] float MaxArrowAngle = -300f;
    [SerializeField] private Text speedText;

    void Start()
    {
        speedText.text = "000";
    }
    
    void Update()
    {
        if (GameManager.Instance.Speed < 10.0f)
            speedText.text = "00" + GameManager.Instance.Speed.ToString();
        else if (GameManager.Instance.Speed < 100.0f)
            speedText.text = "0" + GameManager.Instance.Speed.ToString();
        else if (GameManager.Instance.Speed >= 100.0f && GameManager.Instance.Speed < 300.0f)
            speedText.text = GameManager.Instance.Speed.ToString();

        UpdateArrow();
    }

    void UpdateArrow()
	{
        var procent = GameManager.Instance.Speed / MaxArrowAngle;
        var angle = (MaxArrowAngle - MinArrowAngle) * procent;

        if (Arrow.rotation.y >= -300.0f && Arrow.rotation.y <= 0.0f)
            Arrow.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }
}
