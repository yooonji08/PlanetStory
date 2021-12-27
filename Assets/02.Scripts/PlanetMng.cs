using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlanetState
{
    Default,   // 처음
    Talk,      // 대화
    Mission,   // 미션 시작
    Result     // 미션 완료
}

public class PlanetMng : MonoBehaviour
{
    static public PlanetMng instance; // 싱글톤 인스턴스 (다른 스크립트에서 PlanetMng를 사용하고 싶을 때를 고려하여 변수 선언)
    PlayerCtrl player; // PlayerCtrl스크립트 변수

    public GameObject[] paperPlanet;  // 행성안의 지역 배열
    public static int curArea = 0;    // 현재 행성 안의 지역 인덱스값 (0 : 처음 지역 ~ 8 : 마지막 지역)
                                      
    public GameObject talkBtn;        // 대화하기 버튼
    public GameObject talkCanvas;     // 대화 상자
    int isClickNum = 0;               // 다음 대화로 넘어가기 버튼을 누른 횟수

    public Text nameText;             // 대화하는 사람의 이름
    public Text boxText;              // 대화 박스 텍스트
    public GameObject nextTalkBtn;    // 다음 대화로 넘어가기 버튼

    public GameObject missiontext;     // 미션 텍스트
    public GameObject itemnumtext;        // 획득한 아이템 개수 텍스트
    public Text missionText;          
    public Text itemNumText;          

    public GameObject missionClearText;     // 미션 클리어 텍스트
    public GameObject homeButton;     // 미션 클렁 후, 홈 화면으로 이동 버튼

    public static bool isGoAnother = false; // 다른 지역으로 이동한 상태 확인

    public PlanetState planetState = PlanetState.Default; // 현재 행성안의 상태

    string personName = "우주인"; // 플레이어 이름
    string[] animalNames = { "강아지", "토끼", "돼지", "말" }; // 종이행성에 있는 동물들 이름
    string[] paperPlanetTexts = { // 종이행성에서 나눈 대화
        "이봐! 이거 누가 그런건지 알아 ?", "어떤 것을 말하는 거야?", "여기 있던 꽃 말이야.\n누가 밟았는지 다 찌그러져 있잖아",
        "뭐야뭐야. 진짜네??\n아니 내 비상식량이였는데!!", "아니 토끼야..\n애초에 먹을 생각을 하면 안 되지..", "아무튼 이 꽃은 우리에게 매우 소중한 꽃이잖아\n1년에 한 두번 필까하는 꽃인데..",
        "맞아.. 진짜 슬프다..", "저기 얘들아.. 정말 미안해\n사실 내가 신나게 뛰어다니가 실수로 밟았어..", "뭐야 너였어?? 이걸 어쩔꺼야.\n난 맨날 이 꽃을 보러 오는게 낙이였단말이야",
        "너가 당장 책임져!!!", "강아지야 진정해 진정.\n다들 알다시피 다음 꽃을 보려면 1년은 기다려야 하는 거 알잖아..", "그래그래. 그리고 말도 계속 미안해 하고 있잖아 너무 그러지 마",
        "흥 어차피 죽은 꽃이 다시 살아나는 것도 아니잖아.", "얘들아 여기에는 이 꽃 하나밖에 안자라는 거야?", "뭐야뭐야. 수상한 인간이다. 넌 누구야?",
        "앗 미안해. 안녕 난 떠돌이 탐험가야.", "너희 얘기를 우연히 들었는데, 내가 도와줄 수 있는 방법이 없을까?", "아하! 떠돌이 탐험가여 만나서 반가워.\n사실 이 꽃은 여기 말고도 다른 곳에서도 볼 수 있어.",
        "맞아. 강 건너에 있는 산에는 꽃이 매우 많단다.", "하지만, 우리는 종이로 이루어져서, 강 건너에 있는 꽃들을 자세히 볼 수 없단다.", "그럼 내가 그 쪽으로 갔다 올께. 나는 종이가 아니여서 강을 건널 수 있거든."
    };
    string[] missions = { // 미션
        "4개의 꽃을 찾으시오"
    };
    string[] noClearMissions = { // 미션 완수 못했을 경우
        "음.. 아직 꽃을 모으는 중인 거지?",
        "괜찮아, 천천히 다녀와."
    };
    string[] missionTexts = { // 미션 완수했을 경우
        "인간.. 좀 감동했어\n덕분에 예쁜 꽃을 더 가까이서 볼 수 있게 됐네\n정말 고마워",
        "맞아 덕분이야 정말 고마워!!", "와 꽃을 가져왔구나..!\n넌 내 은인이야", "날 구해준 너에게 선물을 주고 싶어",
        "바로 내 몸에서 떼어낸 종이야..!\n좀 아프지만, 내가 줄 수 있는 최고의 선물은 종이뿐인걸..", "언젠간 종이가 쓰일날이 오길 바랄께, 안녕"
    };
    int missionNum = 0; // 수행중인 미션 인덱스

    public static int getCurArea() // curArea의 get함수
    {
        return curArea;
    }

    public static void setCurArea(int newCurArea) // curArea의 set함수
    {
        curArea = newCurArea;
    }

    void Start()
    {
        if (instance == null) // 싱글톤이 null일경우
        {
            instance = this; // 자기 자신을 할당
        }
    }

