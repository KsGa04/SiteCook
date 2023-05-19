using System;
using System.Collections.Generic;

namespace SiteCook;

public partial class Meal
{
    public int IdMeal { get; set; }

    public byte[]? ImageMeal { get; set; }

    public string NameMeal { get; set; } = null!;

    public string? DescriptionMeal { get; set; }

    public int IdCategory { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
