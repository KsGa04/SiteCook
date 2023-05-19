using System;
using System.Collections.Generic;

namespace SiteCook;

public partial class Category
{
    public int IdCategory { get; set; }

    public string NameCategory { get; set; } = null!;

    public byte[]? ImageCategory { get; set; }

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public virtual ICollection<Moderator> Moderators { get; set; } = new List<Moderator>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
