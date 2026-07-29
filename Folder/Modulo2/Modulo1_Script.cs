using System;
using Unity.VisualScripting;
using UnityEngine;

public class Modulo1_Script : MonoBehaviour
{
    [SerializeField] public int numero1 = 10;
    [SerializeField] public int numero2 = 20;
    [SerializeField] public int numero3 = 30;
    [SerializeField] public bool verdadeiro = true;
    // string texto = "123";


     void OnValidate()
    {
        RodarCalculos();
    }

    void Start()
    {
            RodarCalculos();

    }
        void RodarCalculos()
    {

        if (verdadeiro)
        {
            //Operadores aritméticos

            Debug.Log("Soma: " + (numero1 + numero2));
            Debug.Log("Subtração: " + (numero2 - numero1));
            Debug.Log("Multiplicação: " + (numero1 * numero2));
            Debug.Log("Divisão: " + (numero2 / numero1));
        }
        else
        {
            //Operadores de comparação
            Debug.Log("Igual a: " + (numero1 == numero2));
            Debug.Log("Diferente de: " + (numero1 != numero2));
            Debug.Log("Maior que: " + (numero1 > numero2));
            Debug.Log("Menor que: " + (numero1 < numero2));
            Debug.Log("Maior ou igual a: " + (numero1 >= numero2));
            Debug.Log("Menor ou igual a: " + (numero1 <= numero2));
            Debug.Log("E lógico: " + (verdadeiro && (numero1 < numero2)));
            Debug.Log("Ou lógico: " + (verdadeiro || (numero1 > numero2)));
            Debug.Log("Negação: " + (!verdadeiro));

        }
        //cast
        float numero4 = (float)numero3 / (float)numero2;
        Debug.Log("Cast: " + numero4);

        char MELHORLETRA = (char)65;
        Debug.Log("Int para char: " + MELHORLETRA);

        char letra = 'A';
        int letraInt = (int)letra;
        Debug.Log("Char para int: " + letraInt);
        //cast por meio do Convert

        bool Bool = Convert.ToBoolean(letraInt);
        Debug.Log("int para bool: " + Bool);

        float numero5 = Convert.ToSingle("67");
        numero5 = float.Parse("67");
        Debug.Log($"float para String + {numero5}");
    }    

}

