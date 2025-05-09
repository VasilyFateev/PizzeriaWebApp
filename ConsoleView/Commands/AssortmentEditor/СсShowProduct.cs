using AssortmentEditService.CustomExceptions;
using AssortmentEditService.DatabaseControllers;
using DatabaseAccess;
using ModelClasses;

namespace ConsoleView.Commands.AssortmentEditor
{
	internal class СсShowProduct : IConsoleCommand
    {
        public string ConsoleGroupName => "Assortment Editor";
        public string Name => "showProduct";
        public string Description => "[INFO] Syntax: showProduct [productId:int]\nDisplay product data under the index [id]";

        public async Task<string> Execute(params string[] args)
        {
            AssortementDataContext db = new();
            if (db.Database.CanConnect())
            {
                using (db = new())
                {
                    var controllers = new DatabaseController(db);
                    var categories = await controllers.GetLinkedList();
                    if (args.Length > 0 && int.TryParse(args[0], out var productId))
                    {
                        var product = categories
                        .SelectMany(c => c.Products)
                        .FirstOrDefault(p => p.Id == productId)
                        ?? throw new NotFoundEntityByKeyException(productId, typeof(Product));
                        return new ProductInfoBuilder(product).ToString();
                    }
                    else
                    {
                        return $"[ERROR] Incorrect argument.";
                    }
                }
            }
            else
            {
                return $"[ERROR] Database connection error";
            }
        }
    }
}
