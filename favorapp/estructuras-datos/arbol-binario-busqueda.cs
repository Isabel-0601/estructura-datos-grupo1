using System;
using System.Collections.Generic;
using FavorApp.modelos;

namespace FavorApp.estructuras_datos
{
    // Clase que implementa un Árbol Binario de Búsqueda (ABB) para almacenar objetos de tipo Solicitud
    public class ArbolBinarioBusqueda
    {
        private NodoABB? raiz;

        // Método público para insertar una solicitud en el árbol
        public void Insertar(Solicitud solicitud)
        {
            raiz = InsertarRecursivo(raiz, solicitud);
        }

        // Método recursivo que inserta un nodo en la posición correcta
        private NodoABB InsertarRecursivo(NodoABB? nodo, Solicitud solicitud)
        {
            if (nodo == null) // Caso base: si el nodo es nulo, se crea un nuevo nodo con la solicitud
                return new NodoABB(solicitud);

            if (solicitud.Id < nodo.Dato.Id)
                nodo.Izquierda = InsertarRecursivo(nodo.Izquierda, solicitud);
            else if (solicitud.Id > nodo.Dato.Id)
                nodo.Derecha = InsertarRecursivo(nodo.Derecha, solicitud);

            return nodo;
        }

        // Método público para buscar una solicitud por su id
        public Solicitud? Buscar(int id)
        {
            return BuscarRecursivo(raiz, id);
        }

        private Solicitud? BuscarRecursivo(NodoABB? nodo, int id)
        {
            if (nodo == null) return null;
            if (id == nodo.Dato.Id) return nodo.Dato;
            return id < nodo.Dato.Id ? BuscarRecursivo(nodo.Izquierda, id) : BuscarRecursivo(nodo.Derecha, id);
        }

        // Método público para eliminar una solicitud por su id
        public void Eliminar(int id)
        {
            raiz = EliminarRecursivo(raiz, id);
        }

        // Método recursivo que elimina un nodo del árbol
        private NodoABB? EliminarRecursivo(NodoABB? nodo, int id)
        {
            if (nodo == null) return null;

            if (id < nodo.Dato.Id)
                nodo.Izquierda = EliminarRecursivo(nodo.Izquierda, id);
            else if (id > nodo.Dato.Id)
                nodo.Derecha = EliminarRecursivo(nodo.Derecha, id);
            else
            {
                if (nodo.Izquierda == null) return nodo.Derecha;
                if (nodo.Derecha == null) return nodo.Izquierda;

                nodo.Dato = Minimo(nodo.Derecha).Dato;
                nodo.Derecha = EliminarRecursivo(nodo.Derecha, nodo.Dato.Id);
            }

            return nodo;
        }

        private NodoABB Minimo(NodoABB nodo)
        {
            while (nodo.Izquierda != null)
                nodo = nodo.Izquierda;
            return nodo;
        }

        // Método público para obtener una lista de solicitudes en orden ascendente (inorden)
        public List<Solicitud> ObtenerInorden()
        {
            List<Solicitud> lista = new List<Solicitud>();
            InordenRecursivo(raiz, lista);
            return lista;
        }

        // Método recursivo que recorre el árbol en inorden y llena la lista
        private void InordenRecursivo(NodoABB? nodo, List<Solicitud> lista)
        {
            if (nodo != null)
            {
                InordenRecursivo(nodo.Izquierda, lista);
                lista.Add(nodo.Dato);
                InordenRecursivo(nodo.Derecha, lista);
            }
        }
    }
}
