using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {
    public List<Transform> points;
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 startDirection;
    private Vector3 pastPosition;
    private float pepperSpeed = 2f;
    private float timer; //TimerSetにて活動限界を設定
    private bool timeOut = false;
 

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //startPosition = agent.transform.position;
        // TODO ここTangoから入力
        startPosition.Set(-3.5f, 0f, 0f);
        agent.transform.position = startPosition;
        pastPosition = startPosition;
        startDirection = agent.transform.forward;

        // 初期設定
        // autoBrakingをtrueにするとオブジェクトが目的地の前でブレーキをかけるようになる
        agent.autoBraking = false;
        agent.stoppingDistance = 0.5f;
        agent.speed = pepperSpeed;

        Debug.Log("Hello, pepper");
        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        //タイマーリセット
        TimerSet();

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

    void GoForIt()
    {
        agent.destination = points[destPoint].position;
    }


    void GoToGoal()
    {
        agent.destination = startPosition;
        destPoint++;
    }

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

    void Update()
    {
        //タイマー設置
        TimeCounter();

        // agentが目的地に着いたら待機して
        // pepperとagentの距離が小さければGotoNextPoint、大きければGoForIt
        if (agent.remainingDistance < 0.7f && destPoint + 1 <= points.Count + 2)
        {
            Debug.Log("Reached");
            Debug.Log(agent.transform.position - pastPosition);
            pastPosition = agent.transform.position;
            GotoNextPoint();
        }
        // タイムアウトしたら現在地をもらってから行くべき場所へ
        // 障害物があるときは回避行動がとれるとベター
        else if (timeOut)
        {
            Debug.Log("Time Up!!");
            agent.speed = 0f;
            // TODO ここで現在位置をもらう
            //agent.transform.position = agent.transform.position - new Vector3(0.3f, 0f, 0f);
            Debug.Log(agent.transform.position - pastPosition);
            pastPosition = agent.transform.position;
            TimerSet();
            agent.speed = pepperSpeed;
        }
        // 最初の位置に戻ってきたら、初期化して再びpepperからの通信を待つ
        else if (destPoint + 1 > points.Count + 2)
        {
            Debug.Log("Everything has been done.");
            TimerSet();
            destPoint = 0;
            Start();
        }
    }
}