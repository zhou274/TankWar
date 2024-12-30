using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public static UpgradePanel Instance;

    public Button backButton;

    

    private RectTransform rect;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        backButton.onClick.AddListener(()=> { OnBack(); });

        Hide(); 
    }

    public void Show()
    {
        rect.anchoredPosition = Vector2.zero;
    }

    public void Hide()
    {
        rect.anchoredPosition = new Vector2(2000,0);
    }

    private void OnBack()
    {
        
        StartCoroutine(AnimationHelper.IButtonClick(backButton, () => { Back(); }));
       
    }

    private void Back()
    {
        
        ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.GAME_CHOSE);
    }

}
