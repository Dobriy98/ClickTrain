using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class Utils
{
    public static void SetDateTime(string key, DateTime value){
        string stringDate = value.ToString("u", CultureInfo.InvariantCulture);
        PlayerPrefs.SetString(key, stringDate);
    }

    public static DateTime GetDateTime(string key, DateTime defultValue){
        if(PlayerPrefs.HasKey(key)){
            string stored = PlayerPrefs.GetString(key);
            DateTime result = DateTime.ParseExact(stored,"u",CultureInfo.InvariantCulture);
            return result;
        } else
        {
            return defultValue;
        }
    }
}
