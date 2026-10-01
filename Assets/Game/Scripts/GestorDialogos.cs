using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GestorDialogos : MonoBehaviour
{
    [Header("Componentes de la Interfaz")]
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;
    public Image imagenPersonaje;

    [Header("Panel de Decisiones (Opciones)")]
    public GameObject panelOpciones;
    public Button botonOpcionA;
    public TextMeshProUGUI textoOpcionA;
    public Button botonOpcionB;
    public TextMeshProUGUI textoOpcionB;

    [Header("Configuración de Escena")]
    public string nombrePersonaje;
    public string claveEventoCompletado = "SanFernandoCompletado"; // Identificador para guardar progreso
    public string escenaRegreso = "MapaCapitulo1";
    public float velocidadEscritura = 0.03f;

    [System.Serializable]
    public class Opcion
    {
        public string textoBoton;
        public int indiceDestino; // Línea de diálogo a la que salta si elige esta opción
    }

    [System.Serializable]
    public class Linea
    {
        public Sprite expresionPersonaje;
        [TextArea(3, 5)]
        public string texto;
        public bool esDecision; // Marca si esta línea debe desplegar opciones
        public Opcion opcionA;
        public Opcion opcionB;
    }

    [Header("Lista de Diálogos")]
    public Linea[] lineasDeDialogo;

    private int indiceActual = 0;
    private bool estaEscribiendo = false;
    private bool esperandoDecision = false;
    private Coroutine corrutinaTexto;

    void Start()
    {
        textoNombre.text = nombrePersonaje;
        if (panelOpciones != null) panelOpciones.SetActive(false);

        if (lineasDeDialogo.Length > 0)
        {
            MostrarLineaActual();
        }
    }

    public void SiguienteLinea()
    {
        // Si hay una decisión en pantalla, no avanzar con clic directo
        if (esperandoDecision) return;

        if (estaEscribiendo)
        {
            StopCoroutine(corrutinaTexto);
            textoDialogo.text = lineasDeDialogo[indiceActual].texto;
            estaEscribiendo = false;
            ComprobarDecision();
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

    void MostrarLineaActual()
    {
        if (lineasDeDialogo[indiceActual].expresionPersonaje != null && imagenPersonaje != null)
        {
            imagenPersonaje.sprite = lineasDeDialogo[indiceActual].expresionPersonaje;
        }

        corrutinaTexto = StartCoroutine(EscribirLinea(lineasDeDialogo[indiceActual].texto));
    }

    IEnumerator EscribirLinea(string linea)
    {
        estaEscribiendo = true;
        textoDialogo.text = "";

        foreach (char letra in linea.ToCharArray())
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        estaEscribiendo = false;
        ComprobarDecision();
    }

    void ComprobarDecision()
    {
        if (lineasDeDialogo[indiceActual].esDecision)
        {
            esperandoDecision = true;
            panelOpciones.SetActive(true);

            textoOpcionA.text = lineasDeDialogo[indiceActual].opcionA.textoBoton;
            textoOpcionB.text = lineasDeDialogo[indiceActual].opcionB.textoBoton;

            botonOpcionA.onClick.RemoveAllListeners();
            botonOpcionA.onClick.AddListener(() => SeleccionarOpcion(lineasDeDialogo[indiceActual].opcionA.indiceDestino));

            botonOpcionB.onClick.RemoveAllListeners();
            botonOpcionB.onClick.AddListener(() => SeleccionarOpcion(lineasDeDialogo[indiceActual].opcionB.indiceDestino));
        }
    }

    void SeleccionarOpcion(int siguienteIndice)
    {
        panelOpciones.SetActive(false);
        esperandoDecision = false;
        indiceActual = siguienteIndice;
        MostrarLineaActual();
    }

    void FinalizarEvento()
    {
        // Guardar que este punto de la ciudad ya fue completado
        if (!string.IsNullOrEmpty(claveEventoCompletado))
        {
            PlayerPrefs.SetInt(claveEventoCompletado, 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene(escenaRegreso);
    }
}