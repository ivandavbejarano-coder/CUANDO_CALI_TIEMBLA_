using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Va en el objeto "Personaje" (Image) de cada escena de evento.
// Al iniciar la escena, el personaje pasa de transparente a sólido
// mientras se desplaza desde una posición cercana hasta la suya.
[RequireComponent(typeof(Image))]
public class AparicionPersonaje : MonoBehaviour
{
    [Tooltip("Segundos que espera antes de empezar a aparecer")]
    public float retraso = 0.3f;

    [Tooltip("Segundos que dura la aparición")]
    public float duracion = 1f;

    [Tooltip("Desde dónde empieza, respecto a su posición final (unidades del Canvas). Ej: (-60, 0) entra desde la izquierda; (0, -40) sube desde abajo")]
    public Vector2 desplazamiento = new Vector2(-60f, 0f);

    public AnimationCurve curva = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    Image imagen;
    RectTransform rt;
    Vector2 posicionFinal;

    void Awake()
    {
        imagen = GetComponent<Image>();
        rt = (RectTransform)transform;
        posicionFinal = rt.anchoredPosition; // la posición donde lo dejaste en el editor

        FijarAlpha(0f);
        rt.anchoredPosition = posicionFinal + desplazamiento;
    }

    void Start()
    {
        StartCoroutine(Aparecer());
    }

    IEnumerator Aparecer()
    {
        yield return new WaitForSeconds(retraso);

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            float k = curva.Evaluate(Mathf.Clamp01(t / duracion));
            FijarAlpha(k);
            rt.anchoredPosition = Vector2.Lerp(posicionFinal + desplazamiento, posicionFinal, k);
            yield return null;
        }

        FijarAlpha(1f);
        rt.anchoredPosition = posicionFinal;
    }

    void FijarAlpha(float a)
    {
        Color c = imagen.color;
        c.a = a;
        imagen.color = c;
    }
}
