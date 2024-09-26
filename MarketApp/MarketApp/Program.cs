using UserPanel.UserControlPanel;
using UserPanel.Services;
using MarketLibrary;
using MarketLibrary.Data;
using System;

public class Program
{
     static User _loggedInUser;

    public static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
            while (_loggedInUser == null)
            {
                MainPage(context);
            }
            UserControlPanel(context);
        }
    }

    public static void MainPage(AppDbContext context)
    {
        Console.Clear();
        Console.WriteLine("1. Qeydiyyat");
        Console.WriteLine("2. Daxil ol");

        var choice = Console.ReadLine();
        var userManager = new UserManager(context);

        switch (choice)
        {
            case "1":
                RegisterPage(userManager);
                break;
            case "2":
                LoginPage(userManager);
                break;
        }
    }

    public static void RegisterPage (UserManager userManager)
    {
        Console.Clear();
        Console.WriteLine("Qeydiyyat");

        Console.Write("Ad: ");
        var name = Console.ReadLine();
        Console.Write("Soyad: ");
        var surname = Console.ReadLine();
        Console.Write("Dogum tarixi (dd.MM.yyyy): ");
        var date = DateTime.ParseExact(Console.ReadLine()!, "dd.MM.yyyy", null);
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Shifre: ");
        var password = Console.ReadLine();

        try
        {
            userManager.Register(name, surname, email, password, date);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void LoginPage (UserManager userManager)
    {
        Console.Clear();
        Console.WriteLine("Daxil ol");

        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Shifre: ");
        var password = Console.ReadLine();

        try
        {
            _loggedInUser = userManager.Login(email!, password!);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void UserControlPanel(AppDbContext context)
    {
        var categoryManager = new CategoryManager(context);
        var cartManager = new CartManager(context, _loggedInUser);
        var productManager = new ProductManager(context);

        while (true)
        {
            Console.WriteLine("1. Kateqoriyalar");
            Console.WriteLine("2. Sebet");
            Console.WriteLine("3. Profil");
            Console.WriteLine("4. Chixish");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowCategories(categoryManager, cartManager,productManager);
                    break;
                case "2":
                    ViewCart(cartManager,productManager);
                    break;
                case "3":
                    ShowProfile(context);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Yanlish sechim.");
                    break;
            }
        }
    }

    public static void ShowCategories(CategoryManager categoryManager, CartManager cartManager,ProductManager productManager)
    {
        Console.WriteLine("Kateqoriyalar:");
        var categories = categoryManager.GetCategories();

        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}. {category.Name}");
        }

        Console.WriteLine("Kateqoriyani sechin: ");
        var categoryId = int.Parse(Console.ReadLine());
        var products = productManager.GetProductsByCategory(categoryId);
        if (products != null)
        {
            foreach (var product in products)
            {
                if (product.Quantity > 0)
                {
                    Console.WriteLine($"Mehsul: {product.Name}, Qiymet: {product.Price}, Say: {product.Quantity}");
                }
            }

            Console.WriteLine("Sebete elave etmek istediyiniz mehsulu sechin:");
            var productName = Console.ReadLine();
            Console.WriteLine("Sayi daxil edin:");
            var quantity = int.Parse(Console.ReadLine());

            var selectedProduct = products.FirstOrDefault(p => p.Name == productName);
            if (selectedProduct != null)
            {
                cartManager.AddToCart(selectedProduct, quantity);
            }
        }
        else
        {
            Console.WriteLine("Yanlish kateqoriya.");
        }
    }

    public static void ViewCart(CartManager cartManager,ProductManager productManager)
    {
        Console.WriteLine("Sebet:");
        var cartItems = cartManager.GetCartItems();
        foreach (var cartItem in cartItems)
        {
            Console.WriteLine($"Mehsul: {cartItem.Product.Name}, Say: {cartItem.Quantity}");
        }

        double total = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
        Console.WriteLine($"Umumi mebleg: {total}");

        Console.WriteLine("1. Mehsulu sil");
        Console.WriteLine("2. Odenish et");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Sileceyiniz mehsulun adini daxil edin:");
                var productName = Console.ReadLine();
                var product = cartItems.FirstOrDefault(c => c.Product.Name == productName)?.Product;
                if (product != null)
                {
                    cartManager.RemoveProduct(product.ProductId);
                }
                break;
            case "2":
                Console.Write("Odenish meblegini daxil edin: ");
                var payment = double.Parse(Console.ReadLine());
                var change = cartManager.Checkout(payment);
                Console.WriteLine($"Qalig: {change}");
                var cart = cartManager.GetCartItems();
                foreach (var item in cart)
                {
                    productManager.UpdateStock(item.ProductId, item.Quantity);
                }
                break;
        }
    }

    public static void ShowProfile(AppDbContext context)
    {
        Console.WriteLine($"Ad: {_loggedInUser.Name}");
        Console.WriteLine($"Soyad: {_loggedInUser.Surname}");
        Console.WriteLine($"Email: {_loggedInUser.Email}");
    }
}
