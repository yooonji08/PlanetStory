using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum State
{
    Ready,    // 처음
    Space,    // 게임 진행(우주)
    Planet,   // 게임 진행(행성 안)
    Clear     // 종이행성을 클리어한 후라면
}

public class GameMng : MonoBehaviour
{
    static public GameMng instance; // 싱글톤 인스턴스 (다른 스크립트에서 GameMng를 사용하고 싶을 때를 고려하여 변수 선언)
    public GameObject[] stages; // 스테이지 배열
    int curStage = 0; // 현재 스테이지 배열 인덱스 값
    
    public TrackObj objPlayer; // 플레이어

    public Text titleText; // 게임 제목 및 지면인식 텍스트
    public GameObject objStartBtn; // 시작 버튼
    public GameObject PrevBtn; // 이전 버튼
    public GameObject NextBtn; // 다음 버튼
    public GameObject AdvBtn; // 모험하기 버튼

    public State gameState = State.Ready; // 현재 게임 상태

    void Start()
    {
        if (instance == null) // 싱글톤이 null일경우
        {
            instance = this; // 자기 자신을 할당
        }
    }

    void Update()
    {
        if (gameState == State.Ready) // 게임 상태가 처음 단계라면
        {
            if (objPlayer.isDetected) // 플레이어가 인식됐다면
            {
                if (objStartBtn.activeSelf == false) // 시작 버튼이 없는 상태라면
                {
                    objStartBtn.SetActive(true); // 시작 버튼 활성화
                    titleText.text = "Planet Story"; // 게임 제목 띄우기
                }
            }
            else // 플레이어가 인식이 안됐을 경우
            {
                if (objStartBtn.activeSelf) // 시작버튼이 활성화되어 있다면
                {
                    objStartBtn.SetActive(false); // 시작 버튼 비활성화
                    titleText.text = "지면을 인식해주세요.";
                }
            }
        }

        StageText(); // 스테이지별로 설정한 텍스트
    }

    // StartButton을 누른다면
    public void OnClickStart()
    {
        gameState = State.Space; // Space모드로 전환

        objStartBtn.SetActive(false); // 시작버튼 비활성화
        PrevBtn.SetActive(true); // 이전 버튼 활성화
        NextBtn.SetActive(true); // 다음 버튼 활성화

        NextStage(); // 다음 스테이지로 이동
    }

    // 우주에서 NextButton을 누른다면
    public void OnClickNextButton()
    {
        NextStage();
    }

    // 우주에서 PrevButton을 누른다면
    public void OnClickPrevButton()
    {
        PrevStage();
    }
    
    // 우주에서 탐험하기 버튼을 누른다면
    public void OnClickAdventureButton()
    {
        gameState = State.Planet; // Planet모드로 전환

        titleText.text = ""; // 텍스트 삭제

        SceneManager.LoadScene("PaperPlanet"); // 종이행성으로 이동
    }

    // 이전 스테이지로 바꾸기
    public void PrevStage()
    {
        StartCoroutine(ChangePrevStage());
    }

    IEnumerator ChangePrevStage()
    {
        stages[curStage].SetActive(false); // 현재 스테이지 비활성화

        --curStage; // 인덱스 감소
        if (curStage > 0) // 배열개수만큼만 감소할 수 있도록 if문 추가
        {
            yield return new WaitForSeconds(0.1f); // 0.1초뒤에
            stages[curStage].SetActive(true); // 이전 스테이지 활성화
        }
        else
        {
            titleText.text = "더 이상 행성이 없습니다.";
            yield return new WaitForSeconds(0.5f); // 0.5초뒤에
            titleText.text = "이전 행성으로 돌아갑니다.";
            yield return new WaitForSeconds(0.5f); // 0.5초뒤에
            ++curStage;
            stages[curStage].SetActive(true); // 이전 스테이지 활성화
        }
    }

    // 다음 스테이지로 바꾸기
    public void NextStage()
    {
        StartCoroutine(ChangeNextStage());
    }

    IEnumerator ChangeNextStage()
    {
        stages[curStage].SetActive(false); // 현재 스테이지 비활성화

        ++curStage; // 인덱스 증가
        if (curStage < stages.Length) // 배열개수만큼만 증가할 수 있도록 if문 추가
        {
            yield return new WaitForSeconds(0.1f); // 0.1초뒤에
            stages[curStage].SetActive(true); // 다음 스테이지 활성화
        }
        else // 행성이 없는 경우
        {
            titleText.text = "더 이상 행성이 없습니다.";
            yield return new WaitForSeconds(0.5f); // 0.5초뒤에
            titleText.text = "이전 행성으로 돌아갑니다.";
            yield return new WaitForSeconds(0.5f); // 0.5초뒤에
            --curStage;
            stages[curStage].SetActive(true); // 이전 스테이지 활성화
        }
    }

    // 스테이지별 텍스트&버튼 설정
    void StageText()
    {
        if (gameState == State.Space)
        {
            if (curStage == 1)
            {
                titleText.text = "종이행성";
                AdvBtn.SetActive(true); // 모험하기 버튼 활성화
            }
            else if (curStage == 2)
            {
                titleText.text = "곧 업데이트 할 예정입니다.";
                AdvBtn.SetActive(false); // 모험하기 버튼 비활성화
            }
            else
            {
                AdvBtn.SetActive(false); // 모험하기 버튼 비활성화
            }
        }
    }
}