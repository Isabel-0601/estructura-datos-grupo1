using System;
using System.Collections.Generic;
using FavorApp.modelos;

namespace FavorApp.estructuras_datos
{
    // Clase estática que implementa el algoritmo QuickSort para ordenar listas de Solicitud
    public static class QuickSort
    {
        // Método público para ordenar una lista de Solicitudes según una clave 
        public static void Ordenar(List<Solicitud> lista, Func<Solicitud, IComparable> keySelector)
        {
            if (lista == null || lista.Count <= 1) return;
            QuickSortRecursivo(lista, 0, lista.Count - 1, keySelector);
        }

        // Método recursivo que aplica el algoritmo QuickSort
        private static void QuickSortRecursivo(List<Solicitud> lista, int izquierda, int derecha, Func<Solicitud, IComparable> keySelector)
        {
            if (izquierda < derecha) // Condición de recursión
            {
                int indiceParticion = Particionar(lista, izquierda, derecha, keySelector);
                QuickSortRecursivo(lista, izquierda, indiceParticion - 1, keySelector);
                QuickSortRecursivo(lista, indiceParticion + 1, derecha, keySelector);
            }
        }

        // Método que realiza la partición del arreglo y coloca el pivote en su lugar correcto
        private static int Particionar(List<Solicitud> lista, int izquierda, int derecha, Func<Solicitud, IComparable> keySelector)
        {
            IComparable pivote = keySelector(lista[derecha]);
            int i = izquierda - 1;

            for (int j = izquierda; j < derecha; j++)
            {
                if (keySelector(lista[j]).CompareTo(pivote) <= 0)
                {
                    i++;
                    (lista[i], lista[j]) = (lista[j], lista[i]);
                }
            }

            (lista[i + 1], lista[derecha]) = (lista[derecha], lista[i + 1]);
            return i + 1;
        }
    }
}
