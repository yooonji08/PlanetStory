using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkActive : MonoBehaviour
{
    public GameObject talkBtn; // 대화하기 버튼

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 마크 영역(! 영역)에 들어왔다면
        if (other.CompareTag("PLAYER"))
        {
            if (PlanetMng.instance.planetState == PlanetState.Default) // 처음 상태라면
            {
                talkBtn.gameObject.SetActive(true); // 대화하기 버튼 활성화
            }
            else if (PlanetMng.instance.planetState == PlanetState.Mission) // 미션 상태라면
            {
                PlanetMng.instance.noClearMission(); // 아직 부족하다는 텍스트 출력
            }
            else if (PlanetMng.instance.planetState == PlanetState.Result) // 미션을 완료한 상태라면
            {
                PlanetMng.instance.getClearMission1(); // 텍스트 상자 출력
            }
        }
    }
}
