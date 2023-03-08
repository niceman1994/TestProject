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
    float Speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        maxTorque = 30;
        rigid.centerOfMass = new Vector3(0, -1, 0);
    }
    
    void Update()
    {
        wheelLF.steerAngle = 15 * Input.GetAxis("Horizontal");
        wheelRF.steerAngle = 15 * Input.GetAxis("Horizontal");
        wheelLB.motorTorque = maxTorque * Input.GetAxis("Vertical");
        wheelRB.motorTorque = maxTorque * Input.GetAxis("Vertical");

        wheelTransformLF.Rotate(wheelLF.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheelTransformRF.Rotate(wheelRF.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheelTransformLB.Rotate(wheelLB.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheelTransformRB.Rotate(-wheelRB.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);

        Move();
    }

	void Move()
	{
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(-Vector3.forward * wheelLB.motorTorque * Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector3.back * wheelRB.motorTorque * Time.deltaTime);
    }
}
