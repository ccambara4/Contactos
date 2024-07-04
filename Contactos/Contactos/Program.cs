using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Contactos
{
    class Program
    {
        
    }
}

[Serializable]
public class Contacto //En esta clase declaramos las variables que utilizaremos en nuestra aplicación: ID, Nombre y Apellido, Telefono, Email 
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }

    public override string ToString()//Requisito 1
    {
        return $"ID: {Id}, Nombre y Apellido: {Nombre}, Teléfono: {Telefono}, Email: {Email}";//Requisito 1
    }
}


public class GestorDeContactos//Llamamos a las clases creadas en la aplicacion para que realicen su función
{
    private List<Contacto>/*Requisito 2*/ contactos;
    private const string archivoContactos = "Contactos.dat";//Creamos y llamamos al arcvhivo en donde se van a guardar los datos ingresados en la consola

    public GestorDeContactos()
    {
        contactos = new List<Contacto>();
        CargarContactos();
    }

    public void AgregarContacto(Contacto contacto)//Método para agregar contacto
    {
        contactos.Add(contacto);
        GuardarContactos();
    }

    public List<Contacto> BuscarContactos(string criterio)//Método para buscar contacto
    {
        return contactos.FindAll(c => c.Nombre.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                                       c.Telefono.Contains(criterio));
    }

    public List<Contacto> ListarContactos()//Método para mostrar lista de contaztos
    {
        return contactos;
    }

    public void EliminarContacto(int id)//Método para eliminar contacto por ID
    {
        var contacto = contactos.Find(c => c.Id == id);
        if (contacto != null)
        {
            contactos.Remove(contacto);
            GuardarContactos();
            Console.WriteLine("Contacto eliminado.");
        }
        else
        {
            Console.WriteLine("Error: No se encontró un contacto con ese ID o no existe.");
        }

    }

    private void GuardarContactos()//Método para guardar contacto
    {
        using (FileStream fs = new FileStream(archivoContactos, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, contactos);
        }
    }

    private void CargarContactos()//Método para buscar contacto por nombre o telefono
    {
        if (File.Exists(archivoContactos))
        {
            using (FileStream fs = new FileStream(archivoContactos, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                contactos = (List<Contacto>)formatter.Deserialize(fs);
            }
        }
    }
}

class Program //Creamos el menu de el gestor de contactos y tambiém creamos la función de cada opcion
{
    static void Main(string[] args)
    {
        GestorDeContactos gestor = new GestorDeContactos();
        while (true)//Creamos el menu para que se muestre en la consola al iniciarla
        {
            Console.Clear();
            //Menu
            Console.WriteLine("Bienvenido!!!");
            Console.WriteLine("---------------");
            Console.WriteLine("Menu de Opciones:");
            Console.WriteLine("");
            Console.WriteLine("1. Agregar Contacto");
            Console.WriteLine("2. Buscar Contacto");
            Console.WriteLine("3. Listar Contactos");
            Console.WriteLine("4. Eliminar Contacto");
            Console.WriteLine("5. Guardar contactos");
            Console.WriteLine("6. Cargar contactos");
            Console.WriteLine("7. Salir del menú");
            Console.WriteLine("");

            Console.Write("Seleccione una opción: ");//Creamos un switch para que el usuario pueda escoger la opción que desee realizar
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarContacto(gestor);
                    break;
                case "2":
                    BuscarContacto(gestor);
                    break;
                case "3":
                    ListarContactos(gestor);
                    break;
                case "4":
                    EliminarContacto(gestor);
                    break;
                case "5":
                    GuardarContactos(gestor);
                    break;
                case "6":
                    CargarContactos(gestor);
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Opción no válida o no existe en el menú.");//Agregamos esto por si el usuario coloca una opción inexistente o inválida
                    break;
            }
        }
    }

    static void AgregarContacto(GestorDeContactos gestor)//Creamos la función que tendrá la opcion de agregar el contacto con sus datos requeridos
    {
        Console.Clear();
        Console.WriteLine("Agregar Contacto");
        Contacto contacto = new Contacto();
        contacto.Id = new Random().Next(1, 100000);  //Se genera un ID único, para simular un escenario más real o como si utilizaramos una base de datos
        Console.Write("Nombre y Apellido: ");
        contacto.Nombre = Console.ReadLine();
        Console.Write("Teléfono: ");
        contacto.Telefono = Console.ReadLine();
        Console.Write("Email: ");
        contacto.Email = Console.ReadLine();
        gestor.AgregarContacto(contacto);
        Console.WriteLine("");
        Console.WriteLine("Contacto agregado correctamente. Presione cualquier tecla para regresar al menu principal...");
        Console.ReadKey();
    }

    static void BuscarContacto(GestorDeContactos gestor)//Creamos esta función para buscar un contacto con nombre o numero de telefono, también se podría usar el ID para ser más específico; como en la función de eliminar, pero así es como se pidió
    {
        Console.Clear();
        Console.WriteLine("Buscar Contacto");
        Console.Write("Ingrese nombre o teléfono: ");
        string criterio = Console.ReadLine();
        var resultados = gestor.BuscarContactos(criterio);
        if (resultados.Count > 0)
        {
            foreach (var contacto in resultados)
            {
                Console.WriteLine(contacto);
            }
        }
        else
        {
            Console.WriteLine("No se encontraron contactos o no existe.");
        }
        Console.WriteLine("");
        Console.WriteLine("Presione cualquier tecla para regresar al menu principal...");
        Console.ReadKey();
    }

    static void ListarContactos(GestorDeContactos gestor)//Mostramos todos los contactos ingresados en la aplicación
    {
        Console.Clear();
        Console.WriteLine("Lista de Contactos");
        var contactos = gestor.ListarContactos();
        if (contactos.Count > 0)
        {
            foreach (var contacto in contactos)
            {
                Console.WriteLine(contacto);
            }
        }
        else
        {
            Console.WriteLine("No hay contactos.");
        }
        Console.WriteLine("");
        Console.WriteLine("Presione cualquier tecla para regresar al menu principal...");
        Console.ReadKey();
    }

    static void EliminarContacto(GestorDeContactos gestor)//Eliminamos el contacto con su ID para ser más específico y también mostramos la lista de los contactos existentes para que el usuario sepa cual es el ID a eliminar y no tenga que regresar a la opción de Listar Contactos
    {
        Console.Clear();
        Console.WriteLine("Lista de Contactos");
        var contactos = gestor.ListarContactos();
        if (contactos.Count > 0)
        {
            foreach (var contacto in contactos)
            {
                Console.WriteLine(contacto);
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Eliminar Contacto");
            Console.Write("Ingrese el ID del contacto a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                gestor.EliminarContacto(id);
            }
            else
            {
                Console.WriteLine("Error: ID inválido.");
            }
        }
        else
        {
            Console.WriteLine("No hay contactos.");
        }

        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

     static void GuardarContactos(GestorDeContactos gestor)//Guardamos los contactos que ingresamos
    {
        Console.Clear();
        Console.WriteLine("Contactos guardados. Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    static void CargarContactos(GestorDeContactos gestor)//Cargamos los contactos del archivo Contactos.dat
    {
        Console.Clear();
        Console.WriteLine("Contactos cargados. Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}

