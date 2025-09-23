<Query Kind="Statements">
  <Connection>
    <ID>bf2de8d9-a3de-41ab-8ceb-e145025ece12</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

Albums
	.Where(x => x.ReleaseYear > 1970)
	.OrderBy(x => x.ReleaseYear)
	.ThenBy(x => x.ReleaseLabel)
	.Select(x => new
	{
		Year = x.ReleaseYear,
		Label = x.ReleaseLabel
	})
	.ToList()
	.Dump();

Albums
.Where(x => x.ReleaseYear > 1970)
.OrderBy(x => x.ReleaseYear)
.ThenBy(x => x.ReleaseLabel)
.Select(x => new
{
	Year = x.ReleaseYear,
	Label = x.ReleaseLabel
})
.Distinct()
.ToList()
.Dump();