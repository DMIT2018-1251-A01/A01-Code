<Query Kind="Statements">
  <Connection>
    <ID>87b589ca-6377-4501-bf9b-ca701d8a544d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>WestWind-2024</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

Customers
.GroupBy(c => c.Region)
.Select(g => new
{
	Region = g.Key == null ? "Unknown" : g.Key,
	OrderCount = g.Sum(c => c.Orders.Count()),
	List = g.Select(a => a.Orders.Select
			(o => new
			{
				EmployeeID = o.EmployeeID,
				ShipCity = o.ShipCity
			}
			)
	).ToList()
})
.OrderBy(g => g.OrderCount)
.ToList().Dump();

OrderDetails
	.GroupBy(od => od.Product.ProductName)
	.Select(g => new
	{
		Product = g.Key,
		TotalQty = g.Sum(od => od.Quantity),
		TotalSales = g.Sum(od => od.Quantity * od.UnitPrice)
	}).Dump();
	
	
	
	
	
	
	
	
	
	
	
	
	