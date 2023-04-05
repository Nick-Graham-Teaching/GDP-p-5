using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Game
{
    public enum Level
    {
        TutLevel, FirstLevel
    }

    public class GameLevelManager : StaticSingleton<GameLevelManager>
    {
        public static Level CurrentLevel { get; private set; }

        [SerializeField] Transform Level2ModelsTransform;
        [SerializeField] float Level2ModelsTranslationTime;
        [SerializeField] Vector3 Level2ModelsPositionInTutLevel;
        Vector3 Level2ModelsPositionInLevelOne;


        [SerializeField] ParticleSystem Level2ModelsCloudBridge;
        [SerializeField] ParticleSystem Level2ModelsMainlandClouds;

        ParticleSystem.EmissionModule Level2ModelsCloudBridgeEmissionModule;
        ParticleSystem.EmissionModule Level2ModelsMainlandCloudsEmissionModule;

        private new void Awake()
        {
            base.Awake();
            Level2ModelsPositionInLevelOne = Level2ModelsTransform.position;

            Level2ModelsCloudBridgeEmissionModule = Level2ModelsCloudBridge.emission;
            Level2ModelsMainlandCloudsEmissionModule = Level2ModelsMainlandClouds.emission;
        }

        public static void TutLevelSetup()
        {
            CurrentLevel = Level.TutLevel;
            Instance.Level2ModelsTransform.position = Instance.Level2ModelsPositionInTutLevel;
            Instance.Level2ModelsTransform.gameObject.SetActive(false);

            Instance.Level2ModelsCloudBridgeEmissionModule.enabled    = false;
            Instance.Level2ModelsMainlandCloudsEmissionModule.enabled = false;
        }

        public static void LevelOneSetUp()
        {
            CurrentLevel = Level.FirstLevel;
            Instance.Level2ModelsTransform.gameObject.SetActive(true);
            Instance.StartCoroutine(Instance.TutLevelToFirstLevelTransition());
        }

        IEnumerator TutLevelToFirstLevelTransition()
        {
            yield return new WaitUntil(()=> {

                Level2ModelsTransform.position += (Level2ModelsPositionInLevelOne - Level2ModelsPositionInTutLevel) / Level2ModelsTranslationTime * Time.deltaTime;

                return Level2ModelsTransform.position.y > Level2ModelsPositionInLevelOne.y;
            });

            Level2ModelsCloudBridgeEmissionModule.enabled = true;
            Level2ModelsMainlandCloudsEmissionModule.enabled = true;
        }
    }
}

