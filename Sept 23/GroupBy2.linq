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

InvoiceLines
	.GroupBy(il => new
	{
		il.Product.ProductSubcategory.ProductCategory.ProductCategoryName,
		il.Product.ProductSubcategory.ProductSubcategoryName
	})
	.Select(g => new
	{
		CategoryName = g.Key.ProductCategoryName,
		SubcategoryName = g.Key.ProductSubcategoryName,
		Invoices = g.Select(il => new
		{
			InvoiceID = il.InvoiceID,
			Product = il.Product.ProductName,
			Amount = il.SalesQuantity
		})
		.OrderBy(il => il.Product)
		.ToList()
	})
			.OrderBy(g => g.CategoryName)
			.ThenBy(g => g.SubcategoryName)
		.ToList()
		.Dump();

InvoiceLines
	.GroupBy(il => new
	{
		il.Product.ProductSubcategory.ProductCategory.ProductCategoryID,
		il.Product.ProductSubcategory.ProductSubcategoryID
	})
	.Select(g => new
	{
		CategoryName = ProductCategories.Where(x => x.ProductCategoryID == g.Key.ProductCategoryID)
							.Select(x => x.ProductCategoryName).FirstOrDefault(),
		SubcategoryName = ProductSubcategories.Where(x => x.ProductSubcategoryID == g.Key.ProductSubcategoryID)
							.Select(x => x.ProductSubcategoryName).FirstOrDefault(),
		Invoices = g.Select(il => new
		{
			InvoiceID = il.InvoiceID,
			Product = il.Product.ProductName,
			Amount = il.SalesQuantity
		})
		.OrderBy(il => il.Product)
		.ToList()
	})
			.OrderBy(g => g.CategoryName)
			.ThenBy(g => g.SubcategoryName)
		.ToList()
		.Dump();

