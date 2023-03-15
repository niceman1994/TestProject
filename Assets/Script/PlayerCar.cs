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
	public float SteerAngle = 30.0f;

	public float decSpeed = 50.0f;

	public float currentSpeed;
	public float maxSpeed = 350.0f;
	public float maxRevSpeed = 100.0f;

	public int maxTorque = 10;

	private float prevSteerAngle;
	private Rigidbody rigid;

	WheelFrictionCurve ForRRwheel;
	WheelFrictionCurve SideRRwheel;
	WheelFrictionCurve ForRLwheel;
	WheelFrictionCurve SideRLwheel;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	void Start()
	{
		rigid.centerOfMass = new Vector3(0, -1, 0);
		ForRRwheel = colliderRR.forwardFriction;
		SideRRwheel = colliderRR.sidewaysFriction;
		ForRLwheel = colliderRL.forwardFriction;
		SideRLwheel = colliderRL.sidewaysFriction;
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
		ResetCar();
	}

	void BackLightOnOff()
	{
		if (currentSpeed == 0)
		{
			LeftBackLight.SetActive(true);
			RightBackLight.SetActive(true);
		}
		else
		{
			LeftBackLight.SetActive(false);
			RightBackLight.SetActive(false);
		}
	}

	void ResetCar()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
			colliderRR.motorTorque = 0;
			colliderRL.motorTorque = 0;
		}
	}

	void Control()
	{
		currentSpeed = 2 * 3.14f * colliderRL.radius * colliderRL.rpm * 60 / 1000;
		currentSpeed = currentSpeed <= 190.0f ? Mathf.Round(currentSpeed) : 190.0f;

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
			colliderRR.brakeTorque = 0.0f;
			colliderRL.brakeTorque = 0.0f;
		}

		//float speedFactor = rigid.velocity.magnitude / highestSpeed;
		//float steerAngle = Mathf.Lerp(lowSpeedSteerAngle, highSpeedStreerAngle, 1 / speedFactor);
		//steerAngle *= Input.GetAxis("Horizontal");

		colliderFR.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
		colliderFL.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;

		Drift();
		WheelRotate();
	}
	// TODO : 언제 할 수 있을지는 모르겠지만 추후 수정
	IEnumerator Drift()
	{
		yield return null;

		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 driftAngle = new Vector3(0.0f, Input.GetAxis("Horizontal") * 15.0f, Input.GetAxis("Horizontal") * 1.0f);

			colliderRR.brakeTorque = decSpeed;
			colliderRL.brakeTorque = decSpeed;

			float decreaseSpeed = Mathf.Round(currentSpeed * 0.2f);
			currentSpeed = Mathf.Round(currentSpeed - decreaseSpeed);

			colliderFR.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
			colliderFL.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
			rigid.AddTorque(driftAngle, ForceMode.Acceleration);
		}

		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 driftAngle = new Vector3(0.0f, Input.GetAxis("Horizontal") * 15.0f, Input.GetAxis("Horizontal") * 1.0f);

			colliderRR.brakeTorque = decSpeed;
			colliderRL.brakeTorque = decSpeed;

			float decreaseSpeed = Mathf.Round(currentSpeed * 0.2f);
			currentSpeed = Mathf.Round(currentSpeed - decreaseSpeed);

			colliderFR.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
			colliderFL.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
			rigid.AddTorque(driftAngle, ForceMode.Acceleration);
		}
	}

	void WheelRotate()
	{
		wheelTransformFL.Rotate(colliderFL.rpm / 180 * 360 * Time.fixedDeltaTime, 0, 0);
		wheelTransformFR.Rotate(colliderFR.rpm / 180 * 360 * Time.fixedDeltaTime, 0, 0);
		wheelTransformRL.Rotate(colliderRL.rpm / 180 * 360 * Time.fixedDeltaTime, 0, 0);
		wheelTransformRR.Rotate(colliderRR.rpm / 180 * 360 * Time.fixedDeltaTime, 0, 0);
	}
}
