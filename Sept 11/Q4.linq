<Query Kind="Statements">
  <Connection>
    <ID>e911c88b-78d8-48b8-9b71-c4628c7c6afb</ID>
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

Products
.Where(x => x.ProductSubcategory.ProductCategory.ProductCategoryDescription == 
"Music, Movies and Audio Books")
.OrderBy(x => x.StyleName)
	.Select(x => new
	{
		Name = x.ProductName,
		Color = x.ColorName,
		ColorProcess = x.ColorName == "White" || x.ColorName == "Black" ? "No"
										: "Yes"
	})

	.Dump();