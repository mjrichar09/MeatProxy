using System.Text.Json.Nodes;
using MeatProxy.Core;
using MeatProxy.Core.Content;
using MeatProxy.Core.World;

namespace MeatProxy.Tests;

/// <summary>
/// The geometry has to agree with the graph (ADR 0012): the same records build
/// the playable house and the rendered one. These start from the shipped house
/// and break it one way at a time, so the check is proven to bite.
/// </summary>
public class GeometryTests
{
    [Fact]
    public void Every_room_has_authored_geometry()
    {
        Assert.All(TestHouse.Load().Rooms, r => Assert.Equal(GeometryProvenance.Authored, r.Geometry));
    }

    [Fact]
    public void The_shipped_house_has_no_geometry_problems()
    {
        Assert.Empty(TestHouse.Load().Validate(TestHouse.File.StartRoom));
    }

    [Fact]
    public void Utility_is_still_the_worked_example()
    {
        var utility = TestHouse.Load().Room(new RoomId("utility"));
        Assert.Equal(new Point2(4.0, 0.0), utility.Origin);
        Assert.Equal(new Extent3(3.2, 2.4, 2.2), utility.Dims);
    }

    [Fact]
    public void Overlapping_rooms_are_caught()
    {
        var problems = ValidateWith(rooms => Room(rooms, "pantry")["origin"] = new JsonArray(8.0, 3.0));
        Assert.Contains(problems, p => p.Contains("'kitchen' and 'pantry' overlap") || p.Contains("'pantry' and 'kitchen' overlap"));
    }

    [Fact]
    public void A_door_in_a_wall_the_rooms_do_not_share_is_caught()
    {
        var problems = ValidateWith(rooms => Opening(rooms, "hall", "hall__kitchen")["wall"] = "west");
        Assert.Contains(problems, p => p.Contains("'hall__kitchen'"));
    }

    [Fact]
    public void An_opening_off_the_end_of_its_wall_is_caught()
    {
        var problems = ValidateWith(rooms => Opening(rooms, "kitchen", "kitchen__pantry")["offset"] = 3.8);
        Assert.Contains(problems, p => p.Contains("'kitchen__pantry' runs off the end"));
    }

    [Fact]
    public void An_exterior_door_that_opens_into_a_room_is_caught()
    {
        // Slide the front door along the hall's south wall until it lands in the garage.
        var problems = ValidateWith(rooms => Opening(rooms, "hall", "hall__exterior")["offset"] = 0.9);
        Assert.Contains(problems, p => p.Contains("'hall__exterior' leads outside, but opens into 'garage'"));
    }

    private static IReadOnlyList<string> ValidateWith(Action<JsonArray> mutate)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Content", HouseData.FileName);
        var json = JsonNode.Parse(File.ReadAllText(path))!;
        mutate(json["rooms"]!.AsArray());

        var file = HouseFile.FromJson(json.ToJsonString());
        return file.ToHouse().Validate(file.StartRoom);
    }

    private static JsonNode Room(JsonArray rooms, string id) =>
        rooms.First(r => (string?)r!["id"] == id)!;

    private static JsonNode Opening(JsonArray rooms, string room, string id) =>
        Room(rooms, room)["openings"]!.AsArray().First(o => (string?)o!["id"] == id)!;
}
