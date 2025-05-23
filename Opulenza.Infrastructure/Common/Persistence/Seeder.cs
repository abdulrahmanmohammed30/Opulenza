﻿using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Common.Persistence;

public class Seeder
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public List<ApplicationUser> Users { get; set; } = new();
}