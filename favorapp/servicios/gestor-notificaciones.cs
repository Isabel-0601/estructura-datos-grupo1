using System;
using FavorApp.modelos;
using FavorApp.estructuras_datos;

namespace FavorApp.servicios
{
    public class GestorNotificaciones
    {
        private readonly ColaPrioridad cola = new ColaPrioridad();

        public void EnviarNotificacion(string mensaje, int prioridad)
        {
            cola.Encolar(new Notificacion(mensaje, prioridad));
        }

        // Procesa y muestra todas las notificaciones (desencola)
        public void MostrarNotificaciones()
        {
            if (cola.EstaVacia())
            {
                Console.WriteLine("No hay notificaciones pendientes.");
                return;
            }

            Console.WriteLine("Procesando notificaciones (las removemos de la cola):");
            while (!cola.EstaVacia())
            {
                var n = cola.Desencolar();
                Console.WriteLine(n);
            }
        }
    }
}

