using UnityEngine;

public class CursorBlock : MonoBehaviour {
    [SerializeField] private CursorLockMode cursorLockMode = CursorLockMode.Confined;
    [SerializeField] private bool visible = false;

    private void OnEnable () {
        Cursor.lockState = cursorLockMode;
        Cursor.visible = visible;
    }

}
