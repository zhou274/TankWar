using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class AnimationHelper
{


    public static IEnumerator IScaleAnim(RectTransform rect, Vector3 scaleFrom, Vector3 scaleTo, float duration, UnityAction action = null)
    {

        rect.anchoredPosition = Vector3.zero;
        rect.localScale = scaleFrom;

        Vector3 scale = rect.localScale;
        int opr = scaleFrom.x < scaleTo.x ? 1 : -1;
        while (Vector3.Distance(rect.localScale, scaleTo) > 0.1f)
        {
            scale.x = scale.x + opr * (Time.deltaTime / duration);
            scale.y = scale.y + opr * (Time.deltaTime / duration);
            rect.localScale = scale;
            yield return 0;
        }

        if (scaleTo == new Vector3(0, 0, 1))
        {
            rect.position = new Vector3(2000, 0, 0);
        }
        
        if (action != null)
            action.Invoke();

    }

   


    public static IEnumerator IButtonClick(Button btn, UnityAction action)
    {
        SoundManager.Instance.PlayClickSound();

        RectTransform rect = btn.GetComponent<RectTransform>();
        Vector3 target = new Vector3(0.8f, 0.8f, 1);
        float t = 0, d = 0.2f;
        while (Vector3.Distance(rect.localScale, target) > 0)
        {
            t += Time.deltaTime;
            rect.localScale = Vector3.Lerp(rect.localScale, target, t / d);
            yield return null;
        }
        target = new Vector3(1f, 1f, 1);
        t = 0;
        while (Vector3.Distance(rect.localScale, target) > 0)
        {
            t += Time.deltaTime;
            rect.localScale = Vector3.Lerp(rect.localScale, target, t / d);
            yield return null;
        }

        action.Invoke();

    }


}