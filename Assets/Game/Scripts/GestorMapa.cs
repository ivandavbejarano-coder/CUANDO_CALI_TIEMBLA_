using UnityEngine;
using UnityEngine.UI;

// Va una vez en cada escena de mapa. Hace solo dos cosas:
// actualizar el estado de los puntos y mostrar el botón de continuar al completarlos todos.
// El cambio de escena del botón lo hace CargadorEscenas desde su On Click.
public class GestorMapa : MonoBehaviour
{
    public PuntoMapa[] puntos;
    public Button botonContinuar;

    void Start()
    {
        bool todosCompletados = puntos.Length > 0;

        foreach (PuntoMapa p in puntos)
        {
            p.Refrescar();
            if (!p.Completado) todosCompletados = false;
        }

        if (botonContinuar != null)
            botonContinuar.gameObject.SetActive(todosCompletados);
    }
}