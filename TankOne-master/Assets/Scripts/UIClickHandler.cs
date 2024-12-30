using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickHandler : MonoBehaviour,IPointerClickHandler
{
    public static UIClickHandler Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Handle(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        WeaponButton.Instance.HideList();
        Handle(false);
    }

   
}
