using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WheelCollider wheelLF;
    public WheelCollider wheelRF;
    public WheelCollider wheelLB;
    public WheelCollider wheelRB;

    public Transform wheelTransformLF;
    public Transform wheelTransformRF;
    public Transform wheelTransformLB;
    public Transform wheelTransformRB;

    public int maxTorque;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        maxTorque = 30;
        rigid.centerOfMass = new Vector3(0, -1, 0);
    }
    // TODO : 앞뒤 바퀴가 바뀐거같은 추후 수정
    void Update()
    {
        wheelLB.motorTorque = maxTorque * Input.GetAxis("Vertical");
        wheelRB.motorTorque = maxTorque * Input.GetAxis("Vertical");
        wheelLF.steerAngle = 15 * Input.GetAxis("Horizontal");
        wheelRF.steerAngle = 15 * Input.GetAxis("Horizontal");

        wheelTransformLB.Rotate(wheelLF.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheelTransformRF.Rotate(wheelRF.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheelTransformLB.Rotate(wheelLB.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheelTransformRB.Rotate(wheelRB.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
    }
}
