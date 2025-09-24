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
.GroupBy(psc => psc.ProductCategory.ProductCategoryName)
.Select(g => new
{
	CategoryName = g.Key,
	ProductSubcategories = g.Select(x => new
	{
		SubCategoryName = x.ProductSubcategoryName
	})
	.OrderBy(x => x.SubCategoryName)
	.ToList()
})
	.OrderBy(g => g.CategoryName)
	.ToList()
.Dump();

ProductCategories
	.GroupBy(pc => pc.ProductCategoryName)
.Select(g => new
{
	CategoryName = g.Key,
	ProductSubcategories = ProductSubcategories
			.Where(psc => psc.ProductCategory.ProductCategoryName == g.Key)
	.Select(psc => new
	{
		SubCategoryName = psc.ProductSubcategoryName
	})
	.OrderBy(x => x.SubCategoryName)
	.ToList()
})
	.OrderBy(g => g.CategoryName)
	.ToList()
.Dump();
