using AssortmentEditService.DatabaseControllers;
using DatabaseAccess;
using System.Text;

namespace ConsoleView.Commands.AssortmentEditor
{
    internal class СсShowAssortmentList : IConsoleCommand
    {
        public string ConsoleGroupName => "Assortment Editor List";
        public string Name => "showAssortmentList";
        public string Description => "[INFO] Syntax: showAssortmentList\nDisplay the entire assortment of the pizzeria";

        public async Task<string> Execute(params string[] _)
        {
            AssortementDataContext db = new();
            if (db.Database.CanConnect())
            {
                using (db = new())
                {
                    var controllers = new DatabaseController(db);
                    var categories = await controllers.GetLinkedList();
                    var result = new StringBuilder();
                    for (int i = 0; i < categories.Count; i++)
                    {
                        var category = categories[i];
                        result.Append(category.Name);
                        result.AppendLine();
                        var products = category.Products.ToList();
                        for (var j = 0; j < products.Count; j++)
                        {
                            result.Append(new ProductStringBuilder(products[j]));
                            result.AppendLine();
                        }
                        result.AppendLine();
                    }
                    return result.ToString();
                }
            }
            else
            {
                return $"[ERROR] Database connection error";
            }
        }
    }
}
