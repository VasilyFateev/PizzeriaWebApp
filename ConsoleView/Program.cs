using DatabaseAccess;
using AdminApp.DatabaseControllers;
using ModelClasses;
using AdminApp.ViewController;
using AdminApp.CustomExceptions;

using (AssortementSetupApplicationContext db = new())
{
    bool isAvalaible = db.Database.CanConnect();

    if (isAvalaible)
    {
        var controllers = new DatabaseController(db);
        var categories = await controllers.GetLinkedList();
        AssortimentDatabaseChanges changes = new();
        SendWarnningMessage("The database is available");
        while (true)
        {
            var str = Console.ReadLine();
            if(str == null)
            {
                continue;
            }
            var words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = words[0];
            Console.WriteLine();
            try
            {
                switch (command)
                {
                    case "Q":
                        return;
                    case "Exit":
                        return;
                    case "OpenList":
                        GetAllProductList(categories);
                        break;
                    case "OpenProduct":
                        if (words.Length > 1 && int.TryParse(words[1], out var productId))
                        {
                            GetProductInfo(categories, productId);
                        }
                        break;
                    case "ProductEditName":
                        if (words.Length > 2 && int.TryParse(words[1], out productId))
                        {
                            try
                            {
                                var productEditor = new ProductEditorController(categories, productId, changes.ProductChanges ??= new());
                                productEditor.NameEdit(string.Concat(words.Skip(2)));
                            }
                            catch (NonUniqueNameException ex)
                            {
                                SendErrorMessage(ex.Message);
                            }
                        }
                        break;
                    case "ProductEditDesc":
                        if (words.Length > 2 && int.TryParse(words[1], out productId))
                        {
                            var productEditor = new ProductEditorController(categories, productId, changes.ProductChanges ??= new());
                            productEditor.DescriptionEdit(string.Concat(words.Skip(2)));
                        }
                        break;
                    case "RemoveProduct":
                        if (words.Length > 1 && int.TryParse(words[1], out productId))
                        {
                            var productEditor = new ProductEditorController(categories, productId, changes.ProductChanges ??= new());
                            productEditor.Remove();
                        }
                        break;
                    case "AddProduct":
                        if (words.Length > 2 && int.TryParse(words[1], out var categoryId))
                        {
                            try
                            {
                                var productEditor = new ProductEditorController(categories, 0, changes.ProductChanges ??= new());
                                productEditor.Add(categoryId, string.Concat(words.Skip(2)), "N/A");
                            }
                            catch (NonUniqueNameException ex)
                            {
                                SendErrorMessage(ex.Message);
                            }
                        }
                        break;
                    case "SaveChanges":
                        {
                            await controllers.UpdateAssortimentData(changes);
                            controllers = new DatabaseController(db);
                            categories = await controllers.GetLinkedList();
                            SendDoneMessage("Changes has been saved");
                        }
                        break;
                    default:
                        SendWarnningMessage("Unknown command");
                        break;
                }
            }
            catch (NotFoundEntityByKeyException ex)
            {
                SendErrorMessage(ex.Message);
            }
        }
    }
    else
    {
        Console.WriteLine("The database isn't available");
        return;
    }

    void SendErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    void SendWarnningMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    void SendDoneMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}












static void GetAllProductList(List<ProductCategory> categories)
{
    for (int i = 0; i < categories.Count; i++)
    {
        var category = categories[i];
        Console.WriteLine(category.Name);
        var products = category.Products.ToList();
        for (var j = 0; j < products.Count; j++)
        {
            Console.WriteLine(new ProductStringBuilder(products[j]));
        }
        Console.WriteLine();
    }
}

static void GetProductInfo(List<ProductCategory> categories, int productId)
{
    var product = categories
    .SelectMany(c => c.Products)
    .FirstOrDefault(p => p.Id == productId)
    ?? throw new NotFoundEntityByKeyException(productId, typeof(Product));

    Console.WriteLine(new ProductInfoBuilder(product));
}


