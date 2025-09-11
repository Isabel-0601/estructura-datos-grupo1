using System;
using System.Collections.Generic;
using System.Linq;
using FavorApp.modelos;
using FavorApp.estructuras_datos;

namespace FavorApp.servicios
{
    // Clase que gestiona las operaciones relacionadas con las solicitudes
    public class GestorSolicitudes
    {
        private readonly ArbolBinarioBusqueda arbol = new ArbolBinarioBusqueda();
        private readonly GestorNotificaciones gestorNotificaciones;
        private int nextId = 1;

        // Constructor: inicializa con algunas solicitudes de ejemplo
        public GestorSolicitudes(GestorNotificaciones gestorNotificaciones)
        {
            this.gestorNotificaciones = gestorNotificaciones;
            // Usuarios de prueba
            var u1 = new Usuario(1, "Ana Pérez", "70111111");
            var u2 = new Usuario(2, "Carlos Gómez", "70222222");
            var u3 = new Usuario(3, "María López", "70333333");
            var u4 = new Usuario(4, "Luis Fernández", "70444444");
            var u5 = new Usuario(5, "Pedro Sánchez", "70555555");

            // Solicitudes iniciale
            AgregarSolicitud(new Solicitud(0, "Compra de víveres", 150, DateTime.Now, DateTime.Now.AddDays(3), u1));
            AgregarSolicitud(new Solicitud(0, "Transporte al aeropuerto", 200, DateTime.Now, DateTime.Now.AddDays(2), u2));
            AgregarSolicitud(new Solicitud(0, "Pintar una habitación", 500, DateTime.Now, DateTime.Now.AddDays(7), u3));
            AgregarSolicitud(new Solicitud(0, "Cuidar mascota por 2 días", 300, DateTime.Now, DateTime.Now.AddDays(5), u4));
            AgregarSolicitud(new Solicitud(0, "Clases de matemáticas", 250, DateTime.Now, DateTime.Now.AddDays(4), u5));
        }

        //Crea una nueva solicitud y la inserta en el árbol
        public Solicitud CrearSolicitud(string descripcion, decimal monto, DateTime fechaLimite, Usuario solicitante)
        {
            var solicitud = new Solicitud(nextId++, descripcion, monto, DateTime.Now, fechaLimite, solicitante);
            arbol.Insertar(solicitud);
            gestorNotificaciones.EnviarNotificacion($"Tu solicitud '{descripcion}' fue publicada (ID {solicitud.Id}).", 2);
            return solicitud;
        }

        // Agrega una solicitud (si el Id es 0 se asigna uno nuevo)
        public void AgregarSolicitud(Solicitud solicitud)
        {
            if (solicitud.Id == 0) solicitud.Id = nextId++;
            arbol.Insertar(solicitud);
        }

        // Muestra todas las solicitudes vigentes
        public void MostrarSolicitudes()
        {
            RevisarExpiradas();
            var lista = arbol.ObtenerInorden();
            if (lista.Count == 0) Console.WriteLine("No hay solicitudes registradas.");
            else foreach (var s in lista) Console.WriteLine(s);
        }

        // Busca una solicitud por ID
        public Solicitud? BuscarSolicitud(int id)
        {
            RevisarExpiradas();
            return arbol.Buscar(id);
        }

        // Elimina una solicitud
        public void EliminarSolicitud(int id)
        {
            arbol.Eliminar(id);
            gestorNotificaciones.EnviarNotificacion($"Solicitud {id} eliminada.", 2);
        }

        // Muestra solicitudes ordenadas según un criterio
        public void MostrarSolicitudesOrdenadas(int criterio)
        {
            RevisarExpiradas();
            // CORRECCIÓN: comparar solo la parte de Fecha, no la hora
            var lista = arbol.ObtenerInorden()
                              .Where(s => s.Estado == "Activa" && s.FechaLimite.Date >= DateTime.Now.Date)
                              .ToList();

            if (lista.Count == 0)
            {
                Console.WriteLine("No hay solicitudes vigentes para ordenar.");
                return;
            }

            switch (criterio)
            {
                case 1: // Monto
                    QuickSort.Ordenar(lista, s => s.Monto);
                    break;
                case 2: // Fecha límite
                    QuickSort.Ordenar(lista, s => s.FechaLimite);
                    break;
                case 3: // Fecha creación
                    QuickSort.Ordenar(lista, s => s.FechaCreacion);
                    break;
                default:
                    Console.WriteLine("Criterio inválido. Mostrando por monto.");
                    QuickSort.Ordenar(lista, s => s.Monto);
                    break;
            }

            foreach (var s in lista) Console.WriteLine(s);
        }

