using UnityEngine;
using TMPro;


public class UIController : MonoBehaviour
{
    [SerializeField]
    private Settings settings;
    [SerializeField] Transform gameTrans, chooseTanksTrans;
    [SerializeField] private ChoseTankPanel choseTankPanel;
    

    [SerializeField]private TextMeshProUGUI tmpScore,tmpGem;
    public static UIController Instance;

    #region built-in functions
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        UpdateUI();
        GameData.isFirstOpen = false;
    }

    #endregion

    #region private functions


    #endregion

    #region public functions

    public void UpdateUI()
    {
        tmpScore.text = GameData.playerState.score.ToString();
        tmpGem.text = GameData.playerState.gem.ToString();        
    }
    public void ShowSettings()
    {
        settings.Open();
    }
   
    

    #endregion

}
