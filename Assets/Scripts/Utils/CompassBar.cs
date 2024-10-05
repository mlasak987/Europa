using UnityEngine;

namespace Europa.Utils
{
    public class CompassBar : MonoBehaviour
    {
        [SerializeField] private RectTransform compassBar;

        [SerializeField] private RectTransform northMaker;
        [SerializeField] private RectTransform southMaker;
        [SerializeField] private RectTransform westMaker;
        [SerializeField] private RectTransform eastMaker;

        [SerializeField] private RectTransform[] customMakers;

        public void Update()
        {
            SetMakerPosition(northMaker, Vector3.forward * 1000000000);
            SetMakerPosition(southMaker, Vector3.back * 1000000000);
            SetMakerPosition(westMaker, Vector3.left * 1000000000);
            SetMakerPosition(eastMaker, Vector3.right * 1000000000);
        }

        public void SetMakerPosition(RectTransform makerTransform, Vector3 worldPosition)
        {
            Vector3 directionToTarget = worldPosition - Player.Player.Singleton.transform.position;
            float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), 
                new Vector2(Player.Player.Singleton.transform.forward.x, Player.Player.Singleton.transform.forward.z));
            makerTransform.gameObject.SetActive(angle < 120f && angle > -120f);
            float compassPositionX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
            makerTransform.anchoredPosition = new Vector2(compassBar.rect.width / 2 * compassPositionX, 0);
        }
    }
}