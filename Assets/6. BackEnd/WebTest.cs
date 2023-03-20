using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTest : MonoBehaviour
{
    // 엑셀 파일의 행(가로) 값을 로우라고 하고, 열(세로) 값을 컬럼이라고 한다. (sql에 대한 공부를 하면 좋음)
    // 쿼리는 일종의 주문서(로우를 찾는 것), 데이터를 받을 때는 튜플(일종의 데이터의 집합, 변경 불가능)이라는 형태로 받는다.
    // ※ 튜플은 받은 데이터라서 기본적으로 데이터 변경이 불가능하다. 풀어서(언팩) 사용하고 json파일 형식으로 맞춰야한다.

    // 쿼리를 보냄 -> 튜플로 받음 -> 언팩 후 json파일을 만들고 양식에 맞춰서 엑셀에 저장
    IEnumerator Start() // IEnumerator 로 하는 이유는 웹에서 어떤 것을 요청한 후 응답을 기다리기 위해서이다.
    {
        yield return null;
    }

    void Update()
    {
        
    }
}
