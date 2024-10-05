using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Europa.SaveSystem
{
    public class SaveObject : MonoBehaviour
    {
        [SerializeField] public TMP_Text saveName;
        [SerializeField] public TMP_Text saveDate;
        [SerializeField] public Button playButton;

        private void OnDisable()
        {
            Destroy(gameObject);
        }
    }
}
