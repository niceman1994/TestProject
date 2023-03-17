using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
	[SerializeField] private GameObject LeftBackLight;
	[SerializeField] private GameObject RightBackLight;

	public WheelCollider colliderFR;
	public WheelCollider colliderFL;
	public WheelCollider colliderRR;
	public WheelCollider colliderRL;

	public Transform wheelTransformFL;
	public Transform wheelTransformFR;
	public Transform wheelTransformRL;
	public Transform wheelTransformRR;

	[SerializeField] private float SteerAngle = 30.0f;
	[SerializeField] private float decSpeed = 50.0f;

	public float currentSpeed;
	[SerializeField] private float maxSpeed = 350.0f;
	[SerializeField] private float maxRevSpeed = 100.0f;

	[SerializeField] private int maxTorque = 10;

	private float prevSteerAngle;
	private float carSpeed;
	private Rigidbody rigid;

	WheelFrictionCurve ForFRwheel;
	WheelFrictionCurve SideFRwheel;
	WheelFrictionCurve ForFLwheel;
	WheelFrictionCurve SideFLwheel;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	void Start()
	{
		rigid.centerOfMass = new Vector3(0, -1, 0);
		carSpeed = 27.0f;

		ForFRwheel = colliderFR.forwardFriction;
		SideFRwheel = colliderFR.sidewaysFriction;
		ForFLwheel = colliderFL.forwardFriction;
		SideFLwheel = colliderFL.sidewaysFriction;
	}

	void FixedUpdate()
	{
		Control();
	}

	void Update()
	{
		wheelTransformFL.Rotate(Vector3.up, colliderFL.steerAngle - prevSteerAngle, Space.World);
		wheelTransformFR.Rotate(Vector3.up, colliderFR.steerAngle - prevSteerAngle, Space.World);
		prevSteerAngle = colliderFR.steerAngle;
		BackLightOnOff();
	}

	void BackLightOnOff()
	{
		if (currentSpeed == 0)
		{
			LeftBackLight.SetActive(true);
			RightBackLight.SetActive(true);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			if (currentSpeed < 0 && currentSpeed >= -maxSpeed)
			{
				LeftBackLight.SetActive(true);
				RightBackLight.SetActive(true);
			}
			else if (currentSpeed > 0 && currentSpeed <= maxRevSpeed)
			{
				LeftBackLight.SetActive(false);
				RightBackLight.SetActive(false);
			}
		}
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			if (currentSpeed > 0 && currentSpeed <= maxRevSpeed)
			{
				LeftBackLight.SetActive(true);
				RightBackLight.SetActive(true);
			}
			else if (currentSpeed < 0 && currentSpeed >= -maxSpeed)
			{
				LeftBackLight.SetActive(false);
				RightBackLight.SetActive(false);
			}
		}
		else if (currentSpeed != 0)
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
			colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * carSpeed;
			colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * carSpeed;
		}
		else if (currentSpeed >= 0 && currentSpeed < maxRevSpeed)
		{
			colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * carSpeed;
			colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * carSpeed;
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

		if (Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
		{
			colliderRR.brakeTorque = decSpeed - (decSpeed * 0.8f);
			colliderRL.brakeTorque = decSpeed - (decSpeed * 0.8f);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
		{
			colliderRR.brakeTorque = decSpeed - (decSpeed * 0.8f);
			colliderRL.brakeTorque = decSpeed - (decSpeed * 0.8f);
		}

		Drift();
		WheelRotate();
	}
	// TODO : 언제 할 수 있을지는 모르겠지만 추후 수정
	void Drift()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
			{
				colliderRR.brakeTorque = decSpeed - (decSpeed * 0.25f);
				colliderRL.brakeTorque = decSpeed - (decSpeed * 0.25f);

				transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 90.0f * Time.deltaTime);
			}

			if (Input.GetKeyUp(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
			{
				wheelTransformFL.Rotate(wheelTransformFL.rotation.x, 0.0f, 0.0f);
				wheelTransformFR.Rotate(wheelTransformFR.rotation.x, 0.0f, 0.0f);
			}

			if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
			{
				colliderRR.brakeTorque = decSpeed - (decSpeed * 0.25f);
				colliderRL.brakeTorque = decSpeed - (decSpeed * 0.25f);

				transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 90.0f * Time.deltaTime);
			}

			if (Input.GetKeyUp(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
			{
				wheelTransformFL.Rotate(wheelTransformFL.rotation.x, 0.0f, 0.0f);
				wheelTransformFR.Rotate(wheelTransformFR.rotation.x, 0.0f, 0.0f);
			}
		}
	}

	void WheelPos()
	{
		Vector3 wheelPosition = Vector3.zero;
		Quaternion wheelRotation = Quaternion.identity;

		colliderFR.GetWorldPose(out wheelPosition, out wheelRotation);
		colliderFL.GetWorldPose(out wheelPosition, out wheelRotation);
		colliderRR.GetWorldPose(out wheelPosition, out wheelRotation);
		colliderRL.GetWorldPose(out wheelPosition, out wheelRotation);
	}

	void WheelRotate()
	{
		wheelTransformFL.Rotate(colliderFL.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
		wheelTransformFR.Rotate(colliderFR.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
		wheelTransformRL.Rotate(colliderRL.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
		wheelTransformRR.Rotate(colliderRR.rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
	}
}
