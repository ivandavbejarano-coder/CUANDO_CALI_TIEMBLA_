using UnityEngine;
using UnityEngine.UI;

// Va en el objeto del fondo (sirve con Sprite Renderer o con Image de UI).
public class FondoAnimado : MonoBehaviour
{
    [Tooltip("Los cuadros de la animación, en orden")]
    public Sprite[] cuadros;

    [Tooltip("Cuadros por segundo (2 = lento, 6 = rápido)")]
    public float cuadrosPorSegundo = 4f;

    [Tooltip("Si está activo, va 1-2-3-4-5-4-3-2-1. Si no, 1-2-3-4-5-1-2-3...")]
    public bool idaYVuelta = false;

    SpriteRenderer spriteRenderer;
    Image imagenUI;
    float tiempo;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        imagenUI = GetComponent<Image>();

        if (spriteRenderer == null && imagenUI == null)
            Debug.LogError("FondoAnimado: este objeto necesita un Sprite Renderer o una Image.", this);
    }

    void Update()
    {
        if (cuadros == null || cuadros.Length == 0) return;

        int total = idaYVuelta && cuadros.Length > 1 ? cuadros.Length * 2 - 2 : cuadros.Length;
        tiempo += Time.deltaTime * cuadrosPorSegundo;
        int i = Mathf.FloorToInt(tiempo) % total;
        if (i >= cuadros.Length) i = total - i;

        if (spriteRenderer != null) spriteRenderer.sprite = cuadros[i];
        else if (imagenUI != null) imagenUI.sprite = cuadros[i];
    }
}