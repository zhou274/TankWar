using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Confirmation : MonoBehaviour
{
    public static Confirmation Instance;

    [SerializeField] private Button btnCancel, btnExit;

    public bool isOpen;

    private RectTransform rect;
    private Coroutine iAnim;

    private bool isButtonAnimRunning;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        rect = GetComponent<RectTransform>();
        btnCancel.onClick.AddListener(() => { OnCancel(); });
        btnExit.onClick.AddListener(() => { OnExit(); });


    }
    private void OnCancel()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnCancel));
        }
        Invoke("Hide", 0.4f);
        Invoke("ShowSetting", 0.5f);

    }
    private void ShowSetting()
    {
        GameSettings.Instance.Show();
    }
    private void OnExit()
    {
        if (!isButtonAnimRunning)
        {
            SoundManager.Instance.PlayClickSound();
            isButtonAnimRunning = true;
            StartCoroutine(IButtonAnim(btnExit));
        }
        Invoke("Hide",0.4f);
        Invoke("GoToHome", 0.5f);
    }
    private void GoToHome()
    {
        GameController.Instance.ExitTo(ScreenTransController.STAGE.HOME);
    }
    public void Show()
    {
        isOpen = true;

       Vector3 scaleFrom = new Vector3(0, 0, 1);
        Vector3 scaleTo = new Vector3(1, 1, 1);
        if (iAnim != null)
        {
            StopCoroutine(iAnim);
        }
        iAnim = StartCoroutine(AnimationHelper.IScaleAnim(rect,scaleFrom,scaleTo,0.3f));
    }
    public void Hide()
    {
        Vector3 scaleFrom = new Vector3(1, 1, 1);
        Vector3 scaleTo = new Vector3(0, 0, 1);
        if (iAnim != null)
        {
            StopCoroutine(iAnim);
        }
        iAnim = StartCoroutine(AnimationHelper.IScaleAnim(rect,scaleFrom, scaleTo, 0.3f));
        isOpen = false;
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
