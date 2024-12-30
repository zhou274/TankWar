using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private Button btnSound, btnMusic, btnVibration, btnClose;
    [SerializeField]
    private Sprite onSprite, offSprite;
    [SerializeField]
    private GameObject popup;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AnimationClip open, close;
    private bool isButtonAnimRunning;

    private void Start()
    {
        btnMusic.onClick.AddListener(() => { OnMusic(); });
        btnSound.onClick.AddListener(() => { OnSound(); });
        btnVibration.onClick.AddListener(() => { OnVibrate(); });
        btnClose.onClick.AddListener(() => { OnClose(); });
    }


    public void Open()
    {
        popup.SetActive(true);
        anim.SetTrigger("Open");

        PrepareButton(btnMusic, GameData.playerState.isMusic, GameData.playerState.isMusic ? "Music: ON" : "Music: OFF");
        PrepareButton(btnSound, GameData.playerState.isSound, GameData.playerState.isMusic ? "Sound: ON" : "Sound: OFF");
        PrepareButton(btnVibration, GameData.playerState.isVibrate, GameData.playerState.isMusic ? "Vibrate: ON" : "Vibrate: OFF");
    }
    public void Hide()
    {
        anim.SetTrigger("Close");
        Invoke("HidePopup", 0.5f);
    }
    private void HidePopup()
    {
        popup.SetActive(false);
    }

    private void PrepareButton(Button button, bool isOn, string title)
    {
        button.GetComponent<Image>().sprite = isOn ? onSprite : offSprite;
        button.GetComponentInChildren<TextMeshProUGUI>().text = title;
        button.GetComponentInChildren<TextMeshProUGUI>().color = isOn ? new Color(0.4245283f, 0.09011215f, 0.09011215f, 1f) : Color.white;

    }

    private void OnSound()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnSound));
        }
        GameData.Sound(!GameData.playerState.isSound);
        PrepareButton(btnSound, GameData.playerState.isSound, GameData.playerState.isSound ? "Sound: ON" : "Sound: OFF");
    }
    private void OnMusic()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnMusic));
        }
        GameData.Music(!GameData.playerState.isMusic);
        PrepareButton(btnMusic, GameData.playerState.isMusic, GameData.playerState.isMusic ? "Music: ON" : "Music: OFF");

        SoundManager.Instance.PlayHomeBackground();

    }
    private void OnVibrate()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnVibration));
        }
        GameData.Vibration(!GameData.playerState.isVibrate);
        PrepareButton(btnVibration, GameData.playerState.isVibrate, GameData.playerState.isVibrate ? "Vibrate: ON" : "Vibrate: OFF");
    }

    private void OnClose()
    {
        SoundManager.Instance.PlayClickSound();
        Hide();
    }

    private IEnumerator IButtonAnim(Button button)
    {

        RectTransform rect = button.GetComponent<RectTransform>();
        Vector2 orginalScale = rect.localScale;
        Vector2 toScale = orginalScale;

        float duration = 0.2f;

        while (rect.localScale.x >= 0.8f)
        {
            toScale.x -= 0.8f * Time.deltaTime;
            toScale.y -= 0.8f * Time.deltaTime;
            rect.localScale = toScale;
            duration -= Time.deltaTime;
            yield return null;

        }

        duration = 0.2f;
        while (rect.localScale.x <= 1)
        {
            toScale.x += 0.8f * Time.deltaTime;
            toScale.y += 0.8f * Time.deltaTime;
            rect.localScale = toScale;
            duration -= Time.deltaTime;
            yield return null;

        }
        isButtonAnimRunning = false;



    }
}
