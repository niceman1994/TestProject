using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
	static public CreateManager Instance = null;

	[Range(3.0f, 15.0f)]
	private float time;
	[ReadOnly, SerializeField] private Enemy EnemyObj;

	public List<Object> EnemyList = new List<Object>();

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	private void Start()
	{
		time = 5.0f;
	}

	public void CreateObject(Enemy _Obj) // 싱글톤에서 오브젝트를 만들어주는 함수
	{
		Object enemy = Instantiate(_Obj);
		enemy._Object.AddComponent<MeshFilter>();
		enemy.Initialize();

		EnemyList.Add(enemy); // 초기값 설정 없이 오브젝트를 복제하고 List에 넣었다.

		// 객체 형태의 쿼리문, 실제로는 데이터 중심의 쿼리문으로 많이 사용한다.
		//List<Enemy> tempList = (List<Enemy>)(from e in EnemyList where e.Output() select e); // EnemyList에 있는 것들 중에 특정 조건에 해당되는 e를 넘겨준다.

		// 람다식
		//List<Enemy> tempList = EnemyList.FindAll(e => e.Output());
		//Enemy temp = EnemyList.Find(e => e.Output());
		//EnemyList.Sort( (e, d) => e.Value.CompareTo(d) );
	}

	private void FixedUpdate()
	{
		time -= Time.deltaTime; // time의 시작값인 5초에서 deltaTime의 시간만큼 계속 빼준다.

		if (time < 0.0f) // time의 값이 0 미만이 될 경우 작동됨.
		{
			time = 5.0f; // time을 다시 5초로 설정해준다.

			// 오브젝트를 만들어주는 함수를 다시 호출한다.
			CreateObject(EnemyObj);
		}
	}
}
