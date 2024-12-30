using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinnerPanel : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI winner,rewarded,score,gem;
    [SerializeField] private Button skipButton, continueButton;
    [SerializeField] private GameObject[] tanks;

    public static WinnerPanel Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {        
        
        continueButton.onClick.AddListener(() => { OnContinue(); });
        skipButton.onClick.AddListener(() => { Skip(); });
    }

 


    private void OnContinue()
    {
        tanks[GameData.winnerId - 1].SetActive(false);
        ScreenTransController.Instance.OpenScene(ScreenTransController.STAGE.HOME);
    }

    public void Show()
    {
        int index = GameData.winnerId - 1;
        index = index < 0 ? 0 : index;
        tanks[index].SetActive(true);
        //winner.text = GameData.winnerName;
        score.text = GameData.playerState.score.ToString();
        rewarded.text = GameData.rewarded.ToString(); 
        GameData.SaveData();
    }
    public void Skip()
    {
       
    }
    public void Continue()
    {

    }



}
