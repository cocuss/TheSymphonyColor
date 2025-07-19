using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject PanelOpciones;
    public GameObject BotonInicio;
    public GameObject BotonOpciones;
    public GameObject BotonSalir;
    public GameObject BotonCreditos;
    public GameObject PanelCreditos;

    public void Iniciar()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void Opciones()
    {
        BotonInicio.SetActive(false);
        BotonOpciones.SetActive(false);
        BotonSalir.SetActive(false);
        BotonCreditos.SetActive(false);
        PanelOpciones.SetActive(true);
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void SalirOpciones()
    {
        PanelOpciones.SetActive(false);
        BotonInicio.SetActive(true);
        BotonOpciones.SetActive(true);
        BotonSalir.SetActive(true);
        BotonCreditos.SetActive(true);
    }
    public void AbrirCreditos()
    {
        BotonInicio.SetActive(false);
        BotonOpciones.SetActive(false);
        BotonSalir.SetActive(false);
        BotonCreditos.SetActive(false);
        PanelCreditos.SetActive(true);
    }
    public void CerrarCreditos()
    {
        PanelCreditos.SetActive(false);
        BotonInicio.SetActive(true);
        BotonOpciones.SetActive(true);
        BotonSalir.SetActive(true);
        BotonCreditos.SetActive(true);
    }
}
