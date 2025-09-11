using System;

namespace FavorApp.modelos
{
    public class Notificacion
    {
        public string Mensaje { get; set; }
        public int Prioridad { get; set; } // 1 = baja, 2 = media, 3 = alta
        public DateTime FechaCreacion { get; set; }

        public Notificacion(string mensaje, int prioridad)
        {
            Mensaje = mensaje;
            Prioridad = prioridad;
            FechaCreacion = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{FechaCreacion:dd/MM/yyyy HH:mm}] [Prioridad {Prioridad}] {Mensaje}";
        }
    }
}