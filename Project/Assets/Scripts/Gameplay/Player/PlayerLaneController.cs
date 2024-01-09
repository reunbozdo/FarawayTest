using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class PlayerLaneController : MonoBehaviour
{
    [Inject] private RoadGenerationSettings roadGenerationSettings;
    [Inject] private RoadGenerator roadGenerator;
    [Inject] private CharacterLaneSwitchSettings characterLaneSwitchSettings;

    [ShowInInspector]
    public int CurrentLaneIndex
    {
        get => currentLaneIndex;
        set
        {
            if (value != currentLaneIndex && value >= 0 && value < roadGenerationSettings.laneCount)
            {
                currentLaneIndex = value;
                MoveToCurrentLane();
            }
        }
    }
    private int currentLaneIndex;

    private void OnDestroy()
    {
        Disable();
    }

    public void Activate()
    {
        SwipeHandler.OnSwipe += SwipeHandler_OnSwipe;
    }

    public void Disable()
    {
        SwipeHandler.OnSwipe -= SwipeHandler_OnSwipe;
    }

    public void PutPlayerAtStart()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        int laneIndex = characterLaneSwitchSettings.playerStartLane;
        currentLaneIndex = laneIndex;
        MoveToCurrentLane(true);
    }

    private void SwipeHandler_OnSwipe(SwipeHandler.Direction direction)
    {
        TryToSwitchLane(direction);
    }

    private void TryToSwitchLane(SwipeHandler.Direction direction)
    {
        switch (direction)
        {
            case SwipeHandler.Direction.left:
                CurrentLaneIndex--;
                break;
            case SwipeHandler.Direction.right:
                CurrentLaneIndex++;
                break;
        }
    }

    private void MoveToCurrentLane(bool immediately = false)
    {
        float endXPos = roadGenerator.LaneXCoord(currentLaneIndex);
        float startXPos = transform.position.x;
        float duration = immediately ? 0f : characterLaneSwitchSettings.duration;
        DOTween.To(() => startXPos, x => transform.position = new Vector3(x, transform.position.y, transform.position.z), endXPos, duration);
    }
}