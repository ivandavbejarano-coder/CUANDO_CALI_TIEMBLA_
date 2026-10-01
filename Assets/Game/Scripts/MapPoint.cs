using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPoint : MonoBehaviour
{
    public string nombreEscenaDestino;

    public void CargarEvento()
    {
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}