    public void GoAnotherArea()
    {
        if (planetState == PlanetState.Default)
        {
            talkBtn.gameObject.SetActive(false); // 대화하기 버튼 비활성화
            print("다른 지역으로 이동해서 비활성화");
        }
        paperPlanet[curArea].SetActive(true);
    }

    // 행성 안에서 TalkButton을 누른다면
    public void OnClickTalkButton()
    {
        talkBtn.gameObject.SetActive(false); // 대화하기 버튼 비활성화

        planetState = PlanetState.Talk; // 대화 상태로 바꾸기
        talkCanvas.SetActive(true);     // 대화 상자 활성화
        talkInteraction1();
    }

    // 대화하기 인터랙션(종이행성)
    void talkInteraction1()
    {
        if (planetState == PlanetState.Talk)
        {
            // 다음 대화로 넘어가기 버튼을 클릭한 횟수에 따라서 텍스트 다르게 나타내기
            switch (isClickNum)
            {
                case 0: nameText.text = animalNames[0]; break;
                case 1: nameText.text = animalNames[2]; break;
                case 2: nameText.text = animalNames[0]; break;
                case 3: nameText.text = animalNames[1]; break;
                case 4: case 5: nameText.text = animalNames[0]; break;
                case 6: nameText.text = animalNames[2]; break;
                case 7: nameText.text = animalNames[3]; break;
                case 8: case 9: nameText.text = animalNames[0]; break;
                case 10: nameText.text = animalNames[2]; break;
                case 11: nameText.text = animalNames[1]; break;
                case 12: nameText.text = animalNames[0]; break;
                case 13: nameText.text = personName; break;
                case 14: nameText.text = animalNames[1]; break;
                case 15: case 16: nameText.text = personName; break;
                case 17: nameText.text = animalNames[1]; break;
                case 18: case 19: nameText.text = animalNames[2]; break;
                case 20: nameText.text = personName; break;
            }
            boxText.text = paperPlanetTexts[isClickNum]; // 대화 텍스트 할당
        }
    }

    // 행성 안에서 NextTalkButton을 누른다면
    public void OnClickNextTalkButton()
    {
        if (planetState == PlanetState.Talk && isClickNum < paperPlanetTexts.Length - 1) // 클릭한 횟수가 대화박스 개수보다 작다면
        {
            ++isClickNum; // 클릭한 횟수 증가
            talkInteraction1();
        }
        else if (planetState == PlanetState.Result && isClickNum < missionTexts.Length - 1) // 미션 완료한 상태라면
        {
            ++isClickNum; // 클릭한 횟수 증가
            getClearMission1();
        }
        else // 모든 대화를 마쳤다면
        {
            talkCanvas.SetActive(false); // 대화 상자 끄기
            isClickNum = 0; // 대화버튼 클릭횟수 초기화

            if (planetState == PlanetState.Talk) // 대화 상태라면
            {
                planetState = PlanetState.Mission; // 미션 상태로 바꾸기
                missiontext.SetActive(true); // 미션 나타내기
                itemnumtext.SetActive(true); // 획득한 아이템 개수 나타내기
                missionText.text = missions[missionNum];
                itemNumText.text += " : " + PlayerCtrl.itemnum.ToString();
            }// 꽃 다 모은 이후에 다가가면
            else if (planetState == PlanetState.Result) // 미션 완료 상태라면
            {
                missionClearText.SetActive(true); // 미션 클리어 텍스트 활성화
                missiontext.SetActive(false); // 미션 텍스트 비활성화
                homeButton.SetActive(true); // 홈 화면으로 이동 버튼 활성화

                PlayerCtrl.plusItemnum(1); // 획득한 아이템 개수 증가(미션 완료보상 : 종이)
                itemNumText.text = "획득한 아이템 : " + PlayerCtrl.itemnum.ToString();
            }
        }
    }

    // 미션을 완수하지 못한 경우, 아직 부족하다는 텍스트 출력
    public void noClearMission()
    {
        talkCanvas.SetActive(true); // 대화 상자 활성화
        nextTalkBtn.SetActive(false); // 다음 대화버튼 비활성화
        nameText.text = animalNames[Random.Range(0, animalNames.Length)]; // 대화하는 사람 이름 랜덤 출력
        boxText.text = noClearMissions[Random.Range(0,noClearMissions.Length)]; // 대화 텍스트 랜덤 출력
        Invoke("hideTalkCanvas", 3f); // 3초 후 대화 상자 비활성화
    }

    void hideTalkCanvas()
    {
        talkCanvas.SetActive(false);
    }

    // 미션을 완수했을 경우
    public void getClearMission1()
    {
        talkCanvas.SetActive(true);     // 대화 상자 활성화
        nextTalkBtn.SetActive(true); // 다음 대화버튼 활성화

        // 다음 대화로 넘어가기 버튼을 클릭한 횟수에 따라서 텍스트 다르게 나타내기
        switch (isClickNum)
        {
            case 0: nameText.text = animalNames[0]; break;
            case 1: nameText.text = animalNames[1] + ", " + animalNames[2]; break;
            case 2: case 3: case 4: case 5: nameText.text = animalNames[3]; break;
        }
        boxText.text = missionTexts[isClickNum]; // 대화 텍스트 할당
    }

    // 미션 완료 후, 홈 버튼을 눌렀다면
    public void OnClickHomeButton()
    {
        missionNum = 0; // 미션 완료 초기화
        SceneManager.LoadScene("PlanetStory"); // 메인으로 이동
    }
}
