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

	WheelFrictionCurve SideRRwheel;
	WheelFrictionCurve SideRLwheel;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	void Start()
	{
		rigid.centerOfMass = new Vector3(0.0f, -0.15f, 0.2f);
		power = 18.0f;
		SideRRwheel = colliderRR.sidewaysFriction;
		SideRLwheel = colliderRL.sidewaysFriction;
	}

    void FixedUpdate()
    {
		GetFLWheelPos();
		GetFRWheelPos();
		rigid.AddForce(-transform.up * GameManager.Instance.downForceValue * rigid.velocity.magnitude);
	}

    void Update()
	{
		if (GameManager.Instance.StartRace == true)
		{
			tireTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
			tireTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle - prevSteerAngle) * Time.deltaTime, Space.World);
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

	void GetFLWheelPos() // 왼쪽 앞바퀴 위치 고정
    {
		Vector3 wheelPosition = Vector3.zero;
		Quaternion wheelRotation = Quaternion.identity;

		colliderFL.GetWorldPose(out wheelPosition, out wheelRotation);

		wheelTransformFL.transform.position = wheelPosition;
		wheelTransformFL.transform.rotation = wheelRotation;
		
	}

	void GetFRWheelPos() // 오른쪽 앞바퀴 위치 고정
	{
		Vector3 wheelPosition = Vector3.zero;
		Quaternion wheelRotation = Quaternion.identity;

		colliderFR.GetWorldPose(out wheelPosition, out wheelRotation);

		wheelTransformFR.transform.position = wheelPosition;
		wheelTransformFR.transform.rotation = wheelRotation;
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
			colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
			colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
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
			else if (Input.GetKey(KeyCode.LeftArrow))
				driftStart();

			if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
				driftStop();
		}

		if (GameManager.Instance.tireMarks[0].emitting == true &&
			GameManager.Instance.tireMarks[1].emitting == true)
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				SideRRwheel.stiffness += Time.deltaTime * 1.2f;
				SideRLwheel.stiffness += Time.deltaTime * 1.2f;

				if (SideRRwheel.stiffness >= 1.0f)
					driftStop();
				else if (SideRRwheel.stiffness < 1.0f && SideRRwheel.stiffness >= 0.0f)
				{
					if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
					{
						if (Input.GetAxis("Horizontal") > 0)
							stiffnessDown();
						else if (Input.GetAxis("Horizontal") < 0)
							stiffnessUp();
					}

					if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
					{
						if (Input.GetAxis("Horizontal") > 0)
							stiffnessUp();
						else if (Input.GetAxis("Horizontal") < 0)
							stiffnessDown();
					}
				}
			}
			else
			{
				SideRRwheel.stiffness = 1.0f;
				SideRLwheel.stiffness = 1.0f;
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
		SideRRwheel.stiffness = 1.0f;
		SideRLwheel.stiffness = 1.0f;

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
