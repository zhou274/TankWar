using StarkSDKSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransController : MonoBehaviour
{
    public static ScreenTransController Instance;
    public GameObject gameAllObjs;

    public RectTransform homeZoomRect, gameZoomRect, gameOverZoomRect;
    public Transform backDialog;

    public GameObject gameUI, homeUI, gameOverUI;

    public enum STAGE { HOME,  GAME_CHOSE, UPGRADE,UPGRADE_MSG, GAME, GAMEOVER };

    public STAGE myStage;
    private STAGE prevStage;

    private IEnumerator iAnim;
    private float lastBackTime;

    private StarkAdManager starkAdManager;

    public string clickid;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        OpenScene(myStage);
        InitStage(myStage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            BackButtonHandler();

        if (lastBackTime > 0)
        {
            lastBackTime -= Time.deltaTime;
        }
        if (lastBackTime < 0)
        {
            lastBackTime = 0;
            backDialog.gameObject.SetActive(false);
        }
    }

    public void OpenScene(STAGE stage)
    {
        prevStage = myStage;
        if (iAnim == null)
        {
            iAnim = IAnim(stage);
            StartCoroutine(iAnim);
        }

        myStage = stage;

    }

    public void ChangeStage(STAGE stage)
    {
        myStage = stage;

        switch (myStage)
        {
            case STAGE.GAME_CHOSE:
                GameType.Instance.Hide();
                UpgradePanel.Instance.Hide();
                ChoseTankPanel.Instance.Show();
                PlayerInfoPanel.Instance.Show();
                break;
            case STAGE.UPGRADE:
                UpgradePanel.Instance.Show();
                ChoseTankPanel.Instance.Hide();
                break;
            case STAGE.HOME:
                ChoseTankPanel.Instance.Hide();
                GameType.Instance.Show();
                break;
            
        }
    }

    private void GameScene(bool isOpen)
    {
        gameUI.SetActive(isOpen);
        gameAllObjs.SetActive(isOpen);
        if (isOpen)
        {
            GameController.Instance.Open();
            SoundManager.Instance.PlayGameBackground();
        }

    }

    private void HomeScene(bool isOpen)
    {
        homeUI.SetActive(isOpen);
        if (isOpen)
        {
            GameType.Instance.Show();
            ChoseTankPanel.Instance.Hide();
            UIController.Instance.UpdateUI();
            SoundManager.Instance.PlayHomeBackground();
        }

    }

    private void GameoverScene(bool isOpen)
    {
        gameOverUI.SetActive(isOpen);


        if (isOpen)
        {
            WinnerPanel.Instance.Show();
            SoundManager.Instance.PlayHomeBackground();
        }
        ShowInterstitialAd("29godvm0htif5d19d3",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });

    }


    private void BackButtonHandler()
    {

        switch (myStage)
        {
            case STAGE.HOME:            
                if (lastBackTime <= 0)
                {
                    backDialog.gameObject.SetActive(true);
                    lastBackTime = 2;
                }
                else
                {
                    Application.Quit();
                }
                break;
            case STAGE.GAME_CHOSE:
                //GameType.Instance.Show();
                //ChoseTankPanel.Instance.Hide();
                myStage = STAGE.HOME;
                ChangeStage(myStage);
                break;
            case STAGE.GAME:
                if (!Confirmation.Instance.isOpen)
                    GameSettings.Instance.ShowOrHide();
                else
                {
                    Confirmation.Instance.Hide();
                    Invoke("ShowSettings", 0.3f);
                }
                break;

            case STAGE.UPGRADE:
                ChangeStage(STAGE.GAME_CHOSE);

                break;
            case STAGE.UPGRADE_MSG:
                MessageBox.Instance.Hide();
                myStage = STAGE.UPGRADE;
                break;
        }
    }

    private void ShowSettings()
    {
        GameSettings.Instance.Show();
    }

    private void InitStage(STAGE stage)
    {
        switch (stage)
        {
            case STAGE.HOME:
                HomeScene(true);
                GameScene(false);
                GameoverScene(false);
                break;
            case STAGE.GAME:
                HomeScene(false);
                GameScene(true);
                GameoverScene(false);
                break;

            case STAGE.GAMEOVER:
                HomeScene(false);
                GameScene(false);
                GameoverScene(true);
                break;
        }
    }

    IEnumerator IAnim(STAGE stage)
    {
        RectTransform fromZoom = null, toZoom = null;
        switch (prevStage)
        {
            case STAGE.HOME:
            case STAGE.GAME_CHOSE:            
                fromZoom = homeZoomRect;
                break;
            case STAGE.GAME:
                fromZoom = gameZoomRect;
                break;
            case STAGE.GAMEOVER:
                fromZoom = gameOverZoomRect;
                break;
        }

        Vector3 from = Vector3.zero;
        Vector3 to = new Vector3(30, 30, 1);


        fromZoom.localScale = from;
        float duration = 2f, t = 0;
        while (Vector3.Distance(fromZoom.localScale, to) > 0.2f)
        {
            t += Time.deltaTime;
            fromZoom.localScale = Vector3.Lerp(fromZoom.localScale, to, t / duration);
            yield return 0;
        }


        switch (stage)
        {
            case STAGE.HOME:
                toZoom = homeZoomRect;
                GameScene(false);
                HomeScene(true);
                GameoverScene(false);


                break;
            case STAGE.GAME:
                toZoom = gameZoomRect;
                GameScene(true);
                HomeScene(false);
                break;

            case STAGE.GAMEOVER:
                toZoom = gameOverZoomRect;
                GameScene(false);
                GameoverScene(true);
                break;
        }

        from = new Vector3(30, 30, 1);
        to = Vector3.zero;
        duration = 2f;
        t = 0;
        toZoom.localScale = from;
        while (Vector3.Distance(toZoom.localScale, to) > 0.2f)
        {
            t += Time.deltaTime;
            toZoom.localScale = Vector3.Lerp(toZoom.localScale, to, t / duration);
            yield return 0;
        }

        toZoom.localScale = Vector3.zero;



        iAnim = null;
    }




    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
    }

}
