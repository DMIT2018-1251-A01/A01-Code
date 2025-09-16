<Query Kind="Statements">
  <Connection>
    <ID>63f7df99-61e5-497e-b9e3-230360397903</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Contoso</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

ProductSubcategories
	//.Where(p => p.Products.Count() > 0)
	.Select(p => new
	{
		Category = p.ProductCategory.ProductCategoryName,
		SubCategory = p.ProductSubcategoryName,
		LowestCost = p.Products.Min(p => p.UnitCost),
		LowestPrice = p.Products.Min(p => p.UnitPrice)
	})
	.Where(p => p.LowestCost != null || p.LowestPrice != null)
	.OrderBy(p => p.Category)
	.ThenBy(p => p.SubCategory)
	.Dump();