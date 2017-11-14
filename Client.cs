﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    /*
        // Use this for initialization
        void Start () {
            //ここにpepperのURIを指定
            String uri = "http://192.168.1.140:8000/";
            //String uri = "http://www.yamanjo.net/knowledge/internet/internet_09.html";
            //JSONを記述
            String content = "{'x':1.0,'y':0.0,'theta':0.0}";
            Debug.Log(content);

            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json;charset=UTF-8";
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.Encoding = Encoding.UTF8;
            string response = webClient.UploadString(new Uri(uri), content);
            Debug.Log(response);
        }

        // Update is called once per frame
        void Update () {

        }
    */
    
    void Start()
    {
        StartCoroutine(Post("http://192.168.1.140:8000/"));
    }

    void Update()
    {

    }


    IEnumerator Post(string url)
    {
        // HEADERはDictionaryで記述
        Dictionary<string, string> header = new Dictionary<string, string>();
        // jsonでリクエストを送るのへッダ例
        header.Add("Content-Type", "application/json; charset=UTF-8");

        // LitJsonを使いJSONデータを生成
        LitJson.JsonData data = new LitJson.JsonData();
        data["pepperX"] = 1.0;
        data["pepperY"] = 0.0;
        data["pepperTheta"] = 0.0;
        // シリアライズする(LitJson.JsonData→JSONテキスト)
        string postJsonStr = data.ToJson();
        byte[] postBytes = Encoding.Default.GetBytes(postJsonStr);

        // 送信開始
        WWW www = new WWW(url, postBytes, header);
        yield return www;

        // 成功
        if (www.error == "")
        {
            Debug.Log("Post Success");
        }
        // 失敗
        else
        {
            Debug.Log("Post Failure");
            Debug.Log(www.error);
        }
    }
    
    /*
    void Start()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.140:8000/", form))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    */
    /*
    void Start()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("id", "1");
        dic.Add("name", "2");
        dic.Add("data", "3");
        StartCoroutine(HttpPost("http://192.168.1.140:8000/", dic));
    }

    IEnumerator HttpPost(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<String, String> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);

        // CheckTimeOut()の終了を待つ。5秒を過ぎればタイムアウト
        yield return StartCoroutine(CheckTimeOut(www, 5));

        if (www.error != null)
        {
            Debug.Log("HttpPost NG: " + www.error);
        }
        else if (www.isDone)
        {
            // サーバからのレスポンスを表示
            Debug.Log("HttpPost OK: " + www.text);
        }
    }
    IEnumerator CheckTimeOut(WWW www, float timeout)
    {
        float requestTime = Time.time;

        while (!www.isDone)
        {
            if (Time.time - requestTime < timeout)
                yield return null;
            else
            {
                Debug.Log("TimeOut");  //タイムアウト
                break;
            }
        }
        yield return null;
    }
    */
    /*
    void Start()
    {
        StartCoroutine(Post("http://192.168.1.140:8000/", "{'nickname':'hoge'}"));
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log("Status Code: " + request.responseCode);
    }
    */
}

class TestResponse
{
    public string name;
    public int level;
    public List<string> friend_names;
}