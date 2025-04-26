using DatabaseAccess;
using AdminApp.DatabaseControllers;
using ModelClasses;


string str;

using (AssortementSetupApplicationContext db = new())
{
    bool isAvalaible = db.Database.CanConnect();

    if (isAvalaible)
    {
        var controllers = new ProductRepository(db);
        var categories = await controllers.GetLinkedList();
        AssortimentDatabaseChanges changes = new();
        Console.WriteLine("The database is available");
        Console.WriteLine("COMANNDS\nOpenList\nOpenProduct [id]\n===========================================\n");
        while (true)
        {
            str = Console.ReadLine();
            var words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = words[0];
            Console.WriteLine();
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
                        GetProductInfo(categories, productId);
                    break;
                case "ProductEditName":
                    if (words.Length > 2 && int.TryParse(words[1], out productId))
                    {
                        var productEditor = new ProductEditor(categories, productId, changes.ProductChanges ??= new());
                        productEditor.ProductNameEdit(string.Concat(words.Skip(2)));
                    }
                    break;
                case "ProductEditDesc":
                    if (words.Length > 2 && int.TryParse(words[1], out productId))
                    {
                        var productEditor = new ProductEditor(categories, productId, changes.ProductChanges ??= new());
                        productEditor.ProductDescriptionEdit(string.Concat(words.Skip(2)));
                    }
                    break;
                case "RemoveProduct":
                    if (words.Length > 1 && int.TryParse(words[1], out productId))
                    {
                        var productEditor = new ProductEditor(categories, productId, changes.ProductChanges ??= new());
                        productEditor.RemoveProduct();
                    }
                    break;
                case "AddProduct":
                    if (words.Length > 3 && int.TryParse(words[1], out productId))
                    {
                        var productEditor = new ProductEditor(categories, productId, changes.ProductChanges ??= new());
                        productEditor.AddProduct(words[2], string.Concat(words.Skip(3)), "N/A");
                    }
                    break;
                case "SaveChanges":
                    {
                        await controllers.UpdateAssortimentData(changes); 
                        controllers = new ProductRepository(db);
                        categories = await controllers.GetLinkedList();
                        Console.WriteLine("Changes has been saved");
                    }
                    break; 
            }
        }
    }
    else
    {
        Console.WriteLine("The database isn't available");
        return;
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
    .FirstOrDefault(p => p.Id == productId);

    if (product == null)
    {
        Console.WriteLine("The product was not found under this ID. Check the correctness of the entered ID.");
        return;
    }
    Console.WriteLine(new ProductInfoBuilder(product));
}


