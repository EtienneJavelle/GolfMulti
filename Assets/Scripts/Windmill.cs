using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Windmill : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    void Start() {
        transform.DORotate(Vector3.forward * 180, speed, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
}
