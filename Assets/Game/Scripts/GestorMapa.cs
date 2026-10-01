using UnityEngine;
using UnityEngine.UI;

public class GestorMapa : MonoBehaviour
{
    [Header("Botones del Capítulo 1")]
    public Button botonSanFernando;
    public Button botonLaLuna;
    public Button botonCaney;
    public Button botonTorreCali;
    public Button botonBochalema;

    [Header("Claves de Guardado de Progreso")]
    public string claveSanFernando = "SanFernandoCompletado";
    public string claveLaLuna = "LaLunaCompletada";
    public string claveCaney = "CaneyCompletado";
    public string claveTorreCali = "TorreCaliCompletada";

    void Start()

    {
        
        ActualizarEstadoPuntos();
    }

    public void ActualizarEstadoPuntos()
    {
        // 1. San Fernando (Punto inicial siempre disponible)
        if (botonSanFernando != null)
            botonSanFernando.interactable = true;

        // 2. La Luna se desbloquea tras San Fernando
        bool sanFernandoHecho = PlayerPrefs.GetInt(claveSanFernando, 0) == 1;
        if (botonLaLuna != null)
            botonLaLuna.interactable = sanFernandoHecho;

        // 3. Caney se desbloquea tras La Luna
        bool laLunaHecha = PlayerPrefs.GetInt(claveLaLuna, 0) == 1;
        if (botonCaney != null)
            botonCaney.interactable = laLunaHecha;

        // 4. Torre de Cali se desbloquea tras Caney
        bool caneyHecho = PlayerPrefs.GetInt(claveCaney, 0) == 1;
        if (botonTorreCali != null)
            botonTorreCali.interactable = caneyHecho;

        // 5. Bochalema se desbloquea tras Torre de Cali
        bool torreHecha = PlayerPrefs.GetInt(claveTorreCali, 0) == 1;
        if (botonBochalema != null)
            botonBochalema.interactable = torreHecha;
    }

    [ContextMenu("Borrar Progreso Guardado")]
    public void BorrarProgreso()
    {
        PlayerPrefs.DeleteAll();
        ActualizarEstadoPuntos();
        Debug.Log("Progreso del mapa reiniciado.");
    }
}