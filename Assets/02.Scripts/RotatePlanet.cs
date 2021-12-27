using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    public Transform targetTr; // 중심점 위치
    public float speed; // 회전속도

    Transform tr; // 행성 자신의 위치

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        tr.RotateAround(targetTr.position, Vector3.up, Time.deltaTime * speed); // 중심점 기준으로 떨어진 거리만큼, y축기준으로 회전
    }
}
