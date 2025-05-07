using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 이 스크립트는 JSON 파일로부터 버튼 정보를 불러오고,
// 각 버튼을 클릭하면 해당 설명을 팝업으로 보여주는 역할을 합니다.

public class PopupManager : MonoBehaviour
{
    [Header("버튼 관련")]
    public GameObject buttonTemplate;  // 미리 만들어둔 버튼 템플릿 (비활성 상태)
    public Transform buttonParent;     // 버튼들을 넣을 부모 오브젝트 (레이아웃 그룹 등)

    [Header("팝업 관련")]
    public GameObject popupPanel;      // 설명을 보여줄 팝업 창
    public TMP_Text popupText;         // 팝업 안의 텍스트(UI)

    private List<PopupData> popupDataList;  // JSON에서 불러온 버튼+설명 데이터 리스트

    void Start()
    {
        // JSON 파일에서 버튼과 설명 데이터를 불러옵니다.
        LoadPopupDataFromJSON();

        // 데이터 수만큼 버튼을 만들어서 화면에 표시합니다.
        foreach (var data in popupDataList)
        {
            // 템플릿을 복제해서 새 버튼 생성
            GameObject newButton = Instantiate(buttonTemplate, buttonParent);
            newButton.SetActive(true); // 원래 비활성화된 템플릿을 활성화

            // 버튼 안 텍스트를 JSON에서 불러온 이름으로 설정
            newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = data.buttonLabel;

            // 버튼 클릭 시 해당 설명을 팝업으로 보여주도록 이벤트 연결
            newButton.GetComponent<Button>().onClick.AddListener(() => ShowPopup(data.popupText));
        }

        // 시작 시 팝업은 꺼둡니다.
        popupPanel.SetActive(false);
    }

    // Resources 폴더에 있는 popup_data.json 파일을 불러오는 함수
    void LoadPopupDataFromJSON()
    {
        // Resources 폴더에 있는 popup_data.json 파일을 불러옴
        TextAsset jsonFile = Resources.Load<TextAsset>("popup_data");

        if (jsonFile != null)
        {
            // Unity의 JsonUtility는 배열만 있는 JSON을 바로 읽지 못하기 때문에
            // {"data":[...]} 형태로 감싸줌
            string json = "{\"data\":" + jsonFile.text + "}";

            // JSON 문자열을 PopupDataList 타입으로 변환
            PopupDataList loadedData = JsonUtility.FromJson<PopupDataList>(json);
            popupDataList = loadedData.data;
        }
        else
        {
            // 파일을 찾지 못한 경우 경고 출력 + 빈 리스트 생성
            Debug.LogError("popup_data.json not found in Resources folder.");
            popupDataList = new List<PopupData>();
        }
    }

    // 팝업 창을 열고 텍스트를 설정하는 함수
    public void ShowPopup(string message)
    {
        popupText.text = message;     // 팝업 안에 들어갈 텍스트 설정
        popupPanel.SetActive(true);   // 팝업 창 보이기
    }

    // 팝업 창을 닫는 함수 (닫기 버튼 등에 연결)
    public void ClosePopup()
    {
        popupPanel.SetActive(false);  // 팝업 창 숨기기
    }
}