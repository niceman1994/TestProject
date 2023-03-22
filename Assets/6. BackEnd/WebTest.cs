using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
    지정된 함수를 실행하면 자동적으로 trigger가 실행된다.

    onOpen(e)는 사용자에게 수정 권한이 있는 스프레드시트, 문서, 프레젠테이션 또는 양식을 열 때 실행됩니다.
    onInstall(e)은 사용자가 Google Docs, Sheets, Slides 또는 Forms 내에서 편집자 부가 기능을 설치할 때 실행됩니다.
    onEdit(e)는 사용자가 스프레드시트에서 값을 변경할 때 실행됩니다.
    onSelectionChange(e)는 사용자가 스프레드시트에서 선택 항목을 변경할 때 실행됩니다.
    
    doGet(e)는 사용자가 웹 앱을 방문하거나 프로그램이 웹 앱에 HTTP GET 요청을 보낼 때 실행됩니다.
    -> HTTP GET 요청은 접근권한이 없고 받기만 할 수 있다.
    
    doPost(e)는 프로그램이 웹 앱에 HTTP POST 요청을 보낼 때 실행됩니다.
    -> HTTP POST 요청은 데이터를 전달할 때 사용한다.
*/

public class WebTest : MonoBehaviour
{
    // 엑셀 파일의 행(가로) 값을 로우라고 하고, 열(세로) 값을 컬럼이라고 한다. (sql에 대한 공부를 하면 좋음)
    // 쿼리는 일종의 주문서(로우를 찾는 것), 데이터를 받을 때는 튜플(일종의 데이터의 집합, 변경 불가능)이라는 형태로 받는다.
    // ※ 튜플은 받은 데이터라서 기본적으로 데이터 변경이 불가능하다. 풀어서(언팩) 사용하고 json파일 형식으로 맞춰야한다.

    // 쿼리를 보냄 -> 튜플로 받음 -> 언팩 후 json파일을 만들고 양식에 맞춰서 엑셀에 저장

    // URL 텍스트를 가져오려면 배포할 때 접속 권한을 "나만"이 아닌 모든 사용자로 바꿔야한다.
    string URL = "https://script.google.com/macros/s/AKfycbyY4O6U_UBByXTx8cCFnbhlLF_3jVGArcUftKgYHJGsxXPS4x8j8bCw_efONuSzkIpK-A/exec";

    IEnumerator Start() // IEnumerator 로 하는 이유는 웹에서 어떤 것을 요청한 후 응답을 기다리기 위해서이다.
    {
        using (var www = UnityWebRequest.Get(URL))
        {
            yield return www.SendWebRequest();

            print(www.downloadHandler.text);
        }
    }
}
