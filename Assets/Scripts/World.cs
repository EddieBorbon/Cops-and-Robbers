using UnityEngine;

public sealed class World
{
    private static readonly World instance = new World();
    private GameObject[] hidingSpots;

    // Constructor privado para evitar la instanciación externa
    private World()
    {
        // Busca los hiding spots al inicializar la instancia
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    // Propiedad para acceder a la instancia singleton
    public static World Instance
    {
        get { return instance; }
    }

    // Método para obtener los hiding spots
    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }

    // Método para actualizar los hiding spots (opcional)
    public void UpdateHidingSpots()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }
}