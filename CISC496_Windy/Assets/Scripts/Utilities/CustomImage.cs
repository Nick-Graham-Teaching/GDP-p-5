using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public class CustomImage : UnityEngine.UI.Image
    {
        private PolygonCollider2D _polygonCollider2D;
        private PolygonCollider2D PolygonCollider2D
        {
            get
            {
                if (_polygonCollider2D is null)
                {
                    _polygonCollider2D = GetComponent<PolygonCollider2D>();
                }
                return _polygonCollider2D;
            }
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Vector3 point;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);
            return PolygonCollider2D.OverlapPoint(point);
        }
    }
}
