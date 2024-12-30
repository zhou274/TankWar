using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public Sprite onSprite, offSprite;

    private RectTransform rect;
    private Coroutine iAnim;
    
    
    private bool isOpen,isButtonAnimRunning;

    [SerializeField] private Button btnResume, btnSound, btnMusic, btnVibration, btnMainMenu;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        rect = GetComponent<RectTransform>();

        btnResume.onClick.AddListener(() => { OnResume(); });
        btnSound.onClick.AddListener(() => { OnSound(); });
        btnMusic.onClick.AddListener(() => { OnMusic(); });
        btnVibration.onClick.AddListener(() => { OnVibration(); });
        btnMainMenu.onClick.AddListener(() => { OnMainMenu(); });



        PrepareButton(btnMusic, GameData.playerState.isMusic, GameData.playerState.isMusic ? "Music: ON" :"Music: OFF");
        PrepareButton(btnSound, GameData.playerState.isSound, GameData.playerState.isSound ? "Sound: ON" : "Sound: OFF");
        PrepareButton(btnVibration, GameData.playerState.isVibrate, GameData.playerState.isVibrate ? "Vibration: ON" : "Vibration: OFF");
    }
    private void PrepareButton(Button button,bool isOn,string title)
    {
        button.GetComponent<Image>().sprite =isOn? onSprite:offSprite;
        button.GetComponentInChildren<TextMeshProUGUI>().text = title;
        button.GetComponentInChildren<TextMeshProUGUI>().color = isOn?new Color(0.4245283f, 0.09011215f, 0.09011215f, 1f):Color.white;

    }

    private void OnResume()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnResume));
        }
        Invoke("Hide", 0.4f);
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

        SoundManager.Instance.PlayGameBackground();
    }
    private void OnVibration()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnVibration));
        }

        GameData.Vibration(!GameData.playerState.isVibrate);
        PrepareButton(btnVibration, GameData.playerState.isVibrate, GameData.playerState.isVibrate ? "Vibration: ON" : "Vibration: OFF");

    }
    private void OnMainMenu()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnMainMenu));
        }
        Invoke("Hide", 0.4f);        
        Invoke("ShowConfirmation", 0.6f);
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
    public void ShowOrHide()
    {
        if (isOpen)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void Show()
    {

        Vector3 scaleFrom = new Vector3(0, 0, 1);
        Vector3 scaleTo = new Vector3(1, 1, 1);
        if (iAnim != null)
        {
            StopCoroutine(iAnim);
        }
        iAnim = StartCoroutine(AnimationHelper.IScaleAnim(rect, scaleFrom, scaleTo, 0.3f));
        isOpen = true;

    }
    public void Hide()
    {
        Vector3 scaleFrom = new Vector3(1,1, 1);
        Vector3 scaleTo = new Vector3(0, 0, 1);
        if (iAnim != null)
        {
            StopCoroutine(iAnim);
            iAnim = null;
        }
        iAnim = StartCoroutine(AnimationHelper.IScaleAnim(rect, scaleFrom, scaleTo, 0.3f));
        isOpen = false;
    }
    private void ShowConfirmation()
    {
        Confirmation.Instance.Show();
    }
}
