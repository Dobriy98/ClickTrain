using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlanetTap planetTapScript;
    [SerializeField] private ToolTip toolTipScript;
    [SerializeField] private List<GameObject> asteroids = new List<GameObject>();
    [SerializeField] private Slider sliderTemp;
    [SerializeField] private Image sliderTempImage;
    [SerializeField] private AudioSource audioSourceTrain;
    [SerializeField] private RectTransform panelTimer;
    [SerializeField] private TextMeshProUGUI timerGoldText;
    [SerializeField] private TextMeshProUGUI coat;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private GameObject panelGold;
    [SerializeField] private GameObject buttonGold;
    [SerializeField] private Button buttonConvert;

    [SerializeField] private Text priceCoal;
    [SerializeField] private Text priceGold;
    [SerializeField] private GameObject panelGoldForUpgrade;
    [SerializeField] private GameObject panelForUpgrade;
    [SerializeField] private GameObject endPanel;

    [SerializeField] private GameObject[] rocket;
    [SerializeField] private Button[] buttonsToUpgrade;
    [SerializeField] private Button[] upgradeButtons;

    private int tempValue;
    private int coatValue;
    private int goldValue;

    private int diffSeconds;
    private int secondsToUpdateAsteroid = 30;
    private static int secondsToUpdateGold = 600;

    private static int minsToUpdate = 1;
    private static int maxTemp = 100;
    private int maxCoat;
    private int maxGold;

    private FuncTimer functionTimerGold;

    private Animation anim;

    private int plusGold;
    private int lvlTrain;
    private int plusCoat;
    private int lvlRocket;

    //Цены на прокачку Угля
    private int priceCoatForCoat = 0;
    private int priceGoldForCoat = 0;

    //Цены на прокачку Золота
    private int priceCoatForGold;
    private int priceGoldForGold;


    //Цены на прокачку Рокеты
    private int[] priceCoatForRocket = new int[]{
        500, 1000, 1500
    };
    private int[] priceGoldForRocket = new int[]{
        50, 100, 150
    };

    void Start()
    {
        anim = GameObject.Find("Train_001").GetComponent<Animation>();
        TakeValues();

        #region CheckTime
        DateTime endDate = Utils.GetDateTime("LastSaveDate", DateTime.UtcNow);
        TimeSpan diff = DateTime.UtcNow - endDate;
        diffSeconds = (int)diff.TotalSeconds;
        diffSeconds = Mathf.Clamp(diffSeconds, 0, 7 * 24 * 60 * 60);

        if (plusGold >= 1)
        {
            functionTimerGold = new FuncTimer(PlusGold, secondsToUpdateGold, "goldRemainder");
            CheckSeconds(diffSeconds, secondsToUpdateGold, functionTimerGold, "gold");
        }
        #endregion

        ChangeTempValue(diffSeconds);
        StartCoroutine("AsteroidAnim");
    }


    void Update()
    {
        CheckInteractable();
        if (plusGold >= 1)
        {
            functionTimerGold.Update();
            GoldTimer(functionTimerGold.timerValue);
        }
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
        else
        {
            Start();
        }
    }

    private void GoldTimer(float value)
    {
        int mins = (int)(value / 60);
        string strToText = " mins";
        if (mins == 1)
        {
            strToText = " min";
        }
        else if (mins == 0)
        {
            strToText = "<1 min";
        }
        timerGoldText.text = (mins == 0 ? null : mins.ToString()) + strToText;
        panelTimer.localScale = new Vector3(value / secondsToUpdateGold, 1, 1);
    }


    private void CheckSeconds(int diff, int secondsToUpdate, FuncTimer funcTimer, string nameValue)
    {
        int remainder;
        int result = Math.DivRem(diff, secondsToUpdate, out remainder);

        if (result != 0)
        {
            result *= plusGold;
            ChangeValues(nameValue, result);
        }
        else
        {
            ChangeValues("", result);
        }
        funcTimer.timerValue -= remainder;
    }

    private void Save()
    {
        Utils.SetDateTime("LastSaveDate", DateTime.UtcNow);

        //SetValues
        PlayerPrefs.SetInt("tempValue", tempValue);
        PlayerPrefs.SetInt("Coat", coatValue);
        PlayerPrefs.SetInt("Gold", goldValue);
        PlayerPrefs.SetInt("plusGold", plusGold);
        PlayerPrefs.SetInt("maxCoat", maxCoat);
        PlayerPrefs.SetInt("lvlTrain", lvlTrain);
        PlayerPrefs.SetInt("plusCoat", plusCoat);
        PlayerPrefs.SetInt("maxGold", maxGold);
        PlayerPrefs.SetInt("lvlRocket", lvlRocket);

        //Prices
        PlayerPrefs.SetInt("priceCoatForCoat", priceCoatForCoat);
        PlayerPrefs.SetInt("priceGoldForCoat", priceGoldForCoat);

        PlayerPrefs.SetInt("priceCoatForGold", priceCoatForGold);
        PlayerPrefs.SetInt("priceGoldForGold", priceGoldForGold);

        //Remainders
        if (plusGold >= 1)
        {
            functionTimerGold.SetTimerFromPrefs("goldRemainder");
        }
    }

    private void TakeValues()
    {
        //Temperature
        tempValue = PlayerPrefs.GetInt("tempValue", 0);

        //Coat
        coatValue = PlayerPrefs.GetInt("Coat", 0);
        if (coatValue <= 0)
        {
            audioSourceTrain.Stop();
            anim["Train_animation_1"].speed = 0;
        }
        else
        {
            audioSourceTrain.Play();
            anim["Train_animation_1"].speed = 1;
        }

        //Gold
        goldValue = PlayerPrefs.GetInt("Gold", 0);

        //PlusGold
        plusGold = PlayerPrefs.GetInt("plusGold", 0);

        //lvlTrain
        lvlTrain = PlayerPrefs.GetInt("lvlTrain", 0);
        if (lvlTrain > 0)
        {
            panelGold.SetActive(true);
            buttonGold.SetActive(true);
            buttonConvert.gameObject.SetActive(true);
        }

        // + Coat
        plusCoat = PlayerPrefs.GetInt("plusCoat", 1);
        planetTapScript.plusCoat = plusCoat;

        //MaxValues
        maxCoat = PlayerPrefs.GetInt("maxCoat", 100);
        maxGold = PlayerPrefs.GetInt("maxGold", 10);

        lvlRocket = PlayerPrefs.GetInt("lvlRocket", 0);
        if (lvlRocket <= 2)
        {
            buttonsToUpgrade[lvlRocket].interactable = true;
        }

        if (lvlRocket != 0)
        {
            for (int i = 0; i < lvlRocket; i++)
            {
                buttonsToUpgrade[i].gameObject.SetActive(false);
                rocket[i].SetActive(true);
            }
        }

        //Prices
        priceCoatForCoat = PlayerPrefs.GetInt("priceCoatForCoat", 50);
        priceGoldForCoat = PlayerPrefs.GetInt("priceGoldForCoat", 2);

        priceCoatForGold = PlayerPrefs.GetInt("priceCoatForGold", 100);
        priceGoldForGold = PlayerPrefs.GetInt("priceGoldForGold", 4);
    }

    private void MinusTemp()
    {
        tempValue--;
    }

    private void PlusGold()
    {
        if (goldValue < maxGold)
        {
            int xGold = CheckXGold(tempValue);
            goldValue += (plusGold * xGold);
            goldValue = Mathf.Clamp(goldValue, 0, maxGold);
            gold.text = goldValue.ToString() + "/" + maxGold.ToString();
        }
    }

    IEnumerator AsteroidAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsToUpdateAsteroid);
            AsteroidAction();
            secondsToUpdateAsteroid += 60;
        }
    }
    private void AsteroidAction()
    {
        int rand = plusGold >= 1 ? UnityEngine.Random.Range(0, asteroids.Count) : 0;
        asteroids[rand].SetActive(true);
    }


    private void ChangeTempValue(int diff)
    {
        int mins = Mathf.FloorToInt(diff / (60 * minsToUpdate));

        int xValue = maxCoat / 100;

        int remainder = coatValue - (mins * xValue);
        if (remainder < 0)
        {
            tempValue = Mathf.Clamp(tempValue + coatValue + remainder, -maxTemp, maxTemp);
            coatValue = 0;
        }
        else
        {
            tempValue = Mathf.Clamp(tempValue + mins, -maxTemp, maxTemp);
            coatValue -= mins;
        }
        ChangeValues("", 0);
        sliderTemp.value = tempValue;

        if (tempValue > 0)
        {
            sliderTempImage.color = new Color32(255, 50, 60, 255);
        }
        else
        {
            sliderTempImage.color = new Color32(50, 60, 255, 255);
        }
    }

    public void BuildMine()
    {
        if (plusGold == 0)
        {
            functionTimerGold = new FuncTimer(PlusGold, secondsToUpdateGold, "goldRemainder");
        }

        plusGold += 1;
        PlayerPrefs.SetInt("plusGold", plusGold);
    }

    public void ChangeValues(string nameValue, int res)
    {
        switch (nameValue)
        {
            case "temp":
                if (tempValue > -maxTemp)
                {
                    tempValue -= res;
                    //temp.text = tempValue.ToString();
                }
                break;
            case "tempPlus":
                if (tempValue < maxTemp)
                {
                    tempValue += res;
                    //temp.text = tempValue.ToString();
                }
                break;
            case "coat":
                if (coatValue < maxCoat)
                {
                    coatValue += res;
                    coatValue = Mathf.Clamp(coatValue, 0, maxCoat);
                    coat.text = coatValue.ToString() + "/" + maxCoat.ToString();
                    if (!audioSourceTrain.isPlaying)
                    {
                        audioSourceTrain.Play();
                    }
                    anim["Train_animation_1"].speed = 1;
                }
                break;
            case "coatMinus":
                if (coatValue > 0)
                {
                    coatValue -= res;
                    coat.text = coatValue.ToString() + "/" + maxCoat.ToString();
                    if (coatValue <= 0)
                    {
                        audioSourceTrain.Stop();
                        anim["Train_animation_1"].speed = 0;
                    }
                }
                break;
            case "gold":
                if (goldValue < maxGold)
                {
                    goldValue += res;
                    goldValue = Mathf.Clamp(goldValue, 0, maxGold);
                    gold.text = goldValue.ToString() + "/" + maxGold.ToString();
                }
                break;
            case "goldMinus":
                if (goldValue > 0)
                {
                    goldValue -= res;
                    gold.text = goldValue.ToString() + "/" + maxGold.ToString();
                }
                break;
            default:
                coat.text = coatValue.ToString() + "/" + maxCoat.ToString();
                gold.text = goldValue.ToString() + "/" + maxGold.ToString();
                break;
        }
    }

    public int ShowValues(string name)
    {
        switch (name)
        {
            case "temp":
                return tempValue;
            case "coat":
                return coatValue;
            case "plusCoat":
                return plusCoat;
            case "maxCoat":
                return maxCoat;
            case "plusGold":
                return plusGold;
            case "maxGold":
                return maxGold;
            default:
                return 0;
        }
    }

    public void UpgradeTrain()
    {
        if (lvlTrain == 0)
        {
            //Стоимость
            int priceCoatLvl1 = maxCoat;

            if (coatValue >= priceCoatLvl1)
            {
                //Изменения
                maxCoat = 200;
                panelGold.SetActive(true);
                buttonGold.SetActive(true);
                buttonConvert.gameObject.SetActive(true);
                BuildMine();
                gold.text = goldValue.ToString() + "/" + maxGold.ToString();
                lvlTrain += 1;

                ChangeValues("coatMinus", priceCoatLvl1);
            }
        }
        else if (lvlTrain >= 1)
        {
            //Стоимость
            int priceCoatLvl2 = maxCoat;
            int priceGoldLvl2 = maxGold;

            if (coatValue >= priceCoatLvl2 && goldValue >= priceGoldLvl2)
            {
                maxCoat += 100;
                maxGold += 10;

                lvlTrain += 1;

                ChangeValues("coatMinus", priceCoatLvl2);
                ChangeValues("goldMinus", priceGoldLvl2);
            }
        }
    }
    public void UpgradeCoat()
    {
        if (coatValue >= priceCoatForCoat && goldValue >= priceGoldForCoat)
        {

            ChangeValues("coatMinus", priceCoatForCoat);
            ChangeValues("goldMinus", priceGoldForCoat);

            priceCoatForCoat += 50;
            priceGoldForCoat += 2;

            plusCoat += 1;
            planetTapScript.plusCoat = plusCoat;

            if (toolTipScript != null)
            {
                toolTipScript.HideToolTip();
            }

        }
    }

    public void UpgradeGold()
    {
        if (coatValue >= priceCoatForGold && goldValue >= priceGoldForGold)
        {

            ChangeValues("coatMinus", priceCoatForGold);
            ChangeValues("goldMinus", priceGoldForGold);

            priceCoatForGold += 100;
            priceGoldForGold += 4;

            plusGold += 1;

            gold.text = goldValue.ToString() + "/" + maxGold.ToString();
            
            if (toolTipScript != null)
            {
                toolTipScript.HideToolTip();
            }

        }
    }
    private int CheckXGold(int moodValue)
    {
        if (moodValue < 0)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public string[] InfForTip(string name)
    {
        string[] tipText = new string[2];
        switch (name)
        {
            case "UpgradeCoat":
                panelForUpgrade.SetActive(true);
                priceCoal.text = priceCoatForCoat.ToString();
                priceGold.text = priceGoldForCoat.ToString();
                panelGoldForUpgrade.SetActive(true);
                tipText[0] = "To " + plusCoat.ToString() + " level";
                tipText[1] = "+ 1 coal per click";
                break;
            case "UpgradeGold":
                panelForUpgrade.SetActive(true);
                priceCoal.text = priceCoatForGold.ToString();
                priceGold.text = priceGoldForGold.ToString();
                panelGoldForUpgrade.SetActive(true);
                tipText[0] = "To " + plusGold.ToString() + " level";
                tipText[1] = "+ 1 gold every 10 min";
                break;
            case "UpgradeTrain":
                panelForUpgrade.SetActive(true);
                priceCoal.text = maxCoat.ToString();
                if (plusGold > 0)
                {
                    panelGoldForUpgrade.SetActive(true);
                    priceGold.text = maxGold.ToString();
                    tipText[0] = "To " + (lvlTrain + 1).ToString() + " level";
                    int toMax = maxCoat + 100;
                    int toMaxGold = maxGold + 10;
                    tipText[1] = "Coal Storage to " + toMax.ToString() + "\nGold Storage to " + toMaxGold.ToString();
                }
                else
                {
                    panelGoldForUpgrade.SetActive(false);
                    tipText[0] = "To 1 level";
                    int toMax = maxCoat + 100;
                    tipText[1] = "+ Mine with gold\nCoal Storage to " + toMax.ToString();
                }
                break;
            case "CoalPanel":
                panelForUpgrade.SetActive(false);
                tipText[0] = "Coal";
                tipText[1] = plusCoat.ToString() + " per click\n ";
                break;
            case "GoldPanel":
                panelForUpgrade.SetActive(false);
                float xGold = CheckXGold(tempValue);
                tipText[0] = "Gold";
                tipText[1] = (plusGold * xGold).ToString() + " every " + (secondsToUpdateGold / 60).ToString() + " min\n ";
                break;
            case "UpgradeRocketBottom":
                panelForUpgrade.SetActive(true);
                priceCoal.text = priceCoatForRocket[0].ToString();
                priceGold.text = priceGoldForRocket[0].ToString();
                panelGoldForUpgrade.SetActive(true);
                tipText[0] = "Rocket Bottom Part";
                tipText[1] = "Build rocket bottom part";
                break;
            case "UpgradeRocketMiddle":
                panelForUpgrade.SetActive(true);
                priceCoal.text = priceCoatForRocket[1].ToString();
                priceGold.text = priceGoldForRocket[1].ToString();
                panelGoldForUpgrade.SetActive(true);
                tipText[0] = "Rocket Middle Part";
                tipText[1] = "Build rocket middle part";
                break;
            case "UpgradeRocketTop":
                panelForUpgrade.SetActive(true);
                priceCoal.text = priceCoatForRocket[2].ToString();
                priceGold.text = priceGoldForRocket[2].ToString();
                tipText[0] = "Rocket Top Part";
                tipText[1] = "Build rocket top part";
                break;
        }
        return tipText;
    }

    public void UpgradeRocket()
    {
        if (coatValue >= priceCoatForRocket[lvlRocket] && goldValue >= priceGoldForRocket[lvlRocket])
        {
            buttonsToUpgrade[lvlRocket].gameObject.SetActive(false);
            rocket[lvlRocket].SetActive(true);

            ChangeValues("coatMinus", priceCoatForRocket[lvlRocket]);
            ChangeValues("goldMinus", priceGoldForRocket[lvlRocket]);

            lvlRocket += 1;
            if (lvlRocket <= 2)
            {
                buttonsToUpgrade[lvlRocket].interactable = true;
            }

            if (lvlRocket == 3)
            {
                endPanel.SetActive(true);
            }

            if (toolTipScript != null)
            {
                toolTipScript.HideToolTip();
            }
        }
    }

    private void CheckInteractable()
    {
        int checkGold = (plusGold > 0 ? maxGold : 0);
        if (coatValue >= maxCoat && goldValue >= checkGold)
        {
            upgradeButtons[0].interactable = true;
        }
        else
        {
            upgradeButtons[0].interactable = false;
        }
        if (coatValue >= priceCoatForCoat && goldValue >= priceGoldForCoat)
        {
            upgradeButtons[1].interactable = true;
        }
        else
        {
            upgradeButtons[1].interactable = false;
        }
        if (plusGold > 0)
        {
            if (coatValue >= priceCoatForGold && goldValue >= priceGoldForGold)
            {
                upgradeButtons[2].interactable = true;
            }
            else
            {
                upgradeButtons[2].interactable = false;
            }
        }
        if (coatValue >= maxCoat)
        {
            buttonConvert.interactable = true;
        }
        else
        {
            buttonConvert.interactable = false;
        }
        if (lvlRocket <= 2)
        {
            if (coatValue >= priceCoatForRocket[lvlRocket] && goldValue >= priceGoldForRocket[lvlRocket])
            {
                buttonsToUpgrade[lvlRocket].interactable = true;
            }
            else
            {
                buttonsToUpgrade[lvlRocket].interactable = false;
            }
        }
    }
}
