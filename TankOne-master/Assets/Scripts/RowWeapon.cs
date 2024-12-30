using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RowWeapon : MonoBehaviour, IPointerClickHandler
{

    public int id;
    public int power=10;
    public int demageAmount = 10;
    [SerializeField] private Image imgIcon;
    [SerializeField] private TextMeshProUGUI texTitle;
    [SerializeField] private string title;
    public Sprite sprIcon;

    public void OnPointerClick(PointerEventData eventData)
    {
        WeaponButton.Instance.OnItemSelected(id,sprIcon);
        GameController.Instance.activePlayer.SetWeapon(this);
    }

    private void Start()
    {
        imgIcon.sprite = sprIcon;
        texTitle.text = title;
    }

}
