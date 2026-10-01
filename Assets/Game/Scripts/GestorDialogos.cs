using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
public class GestorDialogos : MonoBehaviour
{
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;

    public string nombrePersonaje;
    public string escenaRegreso = "MapaCapitulo1"; 

    [TextArea(3, 5)]
    public string[] lineasDeDialogo;

    private int indiceActual = 0;

    void Start()
    {
        textoNombre.text = nombrePersonaje;
        if (lineasDeDialogo.Length > 0)
        {
            textoDialogo.text = lineasDeDialogo[0];
        }
    }

    public void SiguienteLinea()
    {
        indiceActual++;

        if (indiceActual < lineasDeDialogo.Length)
        {
            textoDialogo.text = lineasDeDialogo[indiceActual];
        }
        else
        {
            // Al terminar la última frase, vuelve al mapa
            SceneManager.LoadScene(escenaRegreso);
        }
    }
}