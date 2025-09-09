<Query Kind="Expression">
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
	.Where(a => a.AlbumId < 6)
	.Select(a => new
	{
		Album = a.Title,
		Label = a.ReleaseLabel,
		Year = a.ReleaseYear
	})
	.OrderBy(a => a.Album)
	