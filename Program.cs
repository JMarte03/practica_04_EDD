using System;
using System.Collections.Generic;
using System.Linq;

public class Producto
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }

    public Producto(string nombre, string descripcion, decimal precio)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Precio = precio;
    }

    public override string ToString()
    {
        return $"Nombre: {Nombre}, Descripción: {Descripcion}, Precio: {Precio:C}";
    }
}

public class Categoria
{
    public string Nombre { get; set; }
    public List<Producto> Productos { get; set; }
    public List<Categoria> Subcategorias { get; set; }

    public Categoria(string nombre)
    {
        Nombre = nombre;
        Productos = new List<Producto>();
        Subcategorias = new List<Categoria>();
    }

    public void AgregarProducto(Producto producto)
    {
        Productos.Add(producto);
    }

    public void AgregarSubcategoria(Categoria subcategoria)
    {
        Subcategorias.Add(subcategoria);
    }

    public IEnumerable<Producto> BuscarProductos(string nombre)
    {
        var resultado = Productos.Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));

        foreach (var subcategoria in Subcategorias)
        {
            resultado = resultado.Concat(subcategoria.BuscarProductos(nombre));
        }

        return resultado;
    }

    public override string ToString()
    {
        return Nombre;
    }
}

class Program
{
    static void Main()
    {
        var categorias = new List<Categoria>();

        while (true)
        {
            Console.WriteLine("1. Agregar Categoría");
            Console.WriteLine("2. Agregar Subcategoría");
            Console.WriteLine("3. Agregar Producto");
            Console.WriteLine("4. Buscar Productos");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");
            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarCategoria(categorias);
                    break;
                case "2":
                    AgregarSubcategoria(categorias);
                    break;
                case "3":
                    AgregarProducto(categorias);
                    break;
                case "4":
                    BuscarProductos(categorias);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    static void AgregarCategoria(List<Categoria> categorias)
    {
        Console.Write("Ingrese el nombre de la nueva categoría: ");
        var nombre = Console.ReadLine();
        categorias.Add(new Categoria(nombre));
    }

    static void AgregarSubcategoria(List<Categoria> categorias)
    {
        Console.Write("Ingrese el nombre de la categoría principal: ");
        var nombreCategoria = Console.ReadLine();
        var categoria = categorias.FirstOrDefault(c => c.Nombre.Equals(nombreCategoria, StringComparison.OrdinalIgnoreCase));

        if (categoria == null)
        {
            Console.WriteLine("Categoría no encontrada.");
            return;
        }

        Console.Write("Ingrese el nombre de la subcategoría: ");
        var nombreSubcategoria = Console.ReadLine();
        categoria.AgregarSubcategoria(new Categoria(nombreSubcategoria));
    }

    static void AgregarProducto(List<Categoria> categorias)
    {
        Console.Write("Ingrese el nombre de la categoría: ");
        var nombreCategoria = Console.ReadLine();
        var categoria = BuscarCategoriaRecursivamente(categorias, nombreCategoria);

        if (categoria == null)
        {
            Console.WriteLine("Categoría no encontrada.");
            return;
        }

        Console.Write("Ingrese el nombre del producto: ");
        var nombreProducto = Console.ReadLine();
        Console.Write("Ingrese la descripción del producto: ");
        var descripcionProducto = Console.ReadLine();
        Console.Write("Ingrese el precio del producto: ");
        var precioProducto = decimal.Parse(Console.ReadLine());

        categoria.AgregarProducto(new Producto(nombreProducto, descripcionProducto, precioProducto));
    }

    static void BuscarProductos(List<Categoria> categorias)
    {
        Console.Write("Ingrese el nombre del producto a buscar: ");
        var nombreProducto = Console.ReadLine();

        var resultados = new List<Producto>();

        foreach (var categoria in categorias)
        {
            resultados.AddRange(categoria.BuscarProductos(nombreProducto));
        }

        Console.WriteLine("Resultados de búsqueda:");
        if (!resultados.Any())
        {
            Console.WriteLine("No se encontraron productos.");
        }
        else
        {
            foreach (var producto in resultados)
            {
                Console.WriteLine(producto);
            }
        }
    }

    static Categoria BuscarCategoriaRecursivamente(List<Categoria> categorias, string nombre)
    {
        foreach (var categoria in categorias)
        {
            if (categoria.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
            {
                return categoria;
            }

            var subcategoria = BuscarCategoriaRecursivamente(categoria.Subcategorias, nombre);
            if (subcategoria != null)
            {
                return subcategoria;
            }
        }

        return null;
    }
}
