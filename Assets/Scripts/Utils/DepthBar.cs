using TMPro;
using UnityEngine;

namespace Europa.Utils
{
    public class DepthBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentMaker;
        [SerializeField] private TMP_Text maxMaker;

        public void Update()
        {
            if (Player.Player.Singleton.transform.position.y < 0) currentMaker.text = $"{Mathf.RoundToInt(-Player.Player.Singleton.transform.position.y)}";
            else currentMaker.text = "0";
            maxMaker.text = $"{Mathf.Round(Player.Player.Singleton.MaxDepth)} m";
        }
    }
}