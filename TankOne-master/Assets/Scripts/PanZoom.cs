using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    public Transform bottomBoundary;
    public Transform topBoundary;
    public Transform border;


    [SerializeField]
    private Transform player1, player2;


    private float zoomOutMin = 3;
    public float zoomOutMax = 12;



    public bool isPanActive, isZoomActive;
    [SerializeField]
    private Vector2 boundary;

    public bool isCinemationRunning = true;
    public static PanZoom Instance;
    Vector3 touchStart;
    private Camera cam;

    private void Awake()
    {
        Instance = this;
    }
    

    private void Update()
    {
        if (!GameController.GetInstance().isJoysticActive && !isCinemationRunning)
        {
            MobileInput();
            //PCInput();
            
        }




    }

    private void MobileInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float diff = currentMagnitude - prevMagnitude;

            zoom(diff * 0.01f);




        }
        else if (Input.touchCount == 1 && !isZoomActive)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStart = Camera.main.ScreenToWorldPoint(touch.position);
                    break;
                case TouchPhase.Moved:
                    Vector3 dir = touchStart - Camera.main.ScreenToWorldPoint(touch.position);

                    cam.transform.position += dir;

                    PanBounded();

                    isPanActive = true;
                    break;
                case TouchPhase.Ended:

                    isPanActive = false;
                    break;
                case TouchPhase.Canceled:
                    isPanActive = false;
                    break;

            }



        }

        else if (Input.touchCount == 0)
        {
            isZoomActive = false;
            isPanActive = false;
        }
    }
    private void PCInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 dir = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position += dir;

            PanBounded();

            isPanActive = true;
        }

        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void PanBounded()
    {



        float orthographicSize = cam.orthographicSize;
        Vector3 curPos = cam.transform.position;

        //Vector3 pos = new Vector3(Mathf.Clamp(curPos.x, bottomBoundary.position.x + orthographicSize * 2, topBoundary.position.x - orthographicSize * 2), Mathf.Clamp(curPos.y, bottomBoundary.position.y + orthographicSize, topBoundary.position.y - orthographicSize), -10);
        Vector3 pos = new Vector3(Mathf.Clamp(curPos.x, -boundary.x + orthographicSize * 2, boundary.x - orthographicSize * 2), Mathf.Clamp(curPos.y, -boundary.y + orthographicSize, boundary.y - orthographicSize), -10);


        cam.transform.position = pos;

    }

    private void zoom(float increment)
    {
        //zoom in (positive increment)  
        isZoomActive = true;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, zoomOutMin, zoomOutMax);
        PanBounded();


    }

    private IEnumerator ICinemate()
    {

        isCinemationRunning = true;
        cam.orthographicSize = zoomOutMin;
        GameController.Instance.InputController(false);

        //2s delay for screen transaction
        yield return new WaitForSeconds(0.5f);


        Vector3 camPos = cam.transform.position;
        Vector3 target = player2.position;
        target.z = cam.transform.position.z;
        float t = 0;
        float duration = 8.0f;

        while (Vector2.Distance(cam.transform.position, target) > 0.1f)
        {
            t += (1 / duration) * Time.deltaTime;
            cam.transform.position = Vector3.Lerp(cam.transform.position, target, t);

            yield return 0;
        }

        yield return new WaitForSeconds(0.5f);
        target = player1.position;
        target.z = cam.transform.position.z;
        t = 0;
        duration = 15f;
        while (Vector2.Distance(cam.transform.position, target) > 0.1f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);
            cam.transform.position = Vector3.Lerp(cam.transform.position, target, t);

            yield return 0;
        }

        yield return new WaitForSeconds(0.5f);

        while (cam.orthographicSize < zoomOutMax)
        {
            zoom(-0.1f);

            yield return 0;
        }
        isCinemationRunning = false;
        GameController.Instance.InputController(true);

        GameController.Instance.activePlayer.SetActive(true);
        float scale = 0.01f + (0.01f / zoomOutMax * Camera.main.orthographicSize);
        GameController.Instance.activePlayer.canvas.localScale = Vector3.one * scale;
    }

    public void StartCinemation()
    {
        cam = Camera.main;
        float ratio = Screen.height / (float)Screen.width;
        zoomOutMax = Mathf.Abs(border.position.x) * ratio;
        zoomOutMax -= 0.1f;
        boundary = new Vector2(zoomOutMax * 2.0f, zoomOutMax);

        StartCoroutine(ICinemate());
    }
}
