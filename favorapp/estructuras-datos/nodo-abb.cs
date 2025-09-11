using FavorApp.modelos;

namespace FavorApp.estructuras_datos
{
    public class NodoABB
    {
        public Solicitud Dato { get; set; }
        public NodoABB? Izquierda { get; set; }
        public NodoABB? Derecha { get; set; }

        // Constructor que inicializa el nodo con un dato y sin hijos
        public NodoABB(Solicitud dato)
        {
            Dato = dato;
            Izquierda = null; // Inicialmente no tiene hijo izquierdo
            Derecha = null; // Inicialmente no tiene hijo derecho
        }
    }
}
