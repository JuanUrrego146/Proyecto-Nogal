using UnityEngine;

public class CamaraOrbital : MonoBehaviour
{
    [SerializeField] 
    private Transform objetivoCamara;       // Punto alrededor del cual orbita la c�mara
    [SerializeField]
    private Transform pivoteVertical;       // Objeto que rota verticalmente (Pitch)
    [SerializeField]
    private float velocidadRotacion = 0.2f;
    [SerializeField] 
    private float anguloMinimo = 0f;        // L�mite inferior del pitch
    [SerializeField] 
    private float anguloMaximo = 45f;       // L�mite superior del pitch

    private Vector2 ultimaPosicionEntrada;
    private bool estaArrastrando = false;
    private float anguloActualPitch = 0f;

    void Update()
    {
        Vector2 deltaEntrada = Vector2.zero;

        // Entrada con rat�n (PC)
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

        // Entrada t�ctil (m�vil)
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

        // Aplicar rotaciones
        if (deltaEntrada != Vector2.zero)
        {
            // Rotaci�n horizontal (Yaw)
            transform.RotateAround(objetivoCamara.position, Vector3.up, deltaEntrada.x * velocidadRotacion * Time.deltaTime);

            // Rotaci�n vertical (Pitch)
            anguloActualPitch -= deltaEntrada.y * velocidadRotacion * Time.deltaTime;
            anguloActualPitch = Mathf.Clamp(anguloActualPitch, anguloMinimo, anguloMaximo);
            pivoteVertical.localEulerAngles = new Vector3(anguloActualPitch, 0, 0);
        }
    }
}
