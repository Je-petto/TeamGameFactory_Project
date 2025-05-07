using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    [Header("버튼 관련")]
    public GameObject buttonTemplate;
    public Transform buttonParent;

    [Header("팝업 관련")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    private List<PopupData> popupDataList;

    void Start()
    {
        LoadPopupDataFromJSON();

        foreach (var data in popupDataList)
        {
            GameObject newButton = Instantiate(buttonTemplate, buttonParent);
            newButton.SetActive(true);

            newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = data.buttonLabel;
            newButton.GetComponent<Button>().onClick.AddListener(() => ShowPopup(data.popupText));
        }

        popupPanel.SetActive(false);
    }

    void LoadPopupDataFromJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("popup_data");
        if (jsonFile != null)
        {
            string json = "{\"data\":" + jsonFile.text + "}";
            PopupDataList loadedData = JsonUtility.FromJson<PopupDataList>(json);
            popupDataList = loadedData.data;
        }
        else
        {
            Debug.LogError("popup_data.json not found in Resources folder.");
            popupDataList = new List<PopupData>();
        }
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}