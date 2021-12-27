using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
	Rigidbody rigid;
	Animator anim; //Animator속성 변수

	public float moveSpeed; // 이동속도
	public float gravity; // 중력

	public Text itemNum; // 획득한 아이템 개수 (Text UI)
	public static int itemnum; // 획득한 아이템 개수 (변수)
	//public Text distanceNum; // 출발점으로부터 이동한 거리

	public Camera cam; // AR카메라
	public Space mySpace; // 회전축(World)

	Vector3 prePos; // 플레이어의 이전 위치

	void Start()
	{
		anim = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		prePos = transform.localPosition; // 다른 Area 위치의 영향X, 고유 Area내의 고유 위치
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
        {
			prePos = Input.mousePosition; // 위치 갱신
		}

		PlayerMove();
		Physics.gravity = cam.transform.up * -0.5f; // 카메라 바라보는 방향에서 아래쪽으로 중력받기
	}

	// 획득한 아이템 개수 set함수
	public static void plusItemnum(int newItemnum)
    {
		itemnum = newItemnum;
	}

	// 플레이어의 움직임
	void PlayerMove()
    {
		switch (GameMng.instance.gameState) // 게임 모드
        {
			case State.Ready: // 처음 시작
			{
				if (GameMng.instance.objPlayer.isDetected) // 플레이어가 인식됐다면
				{
					anim.Play("Animation"); // 애니메이션 실행
				}
			} break;
			case State.Planet: // 행성 안에 들어왔을 경우
			{
				Vector2 deltaPos = Input.mousePosition - prePos; // 이동량
				deltaPos *= (Time.deltaTime * 10f); // 전환속도 빠르게

				transform.Rotate(0, -deltaPos.x, 0, Space.World); // World 좌표, 마우스의 위치에 따라 y축기준으로 좌우 회전
				if (Input.GetMouseButton(0)) // 플레이어를 클릭하는 동안
				{
					deltaPos *= (Time.deltaTime * 0.1f); // 전환속도 느리게
					transform.Translate(deltaPos.x, 0, deltaPos.y, mySpace);
					/*Vector3 deltaPos = Input.mousePosition - prePos;
					deltaPos *= (Time.deltaTime * 0.1f);
					Vector3 dir = new Vector3(deltaPos.x, 0, deltaPos.y);
					transform.Translate(dir * Time.deltaTime * moveSpeed);
					transform.LookAt(transform.position + dir); // 이동하는 방향으로 회전
					*/
					anim.SetBool("isRun", true);
					
					/*Vector3 deltaPos = Input.mousePosition - prePos;
					deltaPos *= (Time.deltaTime * 0.1f);
					Vector3 dir = new Vector3(deltaPos.x, 0, deltaPos.y);
					transform.Translate(dir, mySpace);
					transform.LookAt(transform.position + dir); // 이동하는 방향으로 회전
					anim.SetBool("isRun", true);*/
				}
                else
                {
					anim.SetBool("isRun", false);
				}
				prePos = Input.mousePosition; // 이전 위치 갱신
			} break;
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		// 꽃 오브젝트와 충돌했고, 미션중인 상태라면
        if (other.CompareTag("FLOWER") && PlanetMng.instance.planetState == PlanetState.Mission)
        {
			itemNum.text = "획득한 아이템 : " + (++itemnum).ToString(); // 획득한 아이템 개수 증가
			other.gameObject.SetActive(false); // 꽃 오브젝트 비활성화

			if (itemnum == 4) // 꽃을 다 모았다면
            {
				PlanetMng.instance.planetState = PlanetState.Result; // 완료 상태로 전환
            }
        }
    }

    // 컴포넌트를 얻었다면
    public void OnFound()
    {
		if (rigid != null)
        {
			rigid.isKinematic = false;
        }
    }

	// 컴포넌트를 발견하지 못했다면
	public void OnLost()
    {
		if (rigid != null)
        {
			rigid.isKinematic = true;
        }
    }
}
