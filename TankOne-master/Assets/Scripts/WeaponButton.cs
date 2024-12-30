using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour, IPointerClickHandler
{
    public Image imgButton;
    public RectTransform weaponListView;
    public GameObject[] weapons;
    public float duration = 2;
    public int selectedWeaponId = 1;

    private float t;
    private List<int> usedWeaponP1, usedWeaponP2;
   [SerializeField] private bool isOpened = false, isRunning;
    private IEnumerator iAnim;
    public static WeaponButton Instance;


    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        ResetWeaponList();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isRunning) return;
        isOpened = !isOpened;

        if (iAnim != null)
            StopCoroutine(iAnim);

        iAnim = Open();
        StartCoroutine(iAnim);
    }

    public void HideList()
    {
        if (isRunning || !isOpened) return;
        isOpened = false;
        iAnim = Open();
        StartCoroutine(iAnim);
    }
    

    public void ResetWeaponList()
    {
        if (usedWeaponP1 == null)
            usedWeaponP1 = new List<int>();
        if (usedWeaponP2 == null)
            usedWeaponP2 = new List<int>();

        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(true);
        usedWeaponP1.Clear();
        usedWeaponP2.Clear();
        isOpened = false;
        isRunning = false;
    }

   
    public void UpdateView(RowWeapon weapon)
    {
        imgButton.sprite = weapon.sprIcon;        
    }
    public void OnItemSelected(int id, Sprite sprite)
    {
        selectedWeaponId = id;
        imgButton.sprite = sprite;
        isOpened = false;
        if (iAnim != null)
            StopCoroutine(iAnim);

        iAnim = Open();
        StartCoroutine(iAnim);        
    }



    private IEnumerator Open()
    {
        isRunning = true;
        UIClickHandler.Instance.Handle(true);
        Vector2 target = new Vector2(isOpened ? -420f : 0f, 0);

        t = 0;
        while (Vector2.Distance(weaponListView.anchoredPosition, target) > 0.1f)
        {
            t += Time.deltaTime;
            weaponListView.anchoredPosition = Vector2.Lerp(weaponListView.anchoredPosition, target, t / duration);
            yield return null;
        }
        isRunning = false;
        
        yield return null;

    }
}

