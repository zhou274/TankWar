using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MessageBox : MonoBehaviour
{
    [SerializeField]private Button btnOk;
    [SerializeField] private TextMeshProUGUI txtMsg;

    private RectTransform rect;

    public static MessageBox Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        btnOk.onClick.AddListener(()=> {
            StartCoroutine(AnimationHelper.IButtonClick(btnOk, () => {
                Hide();
            }));
        });
    }

    public void Show(string msg)
    {
        txtMsg.text = msg;
        rect.anchoredPosition = Constants.SHOW_VECTOR;     
        StartCoroutine(AnimationHelper.IScaleAnim(rect,Constants.ZERO_VECTOR, Constants.ONE_VECTOR, 0.3f));        
    }
    public void Hide()
    {
        StartCoroutine(AnimationHelper.IScaleAnim(rect, Constants.ONE_VECTOR, Constants.ZERO_VECTOR, 0.3f));
              
    }

}
