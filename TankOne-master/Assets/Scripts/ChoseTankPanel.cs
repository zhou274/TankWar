using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChoseTankPanel : MonoBehaviour
{

    public static ChoseTankPanel Instance;

    [SerializeField] private TankListView tankListView;

    [SerializeField] private RectTransform rect;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    public void Show()
    {
        rect.anchoredPosition = Constants.SHOW_VECTOR;
        TankListView.Instance.Show();
    }

    public void Hide()
    {
        rect.anchoredPosition = Constants.HIDE_VECTOR;
        TankListView.Instance.Hide();
    }
}
