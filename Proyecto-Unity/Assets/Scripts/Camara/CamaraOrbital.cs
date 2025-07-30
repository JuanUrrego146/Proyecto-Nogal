﻿using UnityEngine;

public class CamaraOrbital : MonoBehaviour
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [SerializeField] 
    private Transform objetivoCamara;           // Punto alrededor del cual orbita la cámara
    [SerializeField]
    private Transform pivoteVertical;           // Objeto que rota verticalmente (Pitch)
    [SerializeField]
    private float velocidadRotacion = 0.2f;
    [SerializeField] 
    private float anguloMinimo = 0f;            // Límite inferior del pitch
    [SerializeField] 
    private float anguloMaximo = 45f;           // Límite superior del pitch

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private Vector2 ultimaPosicionEntrada;
    private bool estaArrastrando = false;
    private float anguloActualPitch = 0f;

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    void Update()
    {
        GestionEntradasDipositivos();
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PRIVADOS
    // ───────────────────────────────────────
    private void GestionEntradasDipositivos()
    {
        Vector2 deltaEntrada = Vector2.zero;

        // Entrada PC
        deltaEntrada = EntradasConRaton(deltaEntrada);

        // Entrada móvil
        deltaEntrada = EntradasTactil(deltaEntrada);

        AplicarRotacion(deltaEntrada);
    }

    private void AplicarRotacion(Vector2 deltaEntrada)
    {
        if (deltaEntrada != Vector2.zero)
        {
            // Rotación horizontal (Yaw)
            transform.RotateAround(objetivoCamara.position, Vector3.up, deltaEntrada.x * velocidadRotacion * Time.deltaTime);

            // Rotación vertical (Pitch)
            anguloActualPitch -= deltaEntrada.y * velocidadRotacion * Time.deltaTime;
            anguloActualPitch = Mathf.Clamp(anguloActualPitch, anguloMinimo, anguloMaximo);
            pivoteVertical.localEulerAngles = new Vector3(anguloActualPitch, 0, 0);
        }
    }

    private Vector2 EntradasTactil(Vector2 deltaEntrada)
    {
        if (Input.touchCount == 1)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Began)
            {
                estaArrastrando = true;
                ultimaPosicionEntrada = toque.position;
            }
            else if (toque.phase == TouchPhase.Moved && estaArrastrando)
            {
                deltaEntrada = toque.deltaPosition;
            }
            else if (toque.phase == TouchPhase.Ended || toque.phase == TouchPhase.Canceled)
            {
                estaArrastrando = false;
            }
        }

        return deltaEntrada;
    }

    private Vector2 EntradasConRaton(Vector2 deltaEntrada)
    {
        if (Input.GetMouseButtonDown(0))
        {
            estaArrastrando = true;
            ultimaPosicionEntrada = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && estaArrastrando)
        {
            deltaEntrada = (Vector2)Input.mousePosition - ultimaPosicionEntrada;
            ultimaPosicionEntrada = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            estaArrastrando = false;
        }

        return deltaEntrada;
    }
}
