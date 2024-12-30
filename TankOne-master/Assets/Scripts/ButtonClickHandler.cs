using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{
    public GameObject AddCoinsPanel;
    public void OnPlayVsFriend()
    {

        ChoseTankPanel.Instance.Show();
    }
    public void SetAddCoinsPanel()
    {
        AddCoinsPanel.SetActive(true);
    }
}
