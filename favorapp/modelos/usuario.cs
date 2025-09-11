namespace FavorApp.modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        //  Constructor vacío (para usar { })
        public Usuario() { }

        //  Constructor con parámetros (para crear directamente)
        public Usuario(int id, string nombre, string telefono)
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
        }

        public override string ToString()
        {
            return $"{Nombre} (CI {Id}, Tel: {Telefono})";
        }
    }
}