<Query Kind="Statements">
  <Connection>
    <ID>5369a8b5-dcd6-4128-b2f6-d33725f2277b</ID>
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
	.Select(a => new
	{
		Title = a.Title,
		Label = a.ReleaseLabel == null || a.ReleaseLabel.Trim() == ""	? "Unknown"
																		: a.ReleaseLabel,
		Artist = a.Artist.Name,
		Year = a.ReleaseYear
	})
	.OrderBy(a => a.Title)
	.ToList()
	.Dump();