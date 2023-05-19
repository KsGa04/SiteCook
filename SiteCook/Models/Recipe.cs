using System;
using System.Collections.Generic;

namespace SiteCook;

public partial class Recipe
{
    public int IdRecipe { get; set; }

    public byte[]? ImageRecipe { get; set; }

    public string NameRecipe { get; set; } = null!;

    public string Ingredient { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Moder { get; set; }

    public int? IdUser { get; set; }

    public int? IdCategory { get; set; }

    public int? IdMeal { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Meal? IdMealNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
