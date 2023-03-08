using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Transform tireTransformFL;
	public Transform tireTransformFR;
	public Transform tireTransformRL;
	public Transform tireTransformRR;

	public WheelCollider colliderFR;
	public WheelCollider colliderFL;
	public WheelCollider colliderRR;
	public WheelCollider colliderRL;

	public Transform wheelTransformFL;
	public Transform wheelTransformFR;
	public Transform wheelTransformRL;
	public Transform wheelTransformRR;

	public int maxTorque;

	private float prevSteerAngle;
	private float Speed;
	private Rigidbody rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	void Start()
	{
		maxTorque = 30;
		Speed = 50.0f;
		rigid.centerOfMass = new Vector3(0, -1, 0); // 무게중심이 높으면 차가 쉽게 전복된다
	}

	void FixedUpdate()
	{
		Move();
	}

	void Update()
	{
		WheelRotate();
		prevSteerAngle = colliderFR.steerAngle;
	}

	void Move()
	{
		colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * Speed;
		colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * Speed;

		colliderFR.steerAngle = 17 * Input.GetAxis("Horizontal");
		colliderFL.steerAngle = 17 * Input.GetAxis("Horizontal");

		wheelTransformFL.Rotate(-colliderFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		wheelTransformFR.Rotate(colliderFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		wheelTransformRL.Rotate(-colliderRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		wheelTransformRR.Rotate(colliderRR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
	}

	void WheelRotate()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
			tireTransformFL.Rotate(Vector3.up, colliderFL.steerAngle - prevSteerAngle, Space.World);
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			tireTransformFR.Rotate(Vector3.up, colliderFR.steerAngle - prevSteerAngle, Space.World);
	}
}

//public class Player : MonoBehaviour
//{
//    public WheelCollider wheelLF;
//    public WheelCollider wheelRF;
//    public WheelCollider wheelLB;
//    public WheelCollider wheelRB;
//
//    public Transform wheelTransformLF;
//    public Transform wheelTransformRF;
//    public Transform wheelTransformLB;
//    public Transform wheelTransformRB;
//
//    public int maxTorque;
//
//    Rigidbody rigid;
//    float Speed;
//
//    private void Awake()
//    {
//        rigid = GetComponent<Rigidbody>();
//    }
//
//    void Start()
//    {
//        maxTorque = 30;
//        rigid.centerOfMass = new Vector3(0, -1, 0);
//    }
//    
//    void Update()
//    {
//        wheelLF.steerAngle = 15 * Input.GetAxis("Horizontal");
//        wheelRF.steerAngle = 15 * Input.GetAxis("Horizontal");
//        wheelLB.motorTorque = maxTorque * Input.GetAxis("Vertical");
//        wheelRB.motorTorque = maxTorque * Input.GetAxis("Vertical");
//
//        wheelTransformLF.Rotate(wheelLF.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
//        wheelTransformRF.Rotate(wheelRF.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
//        wheelTransformLB.Rotate(wheelLB.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
//        wheelTransformRB.Rotate(-wheelRB.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
//
//        Move();
//    }
//
//	void Move()
//	{
//        if (Input.GetKey(KeyCode.UpArrow))
//            wheelLB.attachedRigidbody.position += Vector3.forward * wheelLB.motorTorque * Time.deltaTime;
//        else if (Input.GetKey(KeyCode.DownArrow))
//            wheelLB.attachedRigidbody.position += Vector3.back * wheelRB.motorTorque * Time.deltaTime;
//    }
//}
