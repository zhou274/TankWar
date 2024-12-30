using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpName, tmpPower;
    [SerializeField] private RectTransform rect;
    [SerializeField] private RectTransform logo;
    private Vector2 initPosLogo=new Vector2(538,-165),initPos=new Vector2(528,-118);

    public static PlayerInfoPanel Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {        
        Hide();
    }

    public void Show()
    {
        rect.anchoredPosition = initPos;
        logo.anchoredPosition = Constants.HIDE_VECTOR;
    }
    public void DataUpdate(string name, string power)
    {
        tmpName.text = name;
        tmpPower.text = power;
    }
    public void Hide()
    {
        rect.anchoredPosition = Constants.HIDE_VECTOR;
        logo.anchoredPosition = initPosLogo;
    }
}

   
