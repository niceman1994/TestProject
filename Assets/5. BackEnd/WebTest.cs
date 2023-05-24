using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

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

[System.Serializable]
public class Userform
{
    public string userName;
    public string phoneNumber;
    public string message;
    public string log;

    public Userform(string userName, string phoneNumber)
	{
        this.userName = userName;
        this.phoneNumber = phoneNumber;
	}

    /*
    public UserInfo(string userName, string phoneNumber, string message, string log)
    {
        this.userName = userName;
        this.phoneNumber = phoneNumber;
        this.message = message;
        this.log = log;
    }
    */
}

[System.Serializable]
public class UserData
{
    public Userform1[] people;
}

[System.Serializable]
public class Userform1
{
    public string name;
    public string phone;
    public string age;

    public Userform1(string name, string phone, string age)
    {
        this.name = name;
        this.phone = phone;
        this.age = age;
    }
}

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;
}

public class WebTest : MonoBehaviour
{
    // 엑셀 파일의 행(가로) 값을 로우라고 하고, 열(세로) 값을 컬럼이라고 한다. (sql에 대한 공부를 하면 좋음)
    // 쿼리는 일종의 주문서(로우를 찾는 것), 데이터를 받을 때는 튜플(일종의 데이터의 집합, 변경 불가능)이라는 형태로 받는다.
    // ※ 튜플은 받은 데이터라서 기본적으로 데이터 변경이 불가능하다. 풀어서(언팩) 사용하고 json파일 형식으로 맞춰야한다.

    // 쿼리를 보냄 -> 튜플로 받음 -> 언팩 후 json파일을 만들고 양식에 맞춰서 엑셀에 저장

    // URL 텍스트를 가져오려면 배포할 때 접속 권한을 "나만"이 아닌 모든 사용자로 바꿔야한다.
    string URL = "https://script.google.com/macros/s/AKfycbxK8Lj5jkr_6IQcpevcoYzQHlHRQTMmH-NGFJebv4glrltUFsYDke_MUqLc91Ozdome/exec";

    public GoogleData GD;
    public InputField IDInput;
    public InputField PWDInput;
    public Button login;
    public Button Register;
    public Button Resetpwd;

    void Start()
	{
        EventSystem.current.SetSelectedGameObject(IDInput.gameObject);

        /*
        TextAsset textAsset = Resources.Load<TextAsset>("Json/123");
        UserData Info = JsonUtility.FromJson<UserData>(textAsset.text);
        //Userform1 Info = JsonUtility.FromJson<Userform1>(textAsset.text);        

        string userdata = JsonUtility.ToJson(Info);

        //if (Info.people.)
        //print(userdata);

        //print(Info.name);
        //print(Info.age);

        yield return null;
        */
        //form.AddField(nameof(Info.userName), Info.userName);
        //form.AddField(nameof(Info.phoneNumber), Info.phoneNumber);
        //form.AddField(nameof(Info.message), Info.message);
        //form.AddField(nameof(Info.log), Info.log);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (EventSystem.current.currentSelectedGameObject == IDInput.gameObject)
                EventSystem.current.SetSelectedGameObject(PWDInput.gameObject);
            else if (EventSystem.current.currentSelectedGameObject == PWDInput.gameObject)
                EventSystem.current.SetSelectedGameObject(login.gameObject);
            else if (EventSystem.current.currentSelectedGameObject == login.gameObject)
                EventSystem.current.SetSelectedGameObject(Register.gameObject);
            else if (EventSystem.current.currentSelectedGameObject == Register.gameObject)
                EventSystem.current.SetSelectedGameObject(Resetpwd.gameObject);
            else if (EventSystem.current.currentSelectedGameObject == Resetpwd.gameObject)
                EventSystem.current.SetSelectedGameObject(IDInput.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (EventSystem.current.currentSelectedGameObject == IDInput.gameObject)
                EventSystem.current.SetSelectedGameObject(PWDInput.gameObject);
        }
    }

    bool SetIDPass()
    {
        if (IDInput.text.Trim() == "" || PWDInput.text.Trim() == "") return false;
        else return true;
    }

    public void register()
    {
        if (!SetIDPass())
        {
            print("아이디 또는 비밀번호가 비어있습니다");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", IDInput.text.Trim());
        form.AddField("pwd", PWDInput.text.Trim());

        StartCoroutine(Post(form));
    }

    public void Login()
    {
        if (!SetIDPass())
        {
            print("아이디 또는 비밀번호가 비어있습니다");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", IDInput.text.Trim());
        form.AddField("pwd", PWDInput.text.Trim());

        StartCoroutine(Post(form));
    }

    void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // 반드시 using을 써야한다
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isDone) print(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
    }

    public void EnterLogin()
    {
        if (EventSystem.current.currentSelectedGameObject == PWDInput.gameObject)
        {
            if (IDInput.text != null && PWDInput.text != null)
                EventSystem.current.SetSelectedGameObject(login.gameObject);
        }
    }

    public void pwdReset()
    {
        if (EventSystem.current.currentSelectedGameObject == Resetpwd.gameObject)
            PWDInput.text = null;
    }
}
