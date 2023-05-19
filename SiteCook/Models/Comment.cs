using System;
using System.Collections.Generic;

namespace SiteCook;

public partial class Comment
{
    public int IdComment { get; set; }

    public string NameComment { get; set; } = null!;

    public DateTime DateComement { get; set; }

    public int IdUser { get; set; }

    public int IdRecipe { get; set; }

    public virtual Recipe IdRecipeNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
