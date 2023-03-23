using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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

public class WebTest : MonoBehaviour
{
    // ���� ������ ��(����) ���� �ο��� �ϰ�, ��(����) ���� �÷��̶�� �Ѵ�. (sql�� ���� ���θ� �ϸ� ����)
    // ������ ������ �ֹ���(�ο츦 ã�� ��), �����͸� ���� ���� Ʃ��(������ �������� ����, ���� �Ұ���)�̶�� ���·� �޴´�.
    // �� Ʃ���� ���� �����Ͷ� �⺻������ ������ ������ �Ұ����ϴ�. Ǯ�(����) ����ϰ� json���� �������� ������Ѵ�.

    // ������ ���� -> Ʃ�÷� ���� -> ���� �� json������ ����� ��Ŀ� ���缭 ������ ����

    // URL �ؽ�Ʈ�� ���������� ������ �� ���� ������ "����"�� �ƴ� ��� ����ڷ� �ٲ���Ѵ�.
    string URL = "https://script.google.com/macros/s/AKfycbyY4O6U_UBByXTx8cCFnbhlLF_3jVGArcUftKgYHJGsxXPS4x8j8bCw_efONuSzkIpK-A/exec";

    public InputField IdInput;
    public InputField PwdInput;
    string id, pwd;

    void Start() 
    {
        id = IdInput.text.Trim();
        pwd = PwdInput.text.Trim();

        Register();
    }

    bool SetIDPass()
    {
        if (id == "" || pwd == "") return false;
        else return true;
    }

    public void Register()
    {
        if (!SetIDPass())
        {
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�.");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("id", "qwasd");
        form.AddField("pwd", "123");

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form) // IEnumerator �� �ϴ� ������ ������ � ���� ��û�� �� ������ ��ٸ��� ���ؼ��̴�.
    {
        //using (var www = UnityWebRequest.Get(URL))
        //{
        //    yield return www.SendWebRequest();
        //
        //    print(www.downloadHandler.text);
        //}

        using (var www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            print(www.downloadHandler.text);
        }
    }
}
