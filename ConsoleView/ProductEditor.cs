using AdminApp.DatabaseControllers;
using ModelClasses;

public class ProductEditor
{
    private readonly Product? product;
    private readonly TableChanges<Product> changes;
    private readonly List<ProductCategory> categories;
    public ProductEditor(List<ProductCategory> categories, int productId, TableChanges<Product> changes)
    {
        product = categories.SelectMany(c => c.Products).FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("The product was not found under this ID. Check the correctness of the entered ID.");
        }
        this.changes = changes;
        changes ??= new();
        this.categories = categories;
    }
    public bool ProductNameEdit(string newValue)
    {
        if (product == null)
        {
            Console.WriteLine("The product was not found under this ID. Check the correctness of the entered ID.");
            return false;
        }

        if (categories.SelectMany(c => c.Products).Any(p => p.Name == newValue))
        {
            Console.WriteLine("A unique product name is required.");
            return false;
        }

        product.Name = newValue;
        changes.ToUpdate ??= [];
        changes.ToUpdate.Add(product);
        return true;
    }

    public bool ProductDescriptionEdit(string newValue)
    {
        if (product == null)
        {
            Console.WriteLine("The product was not found under this ID. Check the correctness of the entered ID.");
            return false;
        }

        product.Description = newValue;
        changes.ToUpdate ??= [];
        changes.ToUpdate.Add(product);
        return true;
    }
    public void AddProduct(string categoryName, string name, string imageLink, string description = "")
    {
        var category = categories.FirstOrDefault(category => category.Name == categoryName);
        if (category == null)
        {
            return;
        }
        if (categories.SelectMany(c => c.Products).Any(p => p.Name == name))
        {
            Console.WriteLine("A unique product name is required.");
            return;
        }
        changes.ToAdd ??= [];
        Product newProduct = new() { CategoryId = category.Id, Name = name, ImageLink = imageLink, Description = description };
        changes.ToAdd.Add(newProduct);
    }

    public void RemoveProduct()
    {
        if (product != null)
        {
            changes.ToRemove ??= [];
            changes.ToRemove.Add(product);
        }
    }
}


