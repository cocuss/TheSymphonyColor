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

    public TextMeshProUGUI TextoPuntaje;
    public GameObject PanelGameOver;
    private int puntaje = 0;
    public TextMeshProUGUI puntajeFinal;


    // Start is called before the first frame update
    void Start()
    {
        iniciarJuego();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void iniciarJuego()
    {
        secuencia.Clear();
        puntaje=0;
        TextoPuntaje.text = "" + puntaje;
        PanelGameOver.SetActive(false);
        AgregarColor();
    }

    public void AgregarColor()
    {
        int ColorRandom = Random.Range(0, 4);
        secuencia.Add(ColorRandom);
        StartCoroutine(MostrarSecuencia());

    }
    IEnumerator MostrarSecuencia()
    {
        Turno = false;
        for (int i = 0; i < secuencia.Count; i++)
        {
            int indice = secuencia[i];
            Color original = BotonesColores[indice].image.color;

            BotonesColores[indice].image.color = Color.white;
            yield return new WaitForSeconds(0.3f);

            BotonesColores[indice].image.color = original;
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
