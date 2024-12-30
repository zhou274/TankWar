using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RowUpgrade : MonoBehaviour
{

    public Button btnUpgrade;
    public TextMeshProUGUI txtInfo;
    public Type type;

    public enum Type { Speed, Power, SpeedAds, PowerAds };

    private int[] speedCoin, powerGem;

    private void Start()
    {
        speedCoin = new int[] { 100, 200, 300, 400 };
        powerGem = new int[] { 5, 10, 15, 20 };
        btnUpgrade.onClick.AddListener(() => { OnUpgrade(); });
        UpdateUI();
    }

    private void UpdateSpeedUI()
    {
        if (GameData.playerState.speedMultiplier >= speedCoin.Length)
        {
            txtInfo.text = "x" + GameData.playerState.speedMultiplier + "\nSpeed";
            btnUpgrade.gameObject.SetActive(false);
        }
        else
        {

            btnUpgrade.gameObject.SetActive(true);

            txtInfo.text = "x" + (GameData.playerState.speedMultiplier + 1) + "\nSpeed";
            int coin = speedCoin[GameData.playerState.speedMultiplier - 1];
            btnUpgrade.GetComponentInChildren<TextMeshProUGUI>().text = coin.ToString();
            btnUpgrade.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void UpdateSpeedAds()
    {
        if (GameData.playerState.speedMultiplier >= speedCoin.Length)
        {
            txtInfo.text = "x" + GameData.playerState.speedMultiplier + "\n速度";
            btnUpgrade.gameObject.SetActive(false);
        }
        else
        {

            btnUpgrade.gameObject.SetActive(true);
            txtInfo.text = "x" + (GameData.playerState.speedMultiplier + 1) + "\n速度";

        }
    }

    private void UpdatePower()
    {
        //if (GameData.playerState.powerMultiplier >= powerGem.Length)
        if (GameData.playerState.speedMultiplier >= speedCoin.Length)
        {
            txtInfo.text = "x" + GameData.playerState.powerMultiplier + "\n力量";
            btnUpgrade.gameObject.SetActive(false);
        }
        else
        {

            btnUpgrade.gameObject.SetActive(true);

            txtInfo.text = "x" + (GameData.playerState.powerMultiplier + 1) + "\n力量";
            int coin = powerGem[GameData.playerState.powerMultiplier - 1];
            btnUpgrade.GetComponentInChildren<TextMeshProUGUI>().text = coin.ToString();
            btnUpgrade.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void UpdatePowerAds()
    {
        //if (GameData.playerState.powerMultiplier >= powerGem.Length)
        if (GameData.playerState.speedMultiplier >= speedCoin.Length)
        {
            txtInfo.text = "x" + GameData.playerState.powerMultiplier + "\n力量";
            btnUpgrade.gameObject.SetActive(false);
        }
        else
        {

            btnUpgrade.gameObject.SetActive(true);
            txtInfo.text = "x" + (GameData.playerState.powerMultiplier + 1) + "\n力量";

        }
    }

    private void UpdateUI()
    {

        
        switch (type)
        {
            case Type.Speed:
                UpdateSpeedUI();
                //UpdateSpeedAds();
                break;
            case Type.SpeedAds:
                //UpdateSpeedUI();
                UpdateSpeedAds();
                break;
            case Type.Power:
                UpdatePower();
                //UpdatePowerAds();
                break;
            case Type.PowerAds:
                //UpdatePower();
                UpdatePowerAds();
                break;
        }

        UIController.Instance.UpdateUI();
    }
    private void OnUpgrade()
    {
        StartCoroutine(AnimationHelper.IButtonClick(btnUpgrade, () => { Callback(); }));
    }
    private void Callback()
    {
        switch (type)
        {
            case Type.Speed:
                int coin = GameData.playerState.score;
                int mult = GameData.playerState.speedMultiplier + 1;
                if (coin >= speedCoin[mult - 2])
                {
                    // available coin
                    GameData.playerState.speedMultiplier = GameData.playerState.speedMultiplier + 1;
                    GameData.playerState.score = GameData.playerState.score - speedCoin[mult - 2];

                    GameData.SaveData();

                    MessageBox.Instance.Show("您已成功升级速度。");
                    ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.UPGRADE_MSG);
                    UpdateUI();

                }
                else
                {
                    ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.UPGRADE_MSG);
                    MessageBox.Instance.Show("你没有足够的硬币！");
                }
                break;
            case Type.SpeedAds:
                AdsControl.Instance.showAds(() =>
                {
                    AdWatched(true);
                });
                break;
            case Type.Power:
                coin = GameData.playerState.gem;
                mult = GameData.playerState.powerMultiplier + 1;
                if (coin >= powerGem[mult - 2])
                {
                    // available coin
                    GameData.playerState.powerMultiplier = GameData.playerState.powerMultiplier + 1;
                    GameData.playerState.gem = GameData.playerState.gem - powerGem[mult - 2];

                    GameData.SaveData();

                    MessageBox.Instance.Show("您已成功升级电源。");
                    ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.UPGRADE_MSG);
                    UpdateUI();
                }
                else
                {
                    ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.UPGRADE_MSG);
                    MessageBox.Instance.Show("您没有足够的 gems!!");
                }
                break;
            case Type.PowerAds:

                AdsControl.Instance.showAds(() =>
                {
                    AdWatched(false);
                });
                break;
        }
    }

    private void AdWatched(bool isForSpeed)
    {
        if (isForSpeed)
        {
            GameData.playerState.speedMultiplier = GameData.playerState.speedMultiplier + 1;
        }
        else
        {
            GameData.playerState.powerMultiplier = GameData.playerState.powerMultiplier + 1;
        }
        GameData.SaveData();
        UpdateUI();
    }

}
