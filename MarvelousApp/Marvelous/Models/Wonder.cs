using System.Collections.Generic;

namespace Marvelous.Models;

public class Wonder
{
    public WonderType Type { get; init; }
    public string Title { get; init; }
    public string SubTitle { get; init; }
    public string RegionTitle { get; init; }
    public string HistoryInfo1 { get; init; }
    public string HistoryInfo2 { get; init; }
    public string ConstructionInfo1 { get; init; }
    public string ConstructionInfo2 { get; init; }
    public string LocationInfo1 { get; init; }
    public string LocationInfo2 { get; init; }
    public string PullQuote1Top { get; init; }
    public string PullQuote1Bottom { get; init; }
    public string PullQuote1Author { get; init; }
    public string PullQuote2 { get; init; }
    public string PullQuote2Author { get; init; }
    public string Callout1 { get; init; }
    public string Callout2 { get; init; }
    public string UnsplashCollectionId { get; init; }
    public string VideoId { get; init; }
    public string VideoCaption { get; init; }
    public string MapCaption { get; init; }
    public IList<string> ImageIds { get; init; }
    public IList<string> Facts { get; init; }
    public int StartYr { get; init; }
    public int EndYr { get; init; }
    public int ArtifactStartYr { get; init; }
    public int ArtifactEndYr { get; init; }
    public string ArtifactCulture { get; init; }
    public string ArtifactGeolocation { get; init; }
    public double Lat { get; init; }
    public double Lng { get; init; }
    public IList<string> HighlightArtifacts { get; init; } // IDs used to assemble HighlightsData, should not be used directly
    public IList<string> HiddenArtifacts { get; init; } // IDs used to assemble CollectibleData, should not be used directly
    public IDictionary<int, string> Events { get; init; }
    public IList<Search> SearchData { get; set; } = new List<Search>();
    public IList<string> SearchSuggestions { get; set; } = new List<string>();

    IList<object> Props => new List<object> { Type, Title, HistoryInfo1, ImageIds, Facts };

    public static IReadOnlyDictionary<WonderType, Wonder> Config { get; } = new Dictionary<WonderType, Wonder>()
    {
        { 
            WonderType.ChichenItza, new Wonder
            {
                Type = WonderType.Colosseum,
                Title = "colosseumTitle",
                SubTitle = "colosseumSubTitle",
                RegionTitle = "colosseumRegionTitle",
                VideoId = "GXoEpNjgKzg",
                StartYr = 70,
                EndYr = 80,
                ArtifactStartYr = 0,
                ArtifactEndYr = 500,
                ArtifactCulture = "colosseumArtifactCulture",
                ArtifactGeolocation = "colosseumArtifactGeolocation",
                Lat = 41.890242126393495,
                Lng = 12.492349361871392,
                UnsplashCollectionId = "VPdti8Kjq9o",
                PullQuote1Top = "colosseumPullQuote1Top",
                PullQuote1Bottom = "colosseumPullQuote1Bottom",
                PullQuote1Author = "",
                PullQuote2 = "colosseumPullQuote2",
                PullQuote2Author = "colosseumPullQuote2Author",
                Callout1 = "colosseumCallout1",
                Callout2 = "colosseumCallout2",
                VideoCaption = "colosseumVideoCaption",
                MapCaption = "colosseumMapCaption",
                HistoryInfo1 = "colosseumHistoryInfo1",
                HistoryInfo2 = "colosseumHistoryInfo2",
                ConstructionInfo1 = "colosseumConstructionInfo1",
                ConstructionInfo2 = "colosseumConstructionInfo2",
                LocationInfo1 = "colosseumLocationInfo1",
                LocationInfo2 = "colosseumLocationInfo2",
                HighlightArtifacts = new List<string> {
                    "251350",
                    "255960",
                    "247993",
                    "250464",
                    "251476",
                    "255960",
                },
                HiddenArtifacts = new List<string> {
                    "245376",
                    "256570",
                    "286136",
                },
                Events = new Dictionary<int, string>{
                    [70] = "colosseum70ce",
                    [82] = "colosseum82ce",
                    [1140] = "colosseum1140ce",
                    [1490] = "colosseum1490ce",
                    [1829] = "colosseum1829ce",
                    [1990] = "colosseum1990ce",
                }
            }
        },

    };

}

public class Search
{
    const string BaseImagePath = "https://images.metmuseum.org/CRDImages/";

    public int Id { get; init; }
    public int Year { get; init; }
    public string Title { get; init; }
    public string Keywords { get; init; }
    public string ImagePath { get; init; }
    public double AspectRatio { get; init; }

    public string ImageUrl => BaseImagePath + ImagePath;


    public Search(int year, int id, string title, string keywords, string imagePath, double aspectRatio = 0)
    {
        Year = year;
        Id = id;
        Title = title;
        Keywords = keywords;
        ImagePath = imagePath;
        AspectRatio = aspectRatio;
    }


    public string Write()
    {
        return $"SearchData({Year}, {Id}, {Title}, {Keywords}, {ImagePath}, {GetFormatedAspectRatio})";
    }

    private string GetFormatedAspectRatio()
    {
        return AspectRatio == 0 ? "" : AspectRatio.ToString("0.00");
    }
}

