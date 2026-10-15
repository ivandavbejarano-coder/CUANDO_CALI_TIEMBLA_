using UnityEngine;
using UnityEngine.SceneManagement;

// Va en un objeto de cada escena que tenga botones de cambio de escena (menú y mapas).
// Cada botón elige un método en su On Click. Los nombres de escena se editan solo aquí.
public class CargadorEscenas : MonoBehaviour
{
    // Deben coincidir EXACTAMENTE con el nombre de la escena en la ventana Project.
    const string EscenaMenu = "MainMenu";
    const string EscenaMapa1 = "Mapa_Capitulo1";
    const string EscenaMapa2 = "Mapa_Capitulo2";
    const string EscenaMapa3 = "Mapa_Capitulo3";

    public void IrAlMenu() => Cargar(EscenaMenu);
    public void IrAlMapaCapitulo1() => Cargar(EscenaMapa1); // botón Iniciar del menú
    public void IrAlMapaCapitulo2() => Cargar(EscenaMapa2); // BotonContinuar del mapa 1
    public void IrAlMapaCapitulo3() => Cargar(EscenaMapa3); // BotonContinuar del mapa 2

    // Genérico: se mantiene para no romper botones que ya configuraste con él.
    public void CargarEscena(string nombreEscena) => Cargar(nombreEscena);

    void Cargar(string nombre)
    {
        if (!Application.CanStreamedLevelBeLoaded(nombre))
        {
            Debug.LogError("CargadorEscenas: la escena '" + nombre + "' no existe o no está en Build Settings.");
            return;
        }

        SceneManager.LoadScene(nombre);
    }
}