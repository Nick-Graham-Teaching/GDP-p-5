using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class MP_DirectionControlIcon : MonoBehaviour
    {
        private Vector2 InitialPosition;

        // Start is called before the first frame update
        void Start()
        {
            InitialPosition = GetComponent<RectTransform>().transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = InitialPosition + 
                Controller.TouchHandler.MoveDirectionControllerMaxRadius * Controller.TouchHandler.PlayerFinger.DeltaPosition;
        }
    }
}

