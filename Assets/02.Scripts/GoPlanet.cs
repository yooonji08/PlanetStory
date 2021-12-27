using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoPlanet : MonoBehaviour // 구역 위치 변수 할당
{
    private void OnTriggerEnter(Collider other) // 플레이어와 충돌했을 경우
	{
		string areaName = other.gameObject.name; // 충돌한 오브젝트 이름

		// 충돌한 오브젝트의 태그가 AREA이고, 대화하기 상태가 아니라면
		if (other.CompareTag("AREA") && PlanetMng.instance.planetState != PlanetState.Talk)
        {
			PlanetMng.instance.paperPlanet[PlanetMng.getCurArea()].SetActive(false); // 이전 지역 비활성화

			// 오브젝트 이름에 따라서 행성 내의 지역 인덱스 변경
			switch (areaName)
			{
				case "GotoArea01": PlanetMng.setCurArea(0); break;
				case "GotoArea02": PlanetMng.setCurArea(1); break;
				case "GotoArea03": PlanetMng.setCurArea(2); break;
				case "GotoArea04": PlanetMng.setCurArea(3); break;
				case "GotoArea05": PlanetMng.setCurArea(4); break;
				case "GotoArea06": PlanetMng.setCurArea(5); break;
				case "GotoArea07": PlanetMng.setCurArea(6); break;
				case "GotoArea08": PlanetMng.setCurArea(7); break;
				case "GotoArea09": PlanetMng.setCurArea(8); break;
			}

			PlanetMng.instance.GoAnotherArea(); // 다른 지역으로 이동
		}
	}
}
