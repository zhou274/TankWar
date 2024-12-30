using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private Camera cam;
    private IEnumerator anim;
    float duration = 0.2f;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }
    public void Shake(float duration=0.2f)
    {
        this.duration = duration;
        if(anim!=null)
        {
            StopCoroutine(anim);
            anim = null;
        }
        anim = IShake();
        StartCoroutine(anim);
    }
    private IEnumerator IShake()
    {
        Vector3 orginalPos = cam.transform.position;
        
        float magnitude = 0.4f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.transform.localPosition = new Vector3(x, y, orginalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.transform.localPosition = orginalPos;
    }
}
