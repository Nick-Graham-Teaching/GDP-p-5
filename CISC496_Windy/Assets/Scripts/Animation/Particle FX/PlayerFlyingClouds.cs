using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Animation
{
    public class PlayerFlyingClouds : MonoBehaviour
    {
        [SerializeField] ParticleSystem Clouds;

        ParticleSystem.EmissionModule ParticleEmission;

        [SerializeField] float ParticleEmissionTime;


        private void Start()
        {
            ParticleEmission = Clouds.emission;
        }

        IEnumerator TurnOffCloudParticle()
        {
            ParticleEmission.enabled = true;

            float startTime = Time.time;

            yield return new WaitUntil(() => !Game.GameProgressManager.Instance.GameState.IsInGame() || Time.time - startTime > ParticleEmissionTime);

            ParticleEmission.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Cloud"))
            {
                StopAllCoroutines();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Cloud"))
            {
                StartCoroutine(TurnOffCloudParticle());
            }
        }
    }
}

