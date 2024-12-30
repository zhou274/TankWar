using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    [SerializeField]private ScreenTransAnim screenTrans;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        screenTrans.StartAnim();
    }
    public void GoToHome()
    {
        screenTrans.EndAnim("Home");
    }
}
