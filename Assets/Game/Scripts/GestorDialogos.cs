using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Va en cada escena de evento. Muestra las líneas de diálogo una a una.
// Si una línea es más larga que la caja de texto, se parte sola en páginas
// según el tamaño de la caja; cada clic avanza a la página siguiente.
// Al terminar, marca el evento como visto y regresa al mapa.
public class GestorDialogos : MonoBehaviour
{
    [Header("Componentes de la Interfaz")]
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;
    public Image imagenPersonaje;

    [Header("Configuración de Escena")]
    public string nombrePersonaje;
    public string escenaRegreso = "MapaCapitulo1";
    public float velocidadEscritura = 0.03f;
    [Tooltip("Segundos antes de empezar el diálogo (para dar tiempo a que aparezca el personaje)")]
    public float retrasoInicial = 1.3f;

    [System.Serializable]
    public class Linea
    {
        public Sprite expresionPersonaje;
        [TextArea(3, 12)]
        public string texto;
    }

    [Header("Lista de Diálogos")]
    public Linea[] lineasDeDialogo;

    private int indiceActual = 0;
    private int paginaActual = 1;
    private int paginasTotales = 1;
    private int finPaginaActual = 0;
    private bool estaEscribiendo = false;
    private bool listo = false;
    private Coroutine corrutinaTexto;

    void Start()
    {
        textoNombre.text = nombrePersonaje;

        // El texto se parte en páginas cuando no cabe en la caja
        textoDialogo.overflowMode = TextOverflowModes.Page;
        textoDialogo.text = "";

        if (lineasDeDialogo.Length > 0)
        {
            AplicarExpresion(); // el personaje aparece ya con su primer sprite
            StartCoroutine(EmpezarConRetraso());
        }
    }

    IEnumerator EmpezarConRetraso()
    {
        yield return new WaitForSeconds(retrasoInicial);
        listo = true;
        MostrarLineaActual();
    }

    public void SiguienteLinea()
    {
        if (!listo) return;

        // Si la página aún se está escribiendo, el clic la completa
        if (estaEscribiendo)
        {
            StopCoroutine(corrutinaTexto);
            textoDialogo.maxVisibleCharacters = finPaginaActual;
            estaEscribiendo = false;
            return;
        }

        // Si la línea tiene más páginas, pasa a la siguiente
        if (paginaActual < paginasTotales)
        {
            paginaActual++;
            corrutinaTexto = StartCoroutine(EscribirPagina());
            return;
        }

        indiceActual++;

        if (indiceActual < lineasDeDialogo.Length)
        {
            MostrarLineaActual();
        }
        else
        {
            FinalizarEvento();
        }
    }

    void AplicarExpresion()
    {
        if (lineasDeDialogo[indiceActual].expresionPersonaje != null && imagenPersonaje != null)
        {
            imagenPersonaje.sprite = lineasDeDialogo[indiceActual].expresionPersonaje;
        }
    }

    void MostrarLineaActual()
    {
        AplicarExpresion();

        textoDialogo.text = lineasDeDialogo[indiceActual].texto;
        textoDialogo.maxVisibleCharacters = 0;
        textoDialogo.pageToDisplay = 1;
        textoDialogo.ForceMeshUpdate(); // calcula cuántas páginas ocupa el texto

        paginasTotales = Mathf.Max(1, textoDialogo.textInfo.pageCount);
        paginaActual = 1;
        corrutinaTexto = StartCoroutine(EscribirPagina());
    }

    IEnumerator EscribirPagina()
    {
        estaEscribiendo = true;

        textoDialogo.pageToDisplay = paginaActual;

        TMP_PageInfo pagina = textoDialogo.textInfo.pageInfo[paginaActual - 1];
        int inicio = pagina.firstCharacterIndex;
        finPaginaActual = Mathf.Min(pagina.lastCharacterIndex + 1, textoDialogo.textInfo.characterCount);

        // Revela solo los caracteres de esta página, uno a uno
        textoDialogo.maxVisibleCharacters = inicio;
        for (int i = inicio + 1; i <= finPaginaActual; i++)
        {
            textoDialogo.maxVisibleCharacters = i;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        estaEscribiendo = false;
    }

    void FinalizarEvento()
    {
        // Marca como completado el evento que abrió el mapa y regresa a él
        ProgresoJuego.CompletarActual();
        SceneManager.LoadScene(escenaRegreso);
    }
}