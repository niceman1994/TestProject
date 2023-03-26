using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

/*
    ������ �Լ��� �����ϸ� �ڵ������� trigger�� ����ȴ�.

    onOpen(e)�� ����ڿ��� ���� ������ �ִ� ���������Ʈ, ����, ���������̼� �Ǵ� ����� �� �� ����˴ϴ�.
    onInstall(e)�� ����ڰ� Google Docs, Sheets, Slides �Ǵ� Forms ������ ������ �ΰ� ����� ��ġ�� �� ����˴ϴ�.
    onEdit(e)�� ����ڰ� ���������Ʈ���� ���� ������ �� ����˴ϴ�.
    onSelectionChange(e)�� ����ڰ� ���������Ʈ���� ���� �׸��� ������ �� ����˴ϴ�.
    
    doGet(e)�� ����ڰ� �� ���� �湮�ϰų� ���α׷��� �� �ۿ� HTTP GET ��û�� ���� �� ����˴ϴ�.
    -> HTTP GET ��û�� ���ٱ����� ���� �ޱ⸸ �� �� �ִ�.
    
    doPost(e)�� ���α׷��� �� �ۿ� HTTP POST ��û�� ���� �� ����˴ϴ�.
    -> HTTP POST ��û�� �����͸� ������ �� ����Ѵ�.
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
    // ���� ������ ��(����) ���� �ο��� �ϰ�, ��(����) ���� �÷��̶�� �Ѵ�. (sql�� ���� ���θ� �ϸ� ����)
    // ������ ������ �ֹ���(�ο츦 ã�� ��), �����͸� ���� ���� Ʃ��(������ �������� ����, ���� �Ұ���)�̶�� ���·� �޴´�.
    // �� Ʃ���� ���� �����Ͷ� �⺻������ ������ ������ �Ұ����ϴ�. Ǯ�(����) ����ϰ� json���� �������� ������Ѵ�.

    // ������ ���� -> Ʃ�÷� ���� -> ���� �� json������ ����� ��Ŀ� ���缭 ������ ����

    // URL �ؽ�Ʈ�� ���������� ������ �� ���� ������ "����"�� �ƴ� ��� ����ڷ� �ٲ���Ѵ�.
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
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�");
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
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�");
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
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isDone) print(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
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
