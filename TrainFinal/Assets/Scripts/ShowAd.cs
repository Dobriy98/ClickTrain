using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShowAd : AdManager, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "3981836";
#elif UNITY_ANDROID
    private string gameId = "3981837";
#endif
    private bool testMode = false;
    private string placement = "rewardedVideo";
    public GameObject currentTarget;
    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        Advertisement.AddListener(this);
    }

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady(placement))
        {
            Advertisement.Show(placement);
        }
    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            ShowCurrentTarget();
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    private void ShowCurrentTarget()
    {
        Debug.Log(this.target);
        switch (this.target)
        {
            case AdTarget.Asteroid0:
                RestoreCoal();
                break;
            case AdTarget.Asteroid1:
                RestoreGold();
                break;
            case AdTarget.Asteroid2:
                RestoreAll();
                break;
            case AdTarget.ChangeButton:
                ChangeButt();
                break;
        }
    }

    private void RestoreCoal()
    {
        Debug.Log(currentTarget);
        if (GMScript != null)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<AdComponentAsteroid>().HidePanel();
            }

            int maxVal = GMScript.ShowValues("maxCoat");
            GMScript.ChangeValues("coat", maxVal);
        }
    }

    private void RestoreGold()
    {
        Debug.Log(currentTarget);
        if (GMScript != null)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<AdComponentAsteroid>().HidePanel();
            }

            int maxVal = GMScript.ShowValues("maxGold");
            GMScript.ChangeValues("gold", maxVal);
        }
    }

    private void ChangeButt()
    {
        Debug.Log(currentTarget);
        if (GMScript != null)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<AdComponent>().HidePanel();
            }

            int maxVal = GMScript.ShowValues("maxCoat");
            GMScript.ChangeValues("coatMinus", maxVal);

            int plusGold = GMScript.ShowValues("plusGold");
            GMScript.ChangeValues("gold", plusGold * 2);
        }
    }

    private void RestoreAll()
    {
        Debug.Log(currentTarget);
        if (GMScript != null)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<AdComponentAsteroid>().HidePanel();
            }

            int maxVal = GMScript.ShowValues("maxCoat");
            GMScript.ChangeValues("coat", maxVal);

            int maxValGold = GMScript.ShowValues("maxGold");
            GMScript.ChangeValues("gold", maxValGold);
        }
    }
}
