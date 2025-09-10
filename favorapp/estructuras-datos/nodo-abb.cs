using FavorApp.modelos;

namespace FavorApp.estructuras_datos
{
    public class NodoABB
    {
        public Solicitud Dato { get; set; }
        public NodoABB? Izquierda { get; set; }
        public NodoABB? Derecha { get; set; }

        public NodoABB(Solicitud dato)
        {
            Dato = dato;
            Izquierda = null;
            Derecha = null;
        }
    }
}
