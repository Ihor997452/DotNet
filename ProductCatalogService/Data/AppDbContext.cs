﻿using DotNet.ProductCatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet.ProductCatalogService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product>? Products { get; set; }
}