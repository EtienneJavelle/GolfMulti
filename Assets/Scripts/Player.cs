using System.Collections;
using UnityEngine;

public class Player : Etienne.Singleton<Player> {
    [SerializeField] private Cinemachine.CinemachineVirtualCameraBase levelCam, freelookCam;
    public bool isYourTurn = false;
    private Ball ball;
    internal bool hasPlayed = false;

    private void Start() {
        ball = GetComponentInChildren<Ball>();
        SetTurn(false);
    }

    private void Update() {
        if(isYourTurn && Input.GetKeyDown(KeyCode.Alpha1)) {
            if(levelCam.Priority == 11) {
                levelCam.Priority = 9;
            } else {
                levelCam.Priority = 11;
            }
        }
    }

    public void SetTurn(bool isYourTurn)
    {
        this.isYourTurn = isYourTurn;
        hasPlayed = !isYourTurn;
        ball.enabled = isYourTurn;
        levelCam.Priority = isYourTurn ? 9 : 11;

        if (isYourTurn)
            StartCoroutine(SendUpdate());
    }

    private IEnumerator SendUpdate()
    {
        while (ball.enabled)
        {
            if (hasPlayed)
                Connect.Send(nameof(MessageType.Update), ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);
            yield return new WaitForSeconds(0.0167f);
        }
    }

    // TODO : UI Feedback camera switch, nb joueur, nom joueur, barre chargement puissance, compteur, bouton retry tp d�but
    // TODO : gagner, sauter tour des done, 5 coup de plus que 1er abandon

    // TODO : corriger bug tir rebond
    // TODO : 3 �me map
    // TODO : Bonne couleur pour premier joueur
}
