using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [Header("UI")]
    public WindowControllerSettings windowControllerSettings;
    [InlineEditor] public AnimationSettings defaultUIElementAnimationSettings;
    [InlineEditor] public ButtonBehaviourSettings defaultButtonBehaviourSettings;
    [InlineEditor] public TextBehaviourSettings defaultTextBehaviourSettings;

    [Space]

    [Header("Input")]
    public SwipeSettings swipeSettings;

    [Space]

    [Header("Player character")]
    public CharacterSpeedSettings characterSpeedSettings;
    public CharacterLaneSwitchSettings characterLaneSwitchSettings;
    public CharacterJumpSettings characterJumpSettings;
    public CharacterCrawlSettings characterCrawlSettings;
    public CharacterFlySettings characterFlySettings;

    [Space]

    [Header("Road generation")]
    public RoadGenerationSettings roadGenerationSettings;
    public ObstacleGenerationSettings obstacleGenerationSettings;
    public BoosterGenerationSettings boosterGenerationSettings;

    public override void InstallBindings()
    {
        Container.Bind<WindowControllerSettings>().FromInstance(windowControllerSettings).AsSingle();
        Container.Bind<AnimationSettings>().FromInstance(defaultUIElementAnimationSettings).AsSingle();
        Container.Bind<ButtonBehaviourSettings>().FromInstance(defaultButtonBehaviourSettings).AsSingle();
        Container.Bind<TextBehaviourSettings>().FromInstance(defaultTextBehaviourSettings).AsSingle();

        Container.Bind<SwipeSettings>().FromInstance(swipeSettings).AsSingle();

        Container.Bind<CharacterSpeedSettings>().FromInstance(characterSpeedSettings).AsSingle();
        Container.Bind<CharacterLaneSwitchSettings>().FromInstance(characterLaneSwitchSettings).AsSingle();
        Container.Bind<CharacterJumpSettings>().FromInstance(characterJumpSettings).AsSingle();
        Container.Bind<CharacterCrawlSettings>().FromInstance(characterCrawlSettings).AsSingle();
        Container.Bind<CharacterFlySettings>().FromInstance(characterFlySettings).AsSingle();

        Container.Bind<RoadGenerationSettings>().FromInstance(roadGenerationSettings).AsSingle();
        Container.Bind<ObstacleGenerationSettings>().FromInstance(obstacleGenerationSettings).AsSingle();
        Container.Bind<BoosterGenerationSettings>().FromInstance(boosterGenerationSettings).AsSingle();
    }
}