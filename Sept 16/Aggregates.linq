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
	.OrderBy(x => x.Title)
	.Select(x => new
	{
		Title = x.Title,
		Artist = x.Artist.Name,
		TrackCount = x.Tracks.Count(),
		Time1 = x.Tracks.Sum(x => x.Milliseconds) / 1000,
		Time2 = x.Tracks.Select(x => x.Milliseconds).Sum() / 1000,
		MinTrack1 = x.Tracks.Min(x => x.Milliseconds) / 1000,
		MinTrack2 = x.Tracks.Select(x => x.Milliseconds).Min() / 1000,
		MaxTrack1 = x.Tracks.Max(x => x.Milliseconds) / 1000,
		MaxTrack2 = x.Tracks.Select(x => x.Milliseconds).Max() / 1000,
		AverageTrack1 = x.Tracks.Average(x => x.Milliseconds) / 1000,
		AverageTrack2 = x.Tracks.Select(x => x.Milliseconds).Average() / 1000,
		AverageTrack3 = x.Tracks.Sum(x => x.Milliseconds) / 1000 / x.Tracks.Count(),
		AverageTrack4 =  x.Tracks.Select(x => x.Milliseconds).Sum() / 1000 / x.Tracks.Count()
	}).Dump();