        // Permite a un usuario postularse a una solicitud
        public void PostularSolicitud(int solicitudId, Usuario postulante)
        {
            RevisarExpiradas();
            var s = arbol.Buscar(solicitudId);
            if (s == null)
            {
                Console.WriteLine("⚠ Solicitud no encontrada.");
                return;
            }
            if (s.Estado != "Activa")
            {
                Console.WriteLine($"⚠ La solicitud no está activa (Estado: {s.Estado}).");
                return;
            }
            if (s.Postulantes.Any(p => p.Id == postulante.Id))
            {
                Console.WriteLine("⚠ Ya te postulaste a esta solicitud.");
                return;
            }
            s.Postulantes.Add(postulante);
            gestorNotificaciones.EnviarNotificacion($"Nuevo postulante {postulante.Nombre} (CI {postulante.Id}) para solicitud {s.Id}.", 2);
            Console.WriteLine("✔ Postulación registrada.");
        }

        // Acepta a un postulante y cambia el estado de la solicitud
        public void AceptarPostulacion(int solicitudId, int postulanteId)
        {
            RevisarExpiradas();
            var s = arbol.Buscar(solicitudId);
            if (s == null) { Console.WriteLine("⚠ Solicitud no encontrada."); return; }
            var postulante = s.Postulantes.FirstOrDefault(p => p.Id == postulanteId);
            if (postulante == null) { Console.WriteLine("⚠ Postulante no encontrado entre los postulantes."); return; }

            s.Aceptado = postulante;
            s.Estado = "En Proceso";
            gestorNotificaciones.EnviarNotificacion($"Tu solicitud {s.Id} fue aceptada por {postulante.Nombre} (CI {postulante.Id}).", 3);
            gestorNotificaciones.EnviarNotificacion($"Has sido aceptado para la solicitud {s.Id}. Contacta a {s.Solicitante.Nombre} ({s.Solicitante.Telefono}).", 2);
            Console.WriteLine("✔ Postulación aceptada y notificaciones enviadas.");
        }

        // Cierra una solicitud (la marca como completada y la elimina del árbol)
        public void CerrarSolicitud(int id)
        {
            RevisarExpiradas();
            var s = arbol.Buscar(id);
            if (s == null) { Console.WriteLine("⚠ Solicitud no encontrada."); return; }
            s.Estado = "Completada";
            arbol.Eliminar(id);
            gestorNotificaciones.EnviarNotificacion($"Solicitud {id} finalizada y removida de la lista de activas.", 2);
            Console.WriteLine("✔ Solicitud cerrada y eliminada del ABB.");
        }

        // Revisa solicitudes expiradas y las elimina (envía notificación alta)
        private void RevisarExpiradas()
        {
            var todas = arbol.ObtenerInorden().ToList();
            foreach (var s in todas)
            {
                if (s.Estado == "Activa" && s.FechaLimite.Date < DateTime.Now.Date)
                {
                    s.Estado = "Expirada";
                    arbol.Eliminar(s.Id);
                    gestorNotificaciones.EnviarNotificacion($"Solicitud {s.Id} ha expirado (límite {s.FechaLimite:dd/MM/yyyy}).", 3);
                }
                else
                {
                    var diasRestantes = (s.FechaLimite.Date - DateTime.Now.Date).Days;
                    if (s.Estado == "Activa" && diasRestantes == 0)
                    {
                        gestorNotificaciones.EnviarNotificacion($"Solicitud {s.Id} vence HOY ({s.FechaLimite:dd/MM/yyyy}). Postúlate pronto.", 3);
                    }
                }
            }
        }
    }
}
