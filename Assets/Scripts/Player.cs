using UnityEngine;

public class Player : Etienne.Singleton<Player> {
    [SerializeField] private Cinemachine.CinemachineVirtualCameraBase levelCam, freelookCam;
    private bool isYourTurn = true;

    private void Update() {
        if(isYourTurn && Input.GetKeyDown(KeyCode.Alpha1)) {
            if(levelCam.Priority == 11) {
                levelCam.Priority = 9;
            } else {
                levelCam.Priority = 11;
            }
        }
    }

    public void SetTurn(bool isYourTurn) {
        this.isYourTurn = isYourTurn;
        if(!isYourTurn) {
            levelCam.Priority = 11;
        }
    }
}
