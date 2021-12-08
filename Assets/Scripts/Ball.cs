using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rgbd;

    [SerializeField] private float minForce = 2f;
    [SerializeField] private float maxForce = 10f;

    [SerializeField] private float force = 0f;

    [SerializeField] private bool loading = false;

    [SerializeField] private GameObject arrow;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            loading = false;

            force = minForce;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            loading = true;

            force = minForce;
        }
        else if (Input.GetMouseButtonUp(0) && loading)
        {
            loading = false;

            rgbd.AddForce(arrow.transform.forward * force, ForceMode.Impulse);

            force = minForce;
        }
        
        if (loading)
            force = Mathf.Lerp(force, maxForce, Time.deltaTime);

        arrow.transform.position = transform.position;
        arrow.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);

        arrow.transform.position += arrow.transform.forward * 0.5f;
    }
}
