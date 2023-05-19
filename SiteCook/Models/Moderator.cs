using System;
using System.Collections.Generic;

namespace SiteCook;

public partial class Moderator
{
    public int IdModerator { get; set; }

    public string Mail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? NikName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }
}
