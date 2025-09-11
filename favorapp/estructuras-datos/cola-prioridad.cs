using System;
using FavorApp.modelos;

namespace FavorApp.estructuras_datos
{
    public class ColaPrioridad
    {
        private class NodoCola
        {
            public Notificacion Dato { get; set; }
            public NodoCola? Siguiente { get; set; }
            public NodoCola(Notificacion dato) => Dato = dato;
        }

        private NodoCola? frente;

        public void Encolar(Notificacion notificacion)
        {
            NodoCola nuevo = new NodoCola(notificacion);

            if (frente == null ||
                notificacion.Prioridad > frente.Dato.Prioridad ||
                (notificacion.Prioridad == frente.Dato.Prioridad && notificacion.FechaCreacion < frente.Dato.FechaCreacion))
            {
                nuevo.Siguiente = frente;
                frente = nuevo;
            }
            else
            {
                NodoCola actual = frente;
                // Avanzar mientras el siguiente tenga mayor prioridad, o igual prioridad pero sea anterior o igual en tiempo
                while (actual.Siguiente != null &&
                       (actual.Siguiente.Dato.Prioridad > notificacion.Prioridad ||
                        (actual.Siguiente.Dato.Prioridad == notificacion.Prioridad &&
                         actual.Siguiente.Dato.FechaCreacion <= notificacion.FechaCreacion)))
                {
                    actual = actual.Siguiente;
                }
                nuevo.Siguiente = actual.Siguiente;
                actual.Siguiente = nuevo;
            }
        }

        public Notificacion Desencolar()
        {
            if (frente == null) throw new InvalidOperationException("La cola está vacía");
            Notificacion dato = frente.Dato;
            frente = frente.Siguiente;
            return dato;
        }

        public bool EstaVacia() => frente == null;

        // Muestra sin desencolar (útil para debug); en práctica el gestor procesará la cola con Desencolar
        public void Mostrar()
        {
            NodoCola? actual = frente;
            while (actual != null)
            {
                Console.WriteLine(actual.Dato);
                actual = actual.Siguiente;
            }
        }
    }
}