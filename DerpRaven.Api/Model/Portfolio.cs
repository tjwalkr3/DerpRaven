﻿namespace DerpRaven.Api.Model;

public class Portfolio
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Name { get; set; } = null!;

    public ProductType ProductType { get; set; } = null!;
    public List<ImageEntity> Images { get; set; } = [];
}
