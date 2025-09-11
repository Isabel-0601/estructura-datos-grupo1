using System;
using System.Collections.Generic;

namespace FavorApp.modelos
{
    // Clase que representa una solicitud dentro de la aplicación
    public class Solicitud
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaLimite { get; set; }
        public string Estado { get; set; } = "Activa";
        public Usuario Solicitante { get; set; } = null!;

        public List<Usuario> Postulantes { get; set; } = new List<Usuario>();

        // 👇 Ahora puede ser null hasta que alguien sea aceptado
        public Usuario? Aceptado { get; set; }

        // Constructor vacío
        public Solicitud()
        {
        }

        // Constructor que permite inicializar todas las propiedades principales
        public Solicitud(int id, string descripcion, decimal monto, DateTime fechaCreacion, DateTime fechaLimite, Usuario solicitante)
        {
            Id = id;
            Descripcion = descripcion;
            Monto = monto;
            FechaCreacion = fechaCreacion;
            FechaLimite = fechaLimite;
            Estado = "Activa";
            Solicitante = solicitante;
            Postulantes = new List<Usuario>();
            Aceptado = null; // Inicialmente nadie ha sido aceptado
        }

        // Método para representar la solicitud como texto legible
        public override string ToString()
        {
            // Si hay un usuario aceptado, se agrega al texto; si no, se omite
            string aceptadoStr = Aceptado != null
                ? $" | Aceptado: {Aceptado.Nombre} (CI {Aceptado.Id})"
                : "";

            return $"[ID: {Id}] {Descripcion} | Monto: {Monto} Bs | Creada: {FechaCreacion:dd/MM/yyyy} | " +
                   $"Límite: {FechaLimite:dd/MM/yyyy} | Estado: {Estado}{aceptadoStr}\n" +
                   $"   → Solicitante: {Solicitante.Nombre} ({Solicitante.Telefono}) | Postulantes: {Postulantes.Count}";
        }
    }
}
