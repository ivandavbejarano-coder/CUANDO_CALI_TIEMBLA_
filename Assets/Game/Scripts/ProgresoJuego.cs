using UnityEngine;

// Clase estática: no va en ningún GameObject.
// Guarda qué eventos ya se vieron, usando el "id" de cada punto del mapa.
public static class ProgresoJuego
{
    const string Clave = "progreso_eventos";

    // Lo fija el punto del mapa antes de abrir su escena; lo lee la escena del evento al terminar.
    public static string EventoActual;

    public static bool EstaCompletado(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        foreach (string e in PlayerPrefs.GetString(Clave, "").Split(';'))
            if (e == id) return true;
        return false;
    }

    public static void Completar(string id)
    {
        if (string.IsNullOrEmpty(id) || EstaCompletado(id)) return;
        string guardado = PlayerPrefs.GetString(Clave, "");
        PlayerPrefs.SetString(Clave, guardado == "" ? id : guardado + ";" + id);
        PlayerPrefs.Save();
    }

    // Se llama desde la escena de evento cuando termina el diálogo.
    public static void CompletarActual()
    {
        Completar(EventoActual);
        EventoActual = null;
    }
}