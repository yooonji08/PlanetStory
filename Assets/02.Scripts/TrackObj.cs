using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObj : MonoBehaviour // 캐릭터 정보 스크립트
{
    public bool isDetected;

    public void OnDetect(bool detect) // 마커인식하면 startButton 띄우기
    {
        isDetected = detect;
    }
}
