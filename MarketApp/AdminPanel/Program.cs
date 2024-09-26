using AdminPanel.StockControlPanel;
using MarketLibrary;
using MarketLibrary.Data;
using System;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
        Login:
            string adminUsername = "AdminFira";
            string adminPassword = "Admin1234";

            Console.Write("İstifadeci adi: ");
            string username = Console.ReadLine();

            Console.Write("Parol: ");
            string password = Console.ReadLine();

            if (username == adminUsername && password == adminPassword)
            {
                Console.WriteLine("Ugurlu girish!");
                AdminControlPanel(context);
            }
            else
            {
                Console.WriteLine("Yalnish istifadeci adi ve ya parol.");
                goto Login;
            }
        }
    }

    public static void AdminControlPanel(AppDbContext context)
    {
        var categoryManager = new CategoryManager(context);
        var productManager = new ProductManager(context);

        while (true)
        {
            Console.WriteLine("1. Kateqoriya elave et\n2. mehsul elave et\n3. Mehsulu yenile\n4. Mehsullari goruntule\n5. chixish");
            Console.WriteLine("sechim edin:");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCategory(categoryManager);
                    break;
                case "2":
                    AddProduct(productManager, categoryManager);
                    break;
                case "3":
                    UpdateProductDetails(productManager);
                    break;
                case "4":
                    ViewProducts(productManager);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Yalnish sechim.");
                    break;
            }
        }
    }

    static void AddCategory(CategoryManager categoryManager)
    {
        Console.WriteLine("Kateqoriya adi daxil edin: ");
        var name = Console.ReadLine();
        categoryManager.AddCategory(name);
    }

    static void AddProduct(ProductManager productManager, CategoryManager categoryManager)
    {
        Console.WriteLine("Mehsulun adini daxil edin: ");
        var name = Console.ReadLine();
        Console.WriteLine("Mehsul təsvirini daxil edin: ");
        var description = Console.ReadLine();
        Console.WriteLine("Mehsul qiymetini daxil edin: ");
        var price = double.Parse(Console.ReadLine());
        Console.WriteLine("Mehsul sayini daxil edin: ");
        var quantity = int.Parse(Console.ReadLine());

        var categories = categoryManager.GetCategories();
        Console.WriteLine("Kateqoriya sechin:");
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}. {category.Name}");
        }

        var categoryId = int.Parse(Console.ReadLine());
        productManager.AddProduct(name, description, categoryId, price, quantity);
    }

    static void UpdateProductDetails(ProductManager productManager)
    {
        Console.WriteLine("Mehsul adini daxil edin: ");
        var name = Console.ReadLine();
        var product = productManager.GetProducts().FirstOrDefault(p => p.Name == name);

        if (product != null)
        {
            Console.WriteLine($"Mehsul adi: {product.Name}\nTesvir: {product.Description}\nQiymet: {product.Price}\nSayi: {product.Quantity}");
            Console.WriteLine("Deyishmek istediyiniz xususiyyeti sechin:\n1. Ad\n2. Tesvir\n3. Qiymet\n4. Say");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Yeni ad daxil edin: ");
                    var newName = Console.ReadLine();
                    productManager.UpdateProductName(product.Name, newName);
                    break;
                case "2":
                    Console.WriteLine("Yeni tesvir daxil edin: ");
                    var newDescription = Console.ReadLine();
                    productManager.UpdateProductDescription(product.Name, newDescription);
                    break;
                case "3":
                    Console.WriteLine("Yeni qiymet daxil edin: ");
                    var newPrice = double.Parse(Console.ReadLine());
                    productManager.UpdateProductPrice(product.Name, newPrice);
                    break;
                case "4":
                    Console.WriteLine("Yeni say daxil edin: ");
                    var newQuantity = int.Parse(Console.ReadLine());
                    productManager.UpdateProductQuantity(product.Name, newQuantity);
                    break;
                default:
                    Console.WriteLine("Yanlish sechim.");
                    break;
            }
        }
    }

    static void ViewProducts(ProductManager productManager)
    {
        var products = productManager.GetProducts();
        foreach (var product in products)
        {
            Console.WriteLine($"Mehsul: {product.Name}, Qiymet: {product.Price}, Say: {product.Quantity}");
        }
    }
}
