using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Va en cada punto del mapa (un Button de UI).
[RequireComponent(typeof(Button))]
public class PuntoMapa : MonoBehaviour
{
    [Tooltip("Único por evento, con el capítulo: cap1_sanfernando, cap2_laluna...")]
    public string id;

    [Tooltip("Nombre exacto de la escena del evento (debe estar en Build Settings)")]
    public string escenaEvento;

    [Header("Opcional: colocar el punto por píxeles de la imagen")]
    [Tooltip("Si está marcado, el punto se coloca solo sobre el mapa (su padre debe tener la Image del mapa)")]
    public bool colocarPorPixel = false;
    [Tooltip("Origen: esquina superior izquierda de la imagen original")]
    public Vector2 posicionPixel;

    Button boton;

    public bool Completado => ProgresoJuego.EstaCompletado(id);

    void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(Abrir);
        if (colocarPorPixel) ColocarPorPixel();
    }

    // Lo llama GestorMapa al arrancar la escena.
    public void Refrescar()
    {
        boton.interactable = !Completado; // cada evento se ve una sola vez
    }

    void Abrir()
    {
        if (string.IsNullOrEmpty(escenaEvento))
        {
            Debug.LogError("PuntoMapa '" + name + "': falta escribir 'escenaEvento'.");
            return;
        }

        ProgresoJuego.EventoActual = id;
        SceneManager.LoadScene(escenaEvento);
    }

    void ColocarPorPixel()
    {
        Image mapa = transform.parent != null ? transform.parent.GetComponent<Image>() : null;
        if (mapa == null || mapa.sprite == null) return;

        Vector2 tam = mapa.sprite.rect.size;
        Vector2 ancla = new Vector2(posicionPixel.x / tam.x, 1f - posicionPixel.y / tam.y);

        RectTransform rt = (RectTransform)transform;
        rt.anchorMin = ancla;
        rt.anchorMax = ancla;
        rt.anchoredPosition = Vector2.zero;
    }
}