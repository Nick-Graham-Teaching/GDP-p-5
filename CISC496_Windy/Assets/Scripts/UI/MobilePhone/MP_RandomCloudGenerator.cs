using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class MP_RandomCloudGenerator : Singleton<MP_RandomCloudGenerator>
    {

        public GameObject Cloud1;
        public GameObject Cloud2;
        public GameObject Cloud3;
        public GameObject Cloud4;
        public GameObject Cloud5;

        public int InitCloudAnim;

        private GameObject[] Clouds;

        // Start is called before the first frame update
        void Start()
        {
            Clouds = new[] { Cloud1, Cloud2, Cloud3, Cloud4, Cloud5 };

            InitCloudAnim = Mathf.FloorToInt(Random.Range(0.0f, 4.9f));

            Clouds[InitCloudAnim].GetComponent<UnityEngine.Animation>().Play();
        }

        public void OnCloudGeneration(int currentIndex)
        {
            int randomIndex;;
            do {
                randomIndex = Mathf.FloorToInt(Random.Range(0.0f, 4.9f));
            } while (randomIndex == currentIndex) ;

            Clouds[randomIndex].GetComponent<UnityEngine.Animation>().Play();
        }
    }

}
