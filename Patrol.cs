using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class Patrol : MonoBehaviour {
    public List<Transform> points;
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 startDirection;
    private Vector3 pastPosition;
    private float pastRotation; //pepperがどっち向いてるか
    private float pepperSpeed = 2f;
    private float timer; //TimerSetにて活動限界を設定
    private bool timeOut = false;
    private String uri = "http://192.168.1.140:8000"; //ここにpepperのURIを指定

    public class JSONClass
    {
        public double x;
        public double y;
        public double theta;
        public bool stop_flg;
        public bool photo_flg;
    }


    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        startPosition = agent.transform.position;
        // TODO ここTangoから入力
        //startPosition.Set(3.4f, 0f, -1.55f);
        agent.transform.position = startPosition;
        pastPosition = startPosition;
        // Tangoから向きをとれるならそれでも良い
        pastRotation = agent.transform.eulerAngles.y;
        startDirection = agent.transform.forward;

        // 初期設定
        // autoBrakingをtrueにするとオブジェクトが目的地の前でブレーキをかけるようになる
        agent.autoBraking = false;
        agent.stoppingDistance = 0.2f;
        agent.speed = pepperSpeed;

        Debug.Log("Hello, pepper");
        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        //タイマーリセット
        //TimerSet();

        // 目的地が設定されていない場合
        if (points.Count == 0)
            return;

        // すべての目的地に訪れていない場合
        if (destPoint + 1 <= points.Count)
        {
            Debug.Log("Go to next.");
            // agentを次に行くべき目的地へ
            agent.destination = points[destPoint].position;
            destPoint++;
        }
        // すべての目的地に訪れた場合
        else if (destPoint + 1 == points.Count + 1)
        {
            agent.speed = 0f;
            agent.speed = pepperSpeed;
            Debug.Log("Go to Goal.");
            GoToGoal();
        }
        // 初期位置に戻ってきた場合
        else if (destPoint + 1 == points.Count + 2)
        {
            Debug.Log("Offset");
            Debug.Log(agent.transform.position - pastPosition);
            agent.speed = 0f;
            agent.transform.forward = startDirection;
            destPoint++;
        }

    }

    void GoToGoal()
    {
        agent.destination = startPosition;
        destPoint++;
    }

    /*
    void TimeCounter()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            timeOut = true;
    }

    void TimerSet()
    {
        timeOut = false;
        timer = 2f;
    }
    */

    void Update()
    {
        //タイマー設置
        //TimeCounter();

        // agentが目的地に着いたら待機して
        // pepperとagentの距離が小さければGotoNextPoint、大きければGoForIt
        if (agent.remainingDistance < 0.2f && destPoint + 1 <= points.Count + 2)
        {
            Debug.Log("Reached");
            Debug.Log(pastRotation - 90);
            Debug.Log(agent.transform.position - pastPosition);

            // pepperの座標系に合わせてMoveToへの引数を変換
            double pepperX = (agent.transform.position - pastPosition).x * (Math.Cos((pastRotation - 90) * (Math.PI / 180)))
                - (agent.transform.position - pastPosition).z * Math.Sin((pastRotation - 90) * (Math.PI / 180));
            double pepperY = (agent.transform.position - pastPosition).x * (Math.Sin((pastRotation - 90) * (Math.PI / 180)))
                + (agent.transform.position - pastPosition).z * Math.Cos((pastRotation - 90) * (Math.PI / 180));
            Debug.Log(pepperX + ", " + pepperY);

            String json = SetJSON(pepperX, pepperY, agent.transform.eulerAngles.y - 90, 0, 1); //0:false, 1:true
            Debug.Log(json);
            //Send2pepper(json);

            pastRotation = agent.transform.eulerAngles.y;
            pastPosition = agent.transform.position;
            GotoNextPoint();
        }
        // タイムアウトしたら現在地をもらってから行くべき場所へ
        // 障害物があるときは回避行動がとれるとベター
        /*
        else if (timeOut)
        {
            Debug.Log("Time Up!!");
            agent.speed = 0f;
            // TODO ここで現在位置をもらう
            //agent.transform.position = agent.transform.position - new Vector3(0.3f, 0f, 0f);
            Debug.Log(pastRotation);
            Debug.Log(agent.transform.position - pastPosition);

            // pepperの座標系に合わせてMoveToへの引数を変換
            float pepperX = (float)((agent.transform.position - pastPosition).x * (Math.Cos((pastRotation - 90) * (Math.PI / 180)))
                - (agent.transform.position - pastPosition).z * Math.Sin((pastRotation - 90) * (Math.PI / 180)));
            float pepperY = (float)((agent.transform.position - pastPosition).x * (Math.Sin((pastRotation - 90) * (Math.PI / 180)))
                + (agent.transform.position - pastPosition).z * Math.Cos((pastRotation - 90) * (Math.PI / 180)));
            Debug.Log(pepperX + ", " + pepperY);

            Debug.Log(SetJSON(pepperX, pepperY, agent.transform.eulerAngles.y - 90));

            pastRotation = agent.transform.eulerAngles.y;
            pastPosition = agent.transform.position;
            TimerSet();
            agent.speed = pepperSpeed;
        }
        */
        // 最初の位置に戻ってきたら、初期化して再びpepperからの通信を待つ
        else if (destPoint + 1 > points.Count + 2)
        {
            Debug.Log("Everything has been done.");
            //TimerSet();
            destPoint = 0;
            Start();
        }
    }

    public String SetJSON(double x, double y, double theta, int stop_flg, int photo_flg)
    {
        /*
        JSONClass unityObject = new JSONClass();
        unityObject.x = x;
        unityObject.y = y;
        unityObject.theta = theta;
        unityObject.stop_flg = stop_flg;
        unityObject.photo_flg = photo_flg;
        string json = JsonUtility.ToJson(unityObject);
        */
        String json = "{\"0\":{\"x\":" + x + ", \"y\":" + y + ", \"theta\":" + theta + "}, " +
            "\"stop_flg\":" + stop_flg + ", \"photo_flg\":" + photo_flg + ", " +
            "\"dest_x\":0.0, \"dest_y\":0.0}";
        return json;
    }

    public void Send2pepper(String json)
    {
        byte[] postBytes = Encoding.Default.GetBytes(json);

        WebClient webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json;charset=UTF-8";
        webClient.Headers[HttpRequestHeader.Accept] = "application/json";
        webClient.Encoding = Encoding.UTF8;

        byte[] response = webClient.UploadData(new Uri(uri), "POST", postBytes);
        Debug.Log(response);
    }
}