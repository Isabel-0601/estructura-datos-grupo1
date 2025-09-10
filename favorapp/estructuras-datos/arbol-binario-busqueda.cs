using System;
using System.Collections.Generic;
using FavorApp.modelos;

namespace FavorApp.estructuras_datos
{
    public class ArbolBinarioBusqueda
    {
        private NodoABB? raiz;

        public void Insertar(Solicitud solicitud)
        {
            raiz = InsertarRecursivo(raiz, solicitud);
        }

        private NodoABB InsertarRecursivo(NodoABB? nodo, Solicitud solicitud)
        {
            if (nodo == null)
                return new NodoABB(solicitud);

            if (solicitud.Id < nodo.Dato.Id)
                nodo.Izquierda = InsertarRecursivo(nodo.Izquierda, solicitud);
            else if (solicitud.Id > nodo.Dato.Id)
                nodo.Derecha = InsertarRecursivo(nodo.Derecha, solicitud);

            return nodo;
        }

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

        public void Eliminar(int id)
        {
            raiz = EliminarRecursivo(raiz, id);
        }

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

        public List<Solicitud> ObtenerInorden()
        {
            List<Solicitud> lista = new List<Solicitud>();
            InordenRecursivo(raiz, lista);
            return lista;
        }

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
