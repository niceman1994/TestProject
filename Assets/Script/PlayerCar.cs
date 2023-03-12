using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
	[SerializeField] private GameObject LeftBackLight;
	[SerializeField] private GameObject RightBackLight;

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

	public float highestSpeed = 350.0f;
	public float lowSpeedSteerAngle = 0.1f;
	public float highSpeedStreerAngle = 35.0f;

	public float decSpeed = 50.0f;

	public float currentSpeed;
	public float maxSpeed = 350.0f;
	public float maxRevSpeed = 100.0f;

	public int maxTorque = 10;

	private float prevSteerAngle;
	private Rigidbody rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	void Start()
	{
		rigid.centerOfMass = new Vector3(0, -1, 0); // 무게중심이 높으면 차가 쉽게 전복된다
	}

	void FixedUpdate()
	{
		Control();
	}

	void Update()
	{
		tireTransformFL.Rotate(Vector3.up, colliderFL.steerAngle - prevSteerAngle, Space.World);
		tireTransformFR.Rotate(Vector3.up, colliderFR.steerAngle - prevSteerAngle, Space.World);
		prevSteerAngle = colliderFR.steerAngle;
		BackLightOnOff();
	}

	void BackLightOnOff()
	{
		if (Input.GetKey(KeyCode.DownArrow) || currentSpeed == 0)
		{
			LeftBackLight.SetActive(true);
			RightBackLight.SetActive(true);
		}
		else if (!Input.GetKey(KeyCode.DownArrow) || currentSpeed != 0)
		{
			LeftBackLight.SetActive(false);
			RightBackLight.SetActive(false);
		}
	}

	void Control()
	{
		currentSpeed = 2 * 3.14f * colliderRL.radius * colliderRL.rpm * 60 / 1000;
		currentSpeed = Mathf.Round(currentSpeed);

		if (currentSpeed <= 0 && currentSpeed > -maxSpeed)
		{
			colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * 5;
			colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * 5;
		}
		else if (currentSpeed >= 0 && currentSpeed < maxRevSpeed)
		{
			colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * 5;
			colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * 5;
		}
		else
		{
			colliderRR.motorTorque = 0;
			colliderRL.motorTorque = 0;
		}

		if (!Input.GetButton("Vertical"))
		{
			colliderRR.brakeTorque = decSpeed;
			colliderRL.brakeTorque = decSpeed;
		}
		else
		{
			colliderRR.brakeTorque = 0;
			colliderRL.brakeTorque = 0;
		}

		float speedFactor = rigid.velocity.magnitude / highestSpeed;
		float steerAngle = Mathf.Lerp(lowSpeedSteerAngle, highSpeedStreerAngle, 1 / speedFactor);
		steerAngle *= Input.GetAxis("Horizontal");

		if (Input.GetKey(KeyCode.LeftShift))
		{
			Drift(steerAngle);

			if (Input.GetKeyUp(KeyCode.LeftShift))
				transform.Rotate(Vector3.zero);
		}
		else
			WheelRotate(steerAngle);
	}
	// TODO : 추후 수정
	void WheelRotate(float angle)
    {
		if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
		{
			wheelTransformFL.Rotate(colliderFL.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			wheelTransformRL.Rotate(colliderRL.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			transform.Rotate(Vector3.up * Time.fixedDeltaTime * angle);
			colliderFL.steerAngle = angle;
		}

		if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
		{
			wheelTransformFR.Rotate(colliderFR.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			wheelTransformRR.Rotate(colliderRR.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			transform.Rotate(Vector3.up * Time.fixedDeltaTime * angle);
			colliderFR.steerAngle = angle;
		}
	}

	void Drift(float angle)
    {
		if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
		{
			wheelTransformFL.Rotate(colliderFL.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			wheelTransformRL.Rotate(colliderRL.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			colliderFL.steerAngle = angle;
			transform.Rotate(Vector3.up * Time.fixedDeltaTime * angle * 3);
		}

		if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
		{
			wheelTransformFR.Rotate(colliderFR.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			wheelTransformRR.Rotate(colliderRR.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
			colliderFR.steerAngle = angle;
			transform.Rotate(Vector3.up * Time.fixedDeltaTime * angle * 3);
		}
	}
}
