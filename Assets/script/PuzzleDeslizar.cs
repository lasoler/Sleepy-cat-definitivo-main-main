using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleDeslizar : MonoBehaviour
{
    [Header("Configuración de los Botones")]
    // Arrastra aquí tus 9 botones en orden inicial perfecto (del 1 al 8, y el 9º el Vacio)
    public List<Button> botonesLista = new List<Button>();

    private int indiceHuecoVacio;
    private List<GameObject> ordenCorrectoSolucion = new List<GameObject>();

    void Start()
    {
        // Guardamos el orden original exacto de los objetos antes de mezclarlos
        foreach (Button boton in botonesLista)
        {
            ordenCorrectoSolucion.Add(boton.gameObject);
        }

        AsignarFuncionesABotones();
        ActualizarIndiceVacio();
        MezclarPuzzle();
    }

    void AsignarFuncionesABotones()
    {
        foreach (Button boton in botonesLista)
        {
            Button botonPresionado = boton;
            botonPresionado.onClick.AddListener(() => IntentarMoverBoton(botonPresionado));
        }
    }

    void ActualizarIndiceVacio()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "Vacio")
            {
                indiceHuecoVacio = i;
                break;
            }
        }
    }

    public void IntentarMoverBoton(Button botonPulsado)
    {
        int indiceBotonPulsado = botonPulsado.transform.GetSiblingIndex();

        if (EsMovimientoValido(indiceBotonPulsado, indiceHuecoVacio))
        {
            IntercambiarBotonesVisuales(indiceBotonPulsado, indiceHuecoVacio);
            ComprobarVictoria();
        }
    }

    bool EsMovimientoValido(int pos1, int pos2)
    {
        int x1 = pos1 % 3;
        int y1 = pos1 / 3;
        int x2 = pos2 % 3;
        int y2 = pos2 / 3;
        return (Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2)) == 1;
    }

    void IntercambiarBotonesVisuales(int pos1, int pos2)
    {
        Transform trans1 = transform.GetChild(pos1);
        Transform trans2 = transform.GetChild(pos2);

        trans1.SetSiblingIndex(pos2);
        trans2.SetSiblingIndex(pos1);

        ActualizarIndiceVacio();
    }

    void ComprobarVictoria()
    {
        // SISTEMA DE DETECCIÓN MEJORADO:
        // Compara si la posición física actual en la pantalla coincide con la lista de solución
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject != ordenCorrectoSolucion[i])
            {
                return; // Si uno solo está fuera de su sitio original, no ha ganado aún
            }
        }

        Debug.Log("¡Puzzle del sótano resuelto con éxito!");
       
        PuzzleSotanoVictoria victoriaSotano = FindFirstObjectByType<PuzzleSotanoVictoria>();
        if (victoriaSotano != null)
        {
            victoriaSotano.ProcesarVictoria();
        }
        else
        {
            Debug.LogError("ERROR: No se encontró el script 'PuzzleSotanoVictoria' en la escena.");
        }
    }

    void MezclarPuzzle()
    {
        for (int i = 0; i < 30; i++)
        {
            List<int> movimientosPosibles = new List<int>();
            for (int j = 0; j < transform.childCount; j++)
            {
                if (EsMovimientoValido(j, indiceHuecoVacio))
                {
                    movimientosPosibles.Add(j);
                }
            }
            int movimientoAzar = movimientosPosibles[Random.Range(0, movimientosPosibles.Count)];
            IntercambiarBotonesVisuales(movimientoAzar, indiceHuecoVacio);
        }
    }
}