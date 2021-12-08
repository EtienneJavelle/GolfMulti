using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    private Rigidbody rgbd;

    [SerializeField] private float minForce = 0.2f;
    [SerializeField] private float maxForce = 4f;

    [SerializeField] private float force = 0f;

    [SerializeField] private float minClubDist = 0.05f;
    [SerializeField] private float maxClubDist = 0.25f;

    [SerializeField] private float clubDist = 0f;
    [SerializeField] private float arrowDist = .2f;

    [SerializeField] private bool loading = false;

    [SerializeField] private Transform arrow;
    [SerializeField] private Transform club;

    float minVelocity = .1f;

    private void Awake() {
        rgbd = GetComponent<Rigidbody>();
    }

    private void Start() {
        ResetLoad();
    }

    private void Update() {
        if (Player.Instance.isYourTurn && !Player.Instance.hasPlayed)
        {
            if (Input.GetMouseButtonUp(1))
            {
                ResetLoad();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ResetLoad(true);
            }
            else if (Input.GetMouseButtonUp(0) && loading)
            {
                rgbd.AddForce(arrow.transform.forward * force, ForceMode.Impulse);

                ResetLoad();
                Player.Instance.hasPlayed = true;
            }
        }

        if(loading) {
            force = Mathf.Lerp(force, maxForce, Time.deltaTime);
        }

        MoveOnForward(arrow);
        arrow.position += arrow.forward * arrowDist ;

        MoveOnForward(club);

        float load = 1f / maxForce * force;
        clubDist = Mathf.MoveTowards(clubDist, minClubDist + load * (maxClubDist - minClubDist), 10f * Time.deltaTime);

        club.position -= club.forward * clubDist;

        rgbd.angularVelocity = Vector3.MoveTowards(rgbd.angularVelocity, Vector3.zero, 10f * Time.deltaTime);

        if(Player.Instance.hasPlayed && rgbd.velocity.magnitude <= minVelocity && rgbd.velocity != Vector3.zero) {
            Debug.Log("Fin du tour");
            rgbd.velocity = Vector3.zero;
            Connect.Send(nameof(MessageType.Shoot));
            Player.Instance.SetTurn(false);
        }
    }

    private void ResetLoad(bool loading = false) {
        this.loading = loading;

        arrow.gameObject.SetActive(loading);
        club.gameObject.SetActive(loading);

        force = minForce;
        clubDist = minClubDist;
    }

    private void MoveOnForward(Transform other) {
        other.position = transform.position;
        other.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
    }
}
