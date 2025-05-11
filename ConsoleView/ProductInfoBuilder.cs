using ProductModelClasses;
using System.Text;

namespace ConsoleView
{
    public class ProductInfoBuilder
    {
        private readonly StringBuilder str;
        public ProductInfoBuilder(Product product)
        {
            str = new();
            str.AppendLine($"{product.Id:d3} ||| {product.Name,-30}\n{product.Description}");
            var productItems = product.ProductItems.ToList();
            var variations = product.Category.Variations.ToList();
            str.AppendLine("Configurations");
            str.Append($" ID |");

            for (int i = 0; i < variations.Count; i++)
            {
                var variation = variations[i];
                str.Append($" {variation.Name,-10} |");
            }
            str.AppendLine($"{" Price",-5} ");

            for (int i = 0; i < productItems.Count; i++)
            {
                var item = productItems[i];
                str.Append($"{item.Id:d3} |");
                for (int j = 0; j < variations.Count; j++)
                {
                    var variation = variations[j];
                    var variationOptions = variation.VariationOptions.ToList();
                    var option = variationOptions.FirstOrDefault(var =>
                    {
                        return var.Configurations.Where(conf => conf.ProductItemId == item.Id).Any();
                    });
                    if (option != null)
                    {
                        str.Append($" {option.Name,-10} |");
                    }
                }
                str.AppendLine($" {item.Price,-5}");
            }
        }

        public override string ToString()
        {
            return str.ToString();
        }
    }
}