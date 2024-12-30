using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public static HomeManager Instance;
    public Transform backDialog;
    [SerializeField] private ScreenTransAnim screenTran;
    public AudioSource aditionalAudioSource;
    public AudioClip click;

    public enum STAGE {HOME, GAME_TYPE, GAME_CHOSE, GAME };

    public STAGE myStage;

    private float lastBackTime;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        myStage = STAGE.GAME_TYPE;

        screenTran.StartAnim();
    }
    private void Update()
    {
     if(Input.GetKeyDown(KeyCode.Escape))
        {
            BackButtonController();
        }
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
    public void GoToGame()
    {
        screenTran.EndAnim("Game");
    }
    public void PlayClickSound()
    {
        aditionalAudioSource.PlayOneShot(click);
    }

    private void BackButtonController()
    {
        switch (myStage)
        {
            case STAGE.GAME_TYPE:
                if (lastBackTime <= 0)
                {
                    backDialog.gameObject.SetActive(true);
                    lastBackTime = 2;
                }
                else
                {
                    print("Exit");
                    Application.Quit();
                }
                
                break;
            case STAGE.GAME_CHOSE:
                GameType.Instance.Show();
                ChoseTankPanel.Instance.Hide();
                myStage = STAGE.GAME_TYPE;
                break;
        }
        
    }
}
