using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyButton : MonoBehaviour
{
    [SerializeField]
    private Image bg, imgLock;
    [SerializeField]
    private Sprite activeSprite, lockedSprite;
    [SerializeField]
    private TextMeshProUGUI tmTitle;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private string title;
    [SerializeField] private Animator anim;

    private void Start()
    {
        Set(isLocked, title);
    }

    public void Set(bool islocked, string title)
    {
        this.isLocked = islocked;
        tmTitle.text = title;
        if (islocked)
        {
            bg.sprite = lockedSprite;
            imgLock.gameObject.SetActive(true);
        }
        else
        {
            bg.sprite = activeSprite;
            imgLock.gameObject.SetActive(false);
        }
    }

   

}
