using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
	[SerializeField] private GameObject LeftBackLight;
	[SerializeField] private GameObject RightBackLight;

	[SerializeField] private WheelCollider colliderFR;
	[SerializeField] private WheelCollider colliderFL;
	[SerializeField] private WheelCollider colliderRR;
	[SerializeField] private WheelCollider colliderRL;

	[SerializeField] private Transform tireTransformFL;
	[SerializeField] private Transform tireTransformFR;
	[SerializeField] private Transform wheelTransformFL;
	[SerializeField] private Transform wheelTransformFR;
	[SerializeField] private Transform wheelTransformRL;
	[SerializeField] private Transform wheelTransformRR;

	[SerializeField] private float SteerAngle = 45f;
	[SerializeField] private float decSpeed = 50.0f;
	[SerializeField] private float currentSpeed;
	[SerializeField] private float maxSpeed = 350.0f;
	[SerializeField] private float maxRevSpeed = 100.0f;
	[SerializeField] private int maxTorque = 10;

	private float prevSteerAngle;
	private Rigidbody rigid;
	private float power;

	WheelFrictionCurve SideRLwheel;
	WheelFrictionCurve SideRRwheel;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	void Start()
	{
		rigid.centerOfMass = new Vector3(0.0f, -0.15f, 0.2f);
		power = 30.0f;
		SideRRwheel = colliderRR.sidewaysFriction;
		SideRLwheel = colliderRL.sidewaysFriction;
	}

    void FixedUpdate()
    {
		UpdateWheelPoses();
		rigid.AddForce(-transform.up * GameManager.Instance.downForceValue * rigid.velocity.magnitude);
	}

    void Update()
	{
		Steer();

		if (GameManager.Instance.CountNum == 0)
		{
			prevSteerAngle = colliderFR.steerAngle;
			Control();
			Drift();
			BackLightOnOff();
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Track"))
        {
			if (GameManager.Instance.tireMarks[0].emitting == true &&
				GameManager.Instance.tireMarks[1].emitting == true)
            {
				foreach (TrailRenderer element in GameManager.Instance.tireMarks)
					element.emitting = false;
            }
        }
    }

	void Steer()
    {
		tireTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
		tireTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
	}

	void UpdateWheelPoses()
    {
		UpdateWheelPos(colliderFL, wheelTransformFL);
		UpdateWheelPos(colliderFR, wheelTransformFR);
    }

	void UpdateWheelPos(WheelCollider _collider, Transform _tire) // 앞바퀴 위치 고정
    {
		Vector3 wheelPosition = Vector3.zero;
		Quaternion wheelRotation = Quaternion.identity;

		_collider.GetWorldPose(out wheelPosition, out wheelRotation);

		_tire.position = wheelPosition;
		_tire.rotation = wheelRotation;
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
			if (GameManager.Instance.useBooster == false)
			{
				colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
				colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
			}
			else
            {
				colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power * 1.1f;
				colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power * 1.1f;
			}
		}
		else if (currentSpeed >= 0 && currentSpeed < maxRevSpeed)
		{
			colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
			colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
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

		colliderFR.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
		colliderFL.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;

		wheelTransformFL.Rotate(colliderFL.rpm / 30 * 360 * Time.deltaTime, 0.0f, 0.0f);
		wheelTransformFR.Rotate(colliderFR.rpm / 30 * 360 * Time.deltaTime, 0.0f, 0.0f);
		wheelTransformRL.Rotate(colliderRL.rpm / 30 * 360 * Time.deltaTime, 0.0f, 0.0f);
		wheelTransformRR.Rotate(colliderRR.rpm / 30 * 360 * Time.deltaTime, 0.0f, 0.0f);
	}
	
	void Drift()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKey(KeyCode.RightArrow))
				driftStart();

			if (Input.GetKey(KeyCode.LeftArrow))
				driftStart();
		}
		else if (GameManager.Instance.tireMarks[0].emitting == true &&
				GameManager.Instance.tireMarks[1].emitting == true)
        {
			SideRRwheel.stiffness += Time.deltaTime * 1.5f;
			SideRLwheel.stiffness += Time.deltaTime * 1.5f;

			if (SideRRwheel.stiffness >= 2.0f)
				driftStop();
			else if (SideRRwheel.stiffness < 2.0f && SideRRwheel.stiffness >= 0.0f)
			{
				if (Input.GetKey(KeyCode.RightArrow))
				{
					if (Input.GetAxis("Horizontal") > 0)
					{
						tireTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
						tireTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
						stiffnessDown();
					}
					else if (Input.GetAxis("Horizontal") < 0)
						stiffnessUp();
				}

				if (Input.GetKey(KeyCode.LeftArrow))
				{
					if (Input.GetAxis("Horizontal") > 0)
						stiffnessUp();
					else if (Input.GetAxis("Horizontal") < 0)
					{
						tireTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
						tireTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
						stiffnessDown();
					}
				}
			}
		}
	}

	void driftStart()
    {
		GameManager.Instance.TrailStartEmitter();
		SideRRwheel.stiffness = 0.0f;
		SideRLwheel.stiffness = 0.0f;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;

		transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * SteerAngle * Time.deltaTime, 0.0f), Space.World);
	}

	void driftStop()
    {
		GameManager.Instance.TrailStopEmitter();
		SideRRwheel.stiffness = 2.0f;
		SideRLwheel.stiffness = 2.0f;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;
	}

	void stiffnessUp()
    {
		SideRRwheel.stiffness += Time.deltaTime;
		SideRLwheel.stiffness += Time.deltaTime;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;

		transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * SteerAngle * Time.deltaTime, 0.0f), Space.World);
	}

	void stiffnessDown()
    {
		SideRRwheel.stiffness -= Time.deltaTime;
		SideRLwheel.stiffness -= Time.deltaTime;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;

		transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * SteerAngle * Time.deltaTime, 0.0f), Space.World);
	}

	public float getCurrentSpeed()
    {
		float _speed = currentSpeed;
		return _speed;
    }

	public float getSteerAngle()
    {
		float _angle = SteerAngle;
		return _angle;
    }
}
