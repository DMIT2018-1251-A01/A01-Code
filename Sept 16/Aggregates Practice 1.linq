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

Products
	.Select(p => new
	{
		Name = p.ProductName,
		TotalOnHand = p.Inventories.Count > 0 ? p.Inventories.Sum(i => i.OnHandQuantity) : 0
		 // , toh2 = p.Inventories.Select(i => i.OnHandQuantity).Sum()
	})
	.OrderBy(p => p.Name)
	.Dump();