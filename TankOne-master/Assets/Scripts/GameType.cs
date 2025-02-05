﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;
using UnityEngine.Analytics;

public class GameType : MonoBehaviour
{
    [SerializeField] private Button btnFriend, btnComputer,btnOnline;
    [SerializeField] RectTransform rect;
    public string clickid;
    private StarkAdManager starkAdManager;


    public static GameType Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        btnFriend.onClick.AddListener(() => { OnFriend(); });
        btnComputer.onClick.AddListener(() => { OnComputer(); });

       

    }
   
    private void OnFriend()
    {
        StartCoroutine(AnimationHelper.IButtonClick(btnFriend, () => { FriendCallback(); }));


    }
    private void FriendCallback()
    {

        ShowVideoAd("bsjpciqholc3ke7h25",
            (bol) => {
                if (bol)
                {

                    GameData.isPlayWithAI = false;

                    ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.GAME_CHOSE);


                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });

        

    }
    private void OnComputer()
    {
        StartCoroutine(AnimationHelper.IButtonClick(btnComputer, () => { ComputerCallback(); }));
    }

    private void ComputerCallback()
    {
        
        GameData.isPlayWithAI = true;
        ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.GAME_CHOSE);

    }
    public void Hide()
    {
        rect.anchoredPosition = new Vector2(2000,0);
    }
    public void Show()
    {
        rect.anchoredPosition = Vector2.zero;
    }


    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }
}
