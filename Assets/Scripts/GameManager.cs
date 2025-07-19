using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Button> BotonesColores;
    private List<int> secuencia=new List<int>();
    private bool Turno = false;
    private int indiceJugador = 0;
    public TextMeshProUGUI TextoContador;
    public TextMeshProUGUI TextoPuntaje;
    public GameObject PanelGameOver;
    private int puntaje = 0;
    public TextMeshProUGUI puntajeFinal;
    public List<AudioClip> audioClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        iniciarJuego();
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void iniciarJuego()
    {
        secuencia.Clear();
        puntaje=0;
        TextoPuntaje.text = "" + puntaje;
        PanelGameOver.SetActive(false);
        StartCoroutine(Contador());
        
    }

    public void AgregarColor()
    {
        int ColorRandom = Random.Range(0, 4);
        secuencia.Add(ColorRandom);
        StartCoroutine(MostrarSecuencia());

    }
    IEnumerator Contador()
    {
        TextoContador.gameObject.SetActive(true);
        for (int i = 3; i >= 1; i--)
        {
            TextoContador.text = "" + i;
            yield return StartCoroutine(AnimarEscala());

        }
        TextoContador.text = "GO!";
        yield return StartCoroutine(AnimarEscala());
        TextoContador.gameObject.SetActive(false);
        AgregarColor();
    }

    IEnumerator AnimarEscala()
    {
        float duracion = 1f;
        float tiempo = 0f;
        Vector3 escalaInicial = Vector3.one * 0.5f;
        Vector3 escalaFinal = Vector3.one;

        TextoContador.transform.localScale = escalaInicial;

        while (tiempo < duracion)
        {
            TextoContador.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        TextoContador.transform.localScale = escalaFinal;
    }

    IEnumerator MostrarSecuencia()
    {
        Turno = false;

        for (int i = 0; i < secuencia.Count; i++)
        {
            int indice = secuencia[i];
            Image imagenBoton = BotonesColores[indice].image;
            Color original = imagenBoton.color;

            // Intensificar el color simulando un foco encendido (ej: +40% intensidad)
            Color iluminado = original * 1.8f;
            iluminado.a = 1f; // Asegurar que la opacidad no se sobrepase

            imagenBoton.color = iluminado;
            Debug.Log("Reproduciendo sonido para el color: " + indice);
            audioSource.PlayOneShot(audioClips[indice]);
            yield return new WaitForSeconds(0.3f);

            imagenBoton.color = original;
            yield return new WaitForSeconds(0.3f);
        }

        indiceJugador = 0;
        Turno = true;
    }

    public void PresionarColor(int indice)
    {
        if (!Turno) return;

        if (indice == secuencia[indiceJugador])
        {
            indiceJugador++;
            if (indiceJugador >= secuencia.Count)
            {
                puntaje++;
                TextoPuntaje.text = "" + puntaje;
                Turno = false;
                Invoke("AgregarColor", 1.0f);
            }
        }
        else
        {
            PanelGameOver.SetActive(true);
            puntajeFinal.text = "" + puntaje;
        }
    }

    public void Reiniciar()
    {
        iniciarJuego();
    }

    public void Salir()
    {
        Application.Quit();
    }
}
