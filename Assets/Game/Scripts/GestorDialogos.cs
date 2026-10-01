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
    public Image imagenPersonaje; // Casilla para controlar el sprite del personaje

    [Header("Configuración de Escena")]
    public string nombrePersonaje;
    public string escenaRegreso = "MapaCapitulo1";

    [Header("Configuración de Escritura")]
    public float velocidadEscritura = 0.03f;

    
    [System.Serializable]
    public class Linea
    {
        public Sprite expresionPersonaje; // Imagen opcional para esta línea
        [TextArea(3, 5)]
        public string texto;
    }

    [Header("Lista de Diálogos")]
    public Linea[] lineasDeDialogo;

    private int indiceActual = 0;
    private bool estaEscribiendo = false;
    private Coroutine corrutinaTexto;

    void Start()
    {
        textoNombre.text = nombrePersonaje;
        if (lineasDeDialogo.Length > 0)
        {
            MostrarLineaActual();
        }
    }

    public void SiguienteLinea()
    {
        if (estaEscribiendo)
        {
            StopCoroutine(corrutinaTexto);
            textoDialogo.text = lineasDeDialogo[indiceActual].texto;
            estaEscribiendo = false;
            return;
        }

        indiceActual++;

        if (indiceActual < lineasDeDialogo.Length)
        {
            MostrarLineaActual();
        }
        else
        {
            SceneManager.LoadScene(escenaRegreso);
        }
    }

    void MostrarLineaActual()
    {
        // Cambia la imagen del personaje si asignaste un sprite para esta línea
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
    }
}