using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTip : MonoBehaviour
{   
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;
    public int characterWrapLimit;

    private Animation anim;

    private void Awake() {
        anim = GetComponent<Animation>();
    }
    public void ShowToolTip(string toolTipStringContent, string toolTipStringHeader, Transform buttPos){
        gameObject.SetActive(true);
        

        int headerLenght;
        int contentLenght;

        if(string.IsNullOrEmpty(toolTipStringHeader)){
            headerField.gameObject.SetActive(false);
            headerLenght = 0;
        } else {
            headerField.gameObject.SetActive(true);
            headerField.text = toolTipStringHeader;
            headerLenght = headerField.text.Length;
        }

        if(string.IsNullOrEmpty(toolTipStringContent)){
            contentField.gameObject.SetActive(false);
            contentLenght = 0;
        } else {
            contentField.gameObject.SetActive(true);
            contentField.text = toolTipStringContent;
            contentLenght = contentField.text.Length;
        }

        //layoutElement.enabled = (headerLenght > characterWrapLimit || contentLenght > characterWrapLimit) ? true : false;
        Vector3 screen = new Vector3(Screen.width,Screen.height,0);
        float screenHeight = Camera.main.ScreenToWorldPoint(screen).y;
        if(buttPos.position.y > screenHeight/2){
            //Up Buttons
            LayoutElement le = GetComponent<LayoutElement>();
            transform.position = buttPos.position + new Vector3(1,0,0);
        } else {
            LayoutElement le = GetComponent<LayoutElement>();
            le.preferredWidth = 900;
            transform.position = buttPos.position + new Vector3(0,1.5f,0);
        }
    }

    public void HideToolTip(){
        gameObject.SetActive(false);
    }
}
