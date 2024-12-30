using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public FixedJoystick moveJoystick, rotateJoystic;
    public GameObject goFire, goWeapn, goFuel;
    public Player player1, player2;
    public bool isJoysticActive,isGameover;
    public Healthbar health1, health2;
    public Image fuel;
    public List<RowWeapon> allWeapons;


    [SerializeField] GameObject[] tankPrefs;

    private bool mPlayer1Turn;

    [SerializeField] private Transform player1Spwan, player2Spwan;


    private bool isPlayWithAI;
    public Player activePlayer;

    public static GameController Instance;

    public static GameController GetInstance()
    {

        return Instance;
    }

    private void Awake()
    {
        Instance = this;
    }



    private void Update()
    {
        if (moveJoystick.Direction.magnitude > 0 || rotateJoystic.Direction.magnitude > 0)
        {
            isJoysticActive = true;
        }
        else
        {
            isJoysticActive = false;
        }



        if (PanZoom.Instance.isZoomActive)
        {
            UpdatePlayerCanvas();
        }

        
    }

    private void ShowSettings()
    {
        GameSettings.Instance.Show();
    }

    private void InitPlayers()
    {
        GameObject go1 = Instantiate(tankPrefs[GameData.player1Id - 1].gameObject, transform);
        go1.transform.position = player1Spwan.position;
        player1 = go1.GetComponent<Player>();
        player1.playerType = 1;
        

        int player2Id = GameData.isPlayWithAI ? Random.Range(1, tankPrefs.Length) : GameData.player2Id;
        GameObject go2 = Instantiate(tankPrefs[player2Id - 1].gameObject, transform);
        go2.transform.position = player2Spwan.position;
        player2 = go2.GetComponent<Player>();
        player2.playerType = 2;
        player2.isAI = isPlayWithAI;


        

    }

    public void Open()
    {
        isPlayWithAI = GameData.isPlayWithAI;


        InitPlayers();

        mPlayer1Turn = true;
        isGameover = false;

        player1.SetActive(false);
        player2.SetActive(false);

        activePlayer = player1;

        PanZoom.Instance.StartCinemation();
    }

    public void UpdatePlayerCanvas()
    {
        if (activePlayer == null) return;
        float scale = 0.01f + (0.01f / PanZoom.Instance.zoomOutMax * Camera.main.orthographicSize);
        activePlayer.canvas.localScale = Vector3.one * scale;
    }

    public void InputController(bool isShow)
    {
     
        if (!isPlayWithAI)
            activePlayer.canvas.gameObject.SetActive(isShow);
        else
            player1.canvas.gameObject.SetActive(isShow);

        moveJoystick.gameObject.SetActive(isShow);
        rotateJoystic.gameObject.SetActive(isShow);
        goFire.gameObject.SetActive(isShow);
        goFuel.gameObject.SetActive(isShow);
        goWeapn.gameObject.SetActive(isShow);
    }
    public void ChangeTurn(float delay)
    {
        Invoke("ChangeTurn", delay);
    }
    public void ChangeTurn()
    {
        mPlayer1Turn = !mPlayer1Turn;


        player1.SetActive(mPlayer1Turn);
        player2.SetActive(!mPlayer1Turn);

        activePlayer = mPlayer1Turn ? player1 : player2;

        UpdatePlayerCanvas();

        if (!mPlayer1Turn && isPlayWithAI)
        {
            InputController(false);
            activePlayer.StartAI();
        }
        else
        {
            InputController(true);

        }
        
    }


    public void Fire()
    {
        activePlayer.Fire();
    }

    public void GameOver(int lostPlayerType)
    {
        GameData.winnerName = lostPlayerType == 1 ? "Player 2" : "Player 1";
        GameData.winnerId = lostPlayerType == 1 ? player2.id : player1.id;
        GameData.playerState.score += 200;
        GameData.rewarded = 200;
        GameData.SaveData();
        AdsControl.Instance.showAds(null);
    }

    public void ExitTo(ScreenTransController.STAGE stage)
    {
        ScreenTransController.Instance.OpenScene(stage);
        Destroy(player1.gameObject, 0.5f);
        Destroy(player2.gameObject, 0.5f);
        activePlayer = null;

    }

    public void ChangeWeapon(RowWeapon weapon)
    {
        activePlayer.selectedWeapon = weapon;
    }

    public void PrepareWeaponList(List<int> usedId)
    {
        for (int i = 0; i < allWeapons.Count; i++)
        {
            if (usedId.Contains(allWeapons[i].id))
            {
                allWeapons[i].gameObject.SetActive(false);
            }
            else
            {
                allWeapons[i].gameObject.SetActive(true);
            }
        }
    }
}
