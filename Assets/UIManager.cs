using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Etienne.Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI mode;
    [SerializeField] private TextMeshProUGUI nbPlayer;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private Slider power;
    [SerializeField] GameObject gameState;
    [SerializeField] GameObject win;

    public void Start()
    {
        win.SetActive(false);        
    }

    public void SetCameraMode(bool free)
    {
        mode.text = free ? "Free" : "Play";
    }

    public void SetUsername(string username)
    {
        this.username.text = username;
    }

    public void SetNbPlayer(int nb)
    {
        nbPlayer.text = nb.ToString();
    }

    public void SetPower(float power)
    {
        this.power.value = power;
    }

    public void YourTurn(bool yourTurn)
    {
        gameState.SetActive(yourTurn);
    }

    public void Win(bool win)
    {
        this.win.SetActive(win);
    }
}
