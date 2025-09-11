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

Employees
	.OrderBy(e => e.LastName)
	.Select(e => new
	{
		Name = e.FirstName + " " + e.LastName,
		Dept = e.DepartmentName,
		IncomeCategory = e.BaseRate < 30 ? "Review Required"
										: "No Review Required"
	})
	.Dump();
