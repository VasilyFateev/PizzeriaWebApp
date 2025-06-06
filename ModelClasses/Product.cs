﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductModelClasses
{
    [Table("product")]
    public class Product : Interfaces.IHasPrimaryKey, Interfaces.IHasUniqueName
	{
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("product_category_id")]
        public int CategoryId { get; set; } = default!;
        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("description")]
        public string? Description { get; set; }
        [Column("image_link")]
        public string ImageLink { get; set; } = null!;
        public ProductCategory Category { get; set; } = null!;
        public ICollection<ProductItem> ProductItems { get; set; } = null!;
        public int GetPrimaryKey() => Id;
        public string GetUniqueName() => Name;
	}

}
