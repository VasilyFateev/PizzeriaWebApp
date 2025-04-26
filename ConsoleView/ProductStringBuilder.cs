using ModelClasses;
using System.Text;

public class ProductStringBuilder
{
    private readonly StringBuilder str;
    public ProductStringBuilder(Product product)
    {
        str = new();
        str.AppendFormat($"{product.Id:d3} ||| {product.Name,-30} ||| ");
        var productItems = product.ProductItems.ToList();
        var variations = product.Category.Variations.ToList();
        for (int i = 0; i < variations.Count; i++)
        {
            var variation = variations[i];
            str.AppendFormat($"{variation.Name,6}");
            var variationOptions = variation.VariationOptions.ToList();
            for (int j = 0; j < variationOptions.Count; j++)
            {
                var option = variationOptions[j];
                var available = productItems
                    .Select(x => x.Configurations)
                    .Where(x =>
                        x.Where(i => i.VariationOptionId == option.Id).Any())
                    .Any();
                var availableSymbol = available ? "+" : "-";
                str.AppendFormat($"[{availableSymbol}]{option.Name,-5} ");
            }
            str.Append("| ");
        }
    }

    public override string ToString()
    {
        return str.ToString();
    }
}
