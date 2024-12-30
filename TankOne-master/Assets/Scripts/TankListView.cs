using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class TankListView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform _container,rect;
    [SerializeField] private Button nextButton, prevButton, chooseButton, buyButton, upgradeButton;
    [SerializeField] private Transform player1Tag, player2Tag, player2TagFull;
    [SerializeField] private TextMeshProUGUI chooseTitle;
    [SerializeField] private RectTransform rectChooseTitle;
    [SerializeField] private GameObject[] tanks;



    public static TankListView Instance;

    private GameObject prevTank;
    private RectTransform lastSelected;
    private int player1Id, player2Id;

    [Tooltip("How fast will page lerp to target position")]
    public float decelerationRate = 10f;

    private Vector2 clampedPos = Vector2.zero;
    private int _currentIndex = 0;
    private float offset;
    [SerializeField] private float spacing = 40;
    private int contentWidth = 100;
    private bool _lerp, _horizontal=true, isDragging = false, isRightDragging, isPlayer1Selected, isActive;
    private Vector2 _lerpTo, _startPosition = new Vector2(635, 0);
    private List<Vector2> _pagePositions = new List<Vector2>();
    private int[] tankPrice = new int[] { 0, 200, 400, 500, 700, 10000 };
    private List<TankInfo> tankInfos = new List<TankInfo>();

    private Vector3 centerPos;
    private IEnumerator corButton;
    float maxDist;


    class TankInfo
    {
        public string name, power;
        public TankInfo(string name, string power)
        {
            this.name = name;
            this.power = power;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        isActive = false;
        tankInfos.Add(new TankInfo("普通坦克", "血量 1050"));
        tankInfos.Add(new TankInfo("森林坦克", "血量 1000"));
        tankInfos.Add(new TankInfo("布拉蒂诺坦克", "血量 1100"));
        tankInfos.Add(new TankInfo("联盟坦克", "血量 800"));
        tankInfos.Add(new TankInfo("夹克坦克", "血量 850"));
        tankInfos.Add(new TankInfo("幽灵坦克", "血量 800"));

        offset = contentWidth + spacing;

        for (int i = 0; i < _container.childCount; i++)
            _pagePositions.Add(new Vector2(_startPosition.x - i * offset, 0));


        prevButton.onClick.AddListener(() => { Prev(); });
        nextButton.onClick.AddListener(() => { Next(); });
        chooseButton.onClick.AddListener(() => { OnChoose(); });
        buyButton.onClick.AddListener(() => { OnBuy(); });
        upgradeButton.onClick.AddListener(() => { OnUpgrade(); });
    }
    private void Update()
    {
        if (!isActive) return;

        clampedPos.x = Mathf.Clamp(_container.anchoredPosition.x, _startPosition.x - offset * (_pagePositions.Count - 1), _startPosition.x);
        _container.anchoredPosition = clampedPos;

        //DraggingAnimation();

        // if moving to target position
        if (_lerp)
        {
            // prevent overshooting with values greater than 1
            float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
            _container.anchoredPosition = Vector2.Lerp(_container.anchoredPosition, _lerpTo, decelerate);



            // time to stop lerping?
            if (Vector2.SqrMagnitude(_container.anchoredPosition - _lerpTo) < 0.25f)
            {

                // snap to target and stop lerping
                _container.anchoredPosition = _lerpTo;
                _lerp = false;
                isDragging = false;
                UpdateCurrentIndex();
                player1Tag.gameObject.SetActive(!isPlayer1Selected);
                if (player1Id != _currentIndex)
                {
                    //player1Tag.GetComponent<RectTransform>().anchoredPosition = _container.GetChild(player1Id).GetComponent<RectTransform>().anchoredPosition;

                }
                if (isPlayer1Selected)
                    player2Tag.gameObject.SetActive(true);

                lastSelected.localScale = new Vector3(1, 1, 1);
                lastSelected = _container.GetChild(_currentIndex).GetComponent<RectTransform>();
                lastSelected.localScale = new Vector3(1.5f, 1.5f, 1);

                tanks[_currentIndex].SetActive(true);
                if (prevTank != tanks[_currentIndex])
                {
                    prevTank.SetActive(false);
                    prevTank = tanks[_currentIndex];
                }

                

                TankSelection(_currentIndex);
            }


            // switches selection icon exactly to correct page
            //if (_showPageSelection)
            //{
            //    SetPageSelection(GetNearestPage());
            //}
        }

    }

    public void Show()
    {
        rect.anchoredPosition = Constants.SHOW_VECTOR;
        //_currentIndex = 0;
        tanks[_currentIndex].SetActive(true);
        prevTank = tanks[_currentIndex];
        chooseTitle.text = "玩家 1 选择";
        player2Tag.gameObject.SetActive(false);
        

        


        lastSelected = _container.GetChild(_currentIndex).GetComponent<RectTransform>();
        lastSelected.localScale = new Vector3(1.5f, 1.5f, 1);
        centerPos = lastSelected.position;
        //maxDist = Vector3.Distance(_container.GetChild(0).GetComponent<RectTransform>().position, _container.GetChild(1).GetComponent<RectTransform>().position);
        maxDist = 150f;

        isPlayer1Selected = false;
        isActive = true;
        LerpToPage(_currentIndex);

    }

    public void Hide()
    {
        rect.anchoredPosition = Constants.HIDE_VECTOR;
        isActive = false;
        _container.GetChild(_currentIndex).GetComponent<RectTransform>().localScale = Vector3.one;
        //tanks[_currentIndex].SetActive(false);
        prevTank = null;
    }

    private void UpdateCurrentIndex()
    {
        _currentIndex = (int)((_startPosition.x - _container.anchoredPosition.x) / offset);

    }
    private void TankSelection(int index)
    {
        //button manage
        if (_currentIndex == 0 || GameData.playerState.boughtTanks.Contains(_currentIndex + 1))
        {
            chooseButton.gameObject.SetActive(true);
            upgradeButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
        }

        else
        {
            chooseButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = tankPrice[_currentIndex].ToString();
        }

        TankInfo tankInfo = tankInfos[index];
        PlayerInfoPanel.Instance.DataUpdate(tankInfo.name, tankInfo.power);
    }

    private void DraggingAnimation()
    {

        if (isDragging)
        {
            lastSelected.localScale = Vector3.one;

            RectTransform leftRect = _container.GetChild(0).GetComponent<RectTransform>();
            RectTransform rightRect = _container.GetChild(1).GetComponent<RectTransform>();



            float currentDist = Vector2.Distance(rightRect.position, centerPos);


            if (currentDist < maxDist)
            {
                float desiredScale = 1.5f - (0.5f / maxDist * currentDist);
                rightRect.localScale = Vector3.one * desiredScale;
            }
            else
            {
                rightRect.localScale = Vector3.one;
            }


            //RectTransform leftRect = null, rightRect = null, centerRect = null;
            //if (_currentPage > 0)
            //{
            //    leftRect = _container.GetChild(_currentPage - 1).GetComponent<RectTransform>();
            //}
            //centerRect = _container.GetChild(_currentPage).GetComponent<RectTransform>();
            //if (_currentPage < _pagePositions.Count - 1)
            //{
            //    rightRect = _container.GetChild(_currentPage + 1).GetComponent<RectTransform>();
            //}
            //float total = Vector2.Distance(centerRect.position, rightRect.position);


            //int opt = isRightDragging ? 1 : -1;

            //    if (leftRect)
            //    {
            //        Vector3 ls = Vector2.zero; ;
            //        float distFromCenter = Vector2.Distance(selection.position, leftRect.position);
            //        float inc = (1f / total) * distFromCenter * 0.5f;
            //        inc = 0.5f - inc;
            //        print("inc:" + inc);
            //        ls.x = inc * opt;
            //        ls.y = inc * opt;

            //        leftRect.localScale =  ls;
            //   }


        }
    }
    private void OnBuy()
    {
        if (corButton != null)
        {
            StopCoroutine(corButton);
            corButton = null;
        }


        corButton = IAnim(2);
        StartCoroutine(corButton);



    }
    private void OnUpgrade()
    {


        StartCoroutine(AnimationHelper.IButtonClick(upgradeButton, () => { ScreenTransController.Instance.ChangeStage(ScreenTransController.STAGE.UPGRADE); }));


    }
    public void OnChoose()
    {

        if (corButton != null)
        {
            StopCoroutine(corButton);
            corButton = null;
        }


        corButton = IAnim(1);
        StartCoroutine(corButton);




    }
    private IEnumerator IAnim(int btnId)
    {
        SoundManager.Instance.PlayClickSound();
        Button btn = btnId == 1 ? chooseButton : buyButton;
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
        if (btnId == 1)
        {
            if (isPlayer1Selected)
            {
                player2Id = _currentIndex + 1;
                GameData.player2Id = player2Id;


                ScreenTransController.Instance.OpenScene(ScreenTransController.STAGE.GAME);

            }
            else
            {
                player1Id = _currentIndex + 1;
                GameData.player1Id = player1Id;
                isPlayer1Selected = true;
                if (GameData.isPlayWithAI)
                {
                    ScreenTransController.Instance.OpenScene(ScreenTransController.STAGE.GAME);
                }
                else
                {
                    player1Tag.gameObject.SetActive(false);
                    player2Tag.gameObject.SetActive(true);
                    chooseTitle.text = "玩家 2 选择";
                    StartCoroutine(IChooseTitle());
                }

            }
        }
        else if (btnId == 2)
        {
            int price = tankPrice[_currentIndex];
            if (price < GameData.playerState.score)
            {
                GameData.playerState.boughtTanks.Add(_currentIndex + 1);
                GameData.playerState.score -= price;
                UIController.Instance.UpdateUI();

                chooseButton.gameObject.SetActive(true);
                //upgradeButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);

                GameData.SaveData();

            }
            else
            {
                print("Not Sufficient money");
            }
        }
    }

    private IEnumerator IChooseTitle()
    {
        Vector2 target = Vector2.one * 1.3f;


        float duration = 0.2f;

        while (duration > 0)
        {
            rectChooseTitle.localScale = Vector2.Lerp(rectChooseTitle.localScale, target, 1 / duration * Time.deltaTime);
            duration -= Time.deltaTime;
            yield return null;
        }

        duration = 0.2f;
        target = Vector2.one;
        while (duration > 0)
        {
            rectChooseTitle.localScale = Vector2.Lerp(rectChooseTitle.localScale, target, 1 / duration * Time.deltaTime);
            yield return null;
        }
    }
    public void Next()
    {

        if (_currentIndex < tanks.Length - 2)
        {
            SoundManager.Instance.PlayClickSound();
            LerpToPage(_currentIndex + 1);
        }


    }
    public void Prev()
    {
        if (_currentIndex > 0)
        {
            SoundManager.Instance.PlayClickSound();
            LerpToPage(_currentIndex - 1);
        }

    }
    private void LerpToPage(int index)
    {
        
        if (index < 0 || index > _pagePositions.Count - 1) return;
        _currentIndex = Mathf.Clamp(index, 0, _pagePositions.Count - 1);
        _lerpTo = new Vector2(_startPosition.x - offset * _currentIndex, 0);

        _lerp = true;
        isDragging = true;
        player1Tag.gameObject.SetActive(false);

        if (isPlayer1Selected)
            player2Tag.gameObject.SetActive(false);
    }

    private int GetNearestPage()
    {
        // based on distance from current position, find nearest page
        Vector2 currentPosition = _container.anchoredPosition;


        float distance = float.MaxValue;
        int nearestPage = _currentIndex;

        for (int i = 0; i < _pagePositions.Count; i++)
        {
            float testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);            
            if (testDist < distance)
            {
                distance = testDist;
                nearestPage = i;
            }
        }        
        return nearestPage;
    }

    public void Open(bool isAi)
    {
        isPlayer1Selected = false;
        chooseTitle.text = "玩家 1 选择";
        //TankSelection(0);


        StartCoroutine(IChooseTitle());
        ScreenTransController.Instance.myStage = ScreenTransController.STAGE.GAME_CHOSE;

    }

    public int PlayerID
    {
        get
        {
            return _currentIndex + 1;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        _lerp = false;
        isDragging = true;
        player1Tag.gameObject.SetActive(false);
        if (isPlayer1Selected)
            player2Tag.gameObject.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        // how much was container's content dragged
        float difference;
        if (_horizontal)
        {
            difference = _startPosition.x - _container.anchoredPosition.x;
        }
        else
        {
            difference = -(_startPosition.y - _container.anchoredPosition.y);
        }

        LerpToPage(GetNearestPage());        
        isDragging = false;

        // test for fast swipe - swipe that moves only +/-1 item
        //if (Time.unscaledTime - _timeStamp < fastSwipeThresholdTime &&
        //    Mathf.Abs(difference) > fastSwipeThresholdDistance &&
        //    Mathf.Abs(difference) < _fastSwipeThresholdMaxLimit)
        //{
        //    if (difference > 0)
        //    {
        //        Next();
        //    }
        //    else
        //    {
        //        Prev();
        //    }
        //}
        //else
        //{
        //    // if not fast time, look to which page we got to
        //    LerpToPage(GetNearestPage());
        //}

        //_dragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        isDragging = true;
        isRightDragging = eventData.delta.x > 0;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        if (eventData.position.x > player1Tag.position.x + spacing * 0.5f)
        {
            Next();
        }
        else if (eventData.position.x < player1Tag.position.x - spacing * 0.5f)
        {
            Prev();
        }
    }
}
