using System;
using System.Globalization;
using System.Text.RegularExpressions;
using FavorApp.modelos;
using FavorApp.servicios;

namespace FavorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GestorNotificaciones gestorNotificaciones = new GestorNotificaciones();
            GestorSolicitudes gestor = new GestorSolicitudes(gestorNotificaciones);

            int opcion;
            do
            {
                Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
                Console.WriteLine("1. Crear Solicitud");
                Console.WriteLine("2. Listar Solicitudes (Inorden)");
                Console.WriteLine("3. Buscar Solicitud por ID");
                Console.WriteLine("4. Eliminar Solicitud por ID");
                Console.WriteLine("5. Ver Solicitudes Ordenadas (QuickSort)");
                Console.WriteLine("6. Postular a una Solicitud");
                Console.WriteLine("7. Aceptar Postulación");
                Console.WriteLine("8. Cerrar Solicitud (finalizar)");
                Console.WriteLine("9. Ver / Procesar Notificaciones");
                Console.WriteLine("10. Salir");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("⚠ Opción inválida. Intente nuevamente.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        // Crear usuario solicitante (validado)
                        int idUsuario;
                        while (true)
                        {
                            Console.Write("Ingrese ID de Usuario (CI numérico): ");
                            if (int.TryParse(Console.ReadLine(), out idUsuario)) break;
                            Console.WriteLine("⚠ ID inválido. Debe ser numérico.");
                        }

                        string nombre;
                        do
                        {
                            Console.Write("Ingrese nombre de Usuario: ");
                            nombre = (Console.ReadLine() ?? "").Trim();
                            if (string.IsNullOrWhiteSpace(nombre)) Console.WriteLine("⚠ El nombre no puede estar vacío.");
                        } while (string.IsNullOrWhiteSpace(nombre));

                        string telefono;
                        while (true)
                        {
                            Console.Write("Ingrese teléfono (8 dígitos): ");
                            telefono = Console.ReadLine()??"".Trim();
                            if (telefono != null && Regex.IsMatch(telefono, @"^\d{8}$")) break;
                            Console.WriteLine("⚠ Teléfono inválido. Solo 8 dígitos numéricos.");
                        }

                        Usuario usuario = new Usuario(idUsuario, nombre, telefono);

                        // Datos de la solicitud
                        Console.Write("Ingrese descripción: ");
                        string descripcion = Console.ReadLine() ?? "";

                        decimal monto;
                        while (true)
                        {
                            Console.Write("Ingrese monto (Bs): ");
                            if (decimal.TryParse(Console.ReadLine(), out monto) && monto >= 0) break;
                            Console.WriteLine("⚠ Monto inválido. Ingrese un número mayor o igual a 0.");
                        }

                        DateTime fechaLimite = DateTime.Now;
                        // Admitimos años de 2 y 4 dígitos y varios formatos comunes
                        string[] formatos = { "dd/MM/yyyy", "d/M/yyyy", "dd/MM/yy", "d/M/yy", "yyyy-MM-dd" };
                        while (true)
                        {
                            Console.Write("Ingrese fecha límite (ej: 09/09/2025 o 09/09/25): ");
                            string f = Console.ReadLine()??"".Trim();
                            bool parsed = false;

                            if (!string.IsNullOrWhiteSpace(f))
                            {
                                parsed = DateTime.TryParseExact(f, formatos,
                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaLimite);

                                // si no se parsea con los formatos exactos, intentar TryParse (más permissivo)
                                if (!parsed)
                                    parsed = DateTime.TryParse(f, CultureInfo.CurrentCulture, DateTimeStyles.None, out fechaLimite);
                            }

                            if (!parsed)
                            {
                                Console.WriteLine("⚠ Fecha inválida. Formatos válidos: dd/MM/yyyy, dd/MM/yy, yyyy-MM-dd.");
                                continue;
                            }

                            // Comparamos solo la parte de fecha (sin hora)
                            if (fechaLimite.Date < DateTime.Now.Date)
                            {
                                Console.WriteLine("⚠ La fecha límite no puede ser anterior a hoy.");
                                continue;
                            }

                            break;
                        }

                        var nueva = gestor.CrearSolicitud(descripcion, monto, fechaLimite, usuario);
                        Console.WriteLine($"✔ Solicitud creada con éxito. ID asignado: {nueva.Id}");
                        break;

                    case 2:
                        Console.WriteLine("\n=== Solicitudes Registradas (Inorden) ===");
                        gestor.MostrarSolicitudes();
                        break;

                    case 3:
                        Console.Write("Ingrese ID de la Solicitud a buscar: ");
                        if (int.TryParse(Console.ReadLine(), out int buscarId))
                        {
                            var solicitud = gestor.BuscarSolicitud(buscarId);
                            Console.WriteLine(solicitud != null ? solicitud.ToString() : "⚠ Solicitud no encontrada.");
                        }
                        else Console.WriteLine("⚠ ID inválido.");
                        break;

                    case 4:
                        Console.Write("Ingrese ID de la Solicitud a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int eliminarId))
                        {
                            gestor.EliminarSolicitud(eliminarId);
                            Console.WriteLine("✔ Operación realizada.");
                        }
                        else Console.WriteLine("⚠ ID inválido.");
                        break;

                    case 5:
                        Console.WriteLine("\n=== Ordenar por ===");
                        Console.WriteLine("1. Monto");
                        Console.WriteLine("2. Fecha límite");
                        Console.WriteLine("3. Fecha de creación");
                        Console.Write("Seleccione: ");
                        if (int.TryParse(Console.ReadLine(), out int criterio))
                        {
                            gestor.MostrarSolicitudesOrdenadas(criterio);
                        }
                        else Console.WriteLine("⚠ Opción inválida.");
                        break;

                    case 6: // Postular
                        Console.Write("ID de la solicitud a postular: ");
                        if (!int.TryParse(Console.ReadLine(), out int idPostular)) { Console.WriteLine("⚠ ID inválido."); break; }
                        // pedir datos postulante
                        int pid;
                        while (true)
                        {
                            Console.Write("Ingrese su ID (CI): ");
                            if (int.TryParse(Console.ReadLine(), out pid)) break;
                            Console.WriteLine("⚠ ID inválido.");
                        }
                        string pnombre;
                        do
                        {
                            Console.Write("Ingrese su nombre: ");
                            pnombre = Console.ReadLine()??"".Trim();
                            if (string.IsNullOrWhiteSpace(pnombre)) Console.WriteLine("⚠ El nombre no puede estar vacío.");
                        } while (string.IsNullOrWhiteSpace(pnombre));
                        string ptelefono;
                        while (true)
                        {
                            Console.Write("Ingrese su teléfono (8 dígitos): ");
                            ptelefono = Console.ReadLine()??"".Trim();
                            if (ptelefono != null && Regex.IsMatch(ptelefono, @"^\d{8}$")) break;
                            Console.WriteLine("⚠ Teléfono inválido. Solo 8 dígitos numéricos.");
                        }
                        Usuario postulante = new Usuario(pid, pnombre, ptelefono);
                        gestor.PostularSolicitud(idPostular, postulante);
                        break;

                    case 7: // Aceptar postulacion
                        Console.Write("ID de la solicitud: ");
                        if (!int.TryParse(Console.ReadLine(), out int idAceptar)) { Console.WriteLine("⚠ ID inválido."); break; }
                        Console.Write("ID del postulante a aceptar: ");
                        if (!int.TryParse(Console.ReadLine(), out int idPostulanteAceptar)) { Console.WriteLine("⚠ ID inválido."); break; }
                        gestor.AceptarPostulacion(idAceptar, idPostulanteAceptar);
                        break;

                    case 8: // Cerrar solicitud
                        Console.Write("ID de la solicitud a cerrar: ");
                        if (int.TryParse(Console.ReadLine(), out int idCerrar))
                        {
                            gestor.CerrarSolicitud(idCerrar);
                        }
                        else Console.WriteLine("⚠ ID inválido.");
                        break;

                    case 9:
                        Console.WriteLine("\n=== Notificaciones (procesando cola) ===");
                        gestorNotificaciones.MostrarNotificaciones();
                        break;
                }

            } while (opcion != 10);

            Console.WriteLine("👋 Saliendo del sistema...");
        }
    }
}
