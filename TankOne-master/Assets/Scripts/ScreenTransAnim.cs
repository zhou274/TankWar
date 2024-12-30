using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTransAnim : MonoBehaviour
{

    [SerializeField]private Image zoomImage;

    public static ScreenTransAnim Instance;

    private Vector3 from, to;
    private string nextScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //zoomImage = GetComponent<Image>();        
    }

    IEnumerator IAnim()
    {
        from = Vector3.zero;
        to = new Vector3(30, 30, 1);

        RectTransform rect = zoomImage.GetComponent<RectTransform>();
        rect.localScale = from;
        float duration = 2f, t = 0;
        while (Vector3.Distance(rect.localScale, to) > 0.2f)
        {
            t += Time.deltaTime;
            rect.localScale = Vector3.Lerp(rect.localScale, to, t / duration);
            yield return 0;
        }
        from = new Vector3(30, 30, 1);
        to = Vector3.zero;

        duration = 2f;
        t = 0;
        while (Vector3.Distance(rect.localScale, to) > 0.2f)
        {
            t += Time.deltaTime;
            rect.localScale = Vector3.Lerp(rect.localScale, to, t / duration);
            yield return 0;
        }

        if (nextScene != null)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
            rect.localScale = Vector3.zero;
    }

    public void EndAnim(string nextScene)
    {
        gameObject.SetActive(true);
        this.nextScene = nextScene;
        from = Vector3.zero;
        to = new Vector3(30, 30, 1);
        StartCoroutine(IAnim());
    }
    public void StartAnim()
    {
        zoomImage.gameObject.SetActive(true);
        
        StartCoroutine(IAnim());
    }
}
