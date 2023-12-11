using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField, Range(0f, 10f)] private float smooth;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, smooth * Time.deltaTime);
    }
}
