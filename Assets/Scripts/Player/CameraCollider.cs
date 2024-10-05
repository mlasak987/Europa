using UnityEngine;

namespace Europa.Player
{
    public class CameraCollider : MonoBehaviour
    {
        [SerializeField] private Material outlineMaterial;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Colectable"))
            {
                Material[] materials = new Material[other.GetComponent<Renderer>().materials.Length + 1];
                for (int i = 0; i < other.GetComponent<Renderer>().materials.Length; i++)
                    materials[i] = other.GetComponent<Renderer>().materials[i];
                materials[other.GetComponent<Renderer>().materials.Length] = outlineMaterial;
                other.GetComponent<Renderer>().materials = materials;

                Player.Singleton.EnablePickUp(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Colectable"))
            {
                Material[] materials = new Material[other.GetComponent<Renderer>().materials.Length - 1];
                for (int i = 0; i < other.GetComponent<Renderer>().materials.Length - 1; i++)
                    materials[i] = other.GetComponent<Renderer>().materials[i];
                other.GetComponent<Renderer>().materials = materials;

                Player.Singleton.DisablePickUp();
            }
        }
    }
}
