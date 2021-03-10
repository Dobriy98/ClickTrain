using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdComponent : AdManager
{
    [SerializeField] private Text coalValueText, goldValueText, plusGoldText;


    public void TakeText()
    {
        string maxCoat = GMScript.ShowValues("maxCoat").ToString();
        string plusGold = GMScript.ShowValues("plusGold").ToString();

        coalValueText.text = maxCoat;
        goldValueText.text = plusGold;
        plusGoldText.text = plusGold;

        ShowPanel();
    }

    public void CollectButton()
    {
        int maxcoat = GMScript.ShowValues("maxCoat");
        GMScript.ChangeValues("coatMinus", maxcoat);

        int plusGold = GMScript.ShowValues("plusGold");
        GMScript.ChangeValues("gold", plusGold);

        HidePanel();
    }
}
