using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Server : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string root = @"c:\wwwroot\"; // ドキュメント・ルート
        string prefix = "http://*/"; // 受け付けるURL

        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(prefix); // プレフィックスの登録
        listener.Start();

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest req = context.Request;
            HttpListenerResponse res = context.Response;

            Debug.Log("Server:" + req.RawUrl);

            // リクエストされたURLからファイルのパスを求める
            string path = root + req.RawUrl.Replace("/", "\\");

            // ファイルが存在すればレスポンス・ストリームに書き出す
            if (File.Exists(path))
            {
                byte[] content = File.ReadAllBytes(path);
                res.OutputStream.Write(content, 0, content.Length);
            }
            res.Close();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
