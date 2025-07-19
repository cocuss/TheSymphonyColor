using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    public List<Button> BotonesColores;
    private List<int> secuencia = new List<int>();
    private bool Turno = false;
    private int indiceJugador = 0;
    public TextMeshProUGUI TextoContador;
    public TextMeshProUGUI TextoPuntaje;
    public GameObject PanelGameOver;
    public GameObject SeccionColores;
    public GameObject SeccionScore;
    private int puntaje = 0;
    public TextMeshProUGUI puntajeFinal;
    public List<AudioClip> audioClips;
    private AudioSource audioSource;

    private int partidasJugadas = 0;
    private string androidGameId = "5903992";
    private string iosGameId = "5903993";
    private string adUnitId = "Interstitial_Android"; // O "Interstitial_iOS" si estás en iOS
    private bool mostrarAdAntesDeReiniciar = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(androidGameId, true, this);
#elif UNITY_IOS
            Advertisement.Initialize(iosGameId, true, this);
#endif

        iniciarJuego();
    }

    public void iniciarJuego()
    {
        secuencia.Clear();
        SeccionColores.SetActive(true);
        SeccionScore.SetActive(true);
        puntaje = 0;
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

            Color iluminado = original * 1.8f;
            iluminado.a = 1f;

            imagenBoton.color = iluminado;
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
            SeccionColores.SetActive(false);
            SeccionScore.SetActive(false);
            PanelGameOver.SetActive(true);
            puntajeFinal.text = "" + puntaje;
        }
    }

    public void Reiniciar()
    {
        partidasJugadas++;

        if (partidasJugadas % 2 == 0)
        {
            mostrarAdAntesDeReiniciar = true;
            Advertisement.Show(adUnitId, this);
        }
        else
        {
            iniciarJuego();
        }
    }

    public void Salir()
    {
        Application.Quit();
    }

    // Callbacks requeridos por Unity Ads
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads inicializado correctamente.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning($"Falló la inicialización de Ads: {error} - {message}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && mostrarAdAntesDeReiniciar)
        {
            mostrarAdAntesDeReiniciar = false;
            iniciarJuego();
        }
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning($"Falló el anuncio: {error} - {message}");
        if (mostrarAdAntesDeReiniciar)
        {
            mostrarAdAntesDeReiniciar = false;
            iniciarJuego();
        }
    }
}
