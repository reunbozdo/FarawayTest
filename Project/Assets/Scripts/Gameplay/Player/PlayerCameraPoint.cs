using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCameraPoint : MonoBehaviour
{
    [SerializeField] private float y = 0f;
    [SerializeField] private bool moveCameraWhenChangingLanes;

    [Inject] private RoadGenerator roadGenerator;
    [Inject] private Player player;

    private Vector3 playerPosition => player.transform.position;

    private void FixedUpdate()
    {
        Vector3 point = new Vector3(moveCameraWhenChangingLanes? playerPosition.x : roadGenerator.MiddleOfTheRoad, y, playerPosition.z);
        transform.position = point;
    }
}
