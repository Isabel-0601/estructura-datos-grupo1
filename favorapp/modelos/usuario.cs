namespace FavorApp.modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }


        public Usuario (int id, string nombre, string telefono)
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
        }

        public override string ToString()
        {
            return $"[Usuario: {Id}] {Nombre} - Tel: {Telefono}";
        }
    }
}