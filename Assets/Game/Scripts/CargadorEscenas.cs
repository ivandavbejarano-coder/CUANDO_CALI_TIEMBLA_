using UnityEngine;
using UnityEngine.SceneManagement;

public class CargadorEscenas : MonoBehaviour
{
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}