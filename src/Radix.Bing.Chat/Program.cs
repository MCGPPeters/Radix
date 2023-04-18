// See https://aka.ms/new-console-template for more information

using System.Globalization;
using HtmlAgilityPack;
using System.Text;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Response;

string conversationId = "";
string conversationSignature = "";
string clientId = "";
using (HttpClient client = new HttpClient())
{
    client.DefaultRequestHeaders.Add("accept", "application/json");
    client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
    client.DefaultRequestHeaders.Add("sec-ch-ua",
        "\"Chromium\";v=\"112\", \"Microsoft Edge\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
    client.DefaultRequestHeaders.Add("sec-ch-ua-arch", "\"x86\"");
    client.DefaultRequestHeaders.Add("sec-ch-ua-bitness", "\"64\"");
    client.DefaultRequestHeaders.Add("sec-ch-ua-full-version", "\"112.0.1722.7\"");
    client.DefaultRequestHeaders.Add("sec-ch-ua-full-version-list",
        "\"Chromium\";v=\"112.0.5615.20\", \"Microsoft Edge\";v=\"112.0.1722.7\", \"Not:A-Brand\";v=\"99.0.0.0\"");
    client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
    client.DefaultRequestHeaders.Add("User-Agent",
        "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Mobile Safari/537.36 Edg/112.0.1722.34");
    client.DefaultRequestHeaders.Add("sec-ch-ua-model", "\"\"");
    client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
    client.DefaultRequestHeaders.Add("sec-ch-ua-platform-version", "\"15.0.0\"");
    client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
    client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
    client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
    client.DefaultRequestHeaders.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
    client.DefaultRequestHeaders.Add("x-ms-useragent",
        "549A35A07AEEF2E82AEBCBA91C1B9F1913BBED2FE8ADFE92F7CA1033CEA6F49E");
    client.DefaultRequestHeaders.Add("Sec-MS-GEC",
        "azsdk-js-api-client-factory/1.0.0-beta.1 core-rest-pipeline/1.10.0 OS/Win32");
    client.DefaultRequestHeaders.Add("Sec-MS-GEC-Version", "1-112.0.1722.34");
    client.DefaultRequestHeaders.Add("cookie",
        "ipv6=hit=1681405799689&t=4; USRLOC=HS=1&CLOC=LAT=51.54198117287724|LON=4.799573839747067|A=733.4464586120832|TS=230413101133|SRC=W; MUID=07A33602F11C6B7E05852422F0E96A57; _EDGE_V=1; SRCHD=AF=NOFORM; SRCHUID=V=2&GUID=22FBFACB88944818B2BC318D86CB1B3E&dmnchg=1; BCP=AD=0&AL=0&SM=0&CS=M; ANON=A=1724666A068E01393AF164E3FFFFFFFF&E=1b6b&W=2; BFBUSR=BAWAS=1&BAWFS=1; _UR=QS=0&TQS=0; _HPVN=CS=eyJQbiI6eyJDbiI6MiwiU3QiOjAsIlFzIjowLCJQcm9kIjoiUCJ9LCJTYyI6eyJDbiI6MiwiU3QiOjAsIlFzIjowLCJQcm9kIjoiSCJ9LCJReiI6eyJDbiI6MiwiU3QiOjAsIlFzIjowLCJQcm9kIjoiVCJ9LCJBcCI6dHJ1ZSwiTXV0ZSI6dHJ1ZSwiTGFkIjoiMjAyMy0wMy0wOFQwMDowMDowMFoiLCJJb3RkIjowLCJHd2IiOjAsIkRmdCI6bnVsbCwiTXZzIjowLCJGbHQiOjAsIkltcCI6NH0=; MUIDB=07A33602F11C6B7E05852422F0E96A57; SRCHUSR=DOB=20220915&T=1678972050000; BFB=AhCAxXg9v9gzgZ9kZl6832H8L1LqL7WGqsVYslhf5XkWXYdcbb7INqFcCCuJ-TQztIzT4FvjZ0np6JYLrbJ9Aw-2NmRVJxJt1XQgDGD_VLufJzTSs-PvbVB4KY0bEHg8oDUFihsweB7t0dVWtmitbX6oM9FUKbWLHjlfKLozRnNRCA; OID=AhDOq-TkF6dPkPU5DsYagIWOC2xR4nyEiSi-K64oUzi0fnUbfTSLTdtMLeAIb3N_TUzaoDKxST-55DGeNJDvzZiu; OIDI=AhAvHi-ONBQgB6YOK3YWH_ZUXGMF9ui1nNp90etqPXoFcw; EDGSRVCPERSIST=discoverl1=firsttimeinuds=1; _EDGE_S=SID=3CC669620ED96346300B7B930F6B62AB; WLS=C=3bcf4f74142cb936&N=Maurice; EDGSRVC=edgeservices=displaytheme=darkschemeovr|language=EN; EDGSRVCUSR=udscdxtone=Precise; EDGSRVCSCEN=shell=undersidev2=1|clientscopes=noheader-coauthor-chat-docvisibility-visibilitypm&chat=undersidev2=1|clientscopes=chat-noheader&compose=undersidev2=1|clientscopes=chat-coauthor-noheader&discoverl1=undersidev2=1|clientscopes=insights-noheader; _SS=SID=3CC669620ED96346300B7B930F6B62AB&R=706&RB=706&GB=0&RG=0&RP=531; dsc=order=ShopOrderNewsOverShop; ipv6=hit=1681310322044&t=4; _clck=1xh0wyu|1|fap|0; SUID=A; _RwBf=ilt=10&ihpd=0&ispd=3&rc=706&rb=706&gb=0&rg=0&pc=531&mtu=0&rbb=0&g=0&cid=&clo=0&v=1&l=2023-04-12T07:00:00.0000000Z&lft=0001-01-01T00:00:00.0000000&aof=0&o=0&p=bingcopilotwaitlist&c=MY00IA&t=5960&s=2023-02-08T08:02:58.4139652+00:00&ts=2023-04-12T13:38:37.7968697+00:00&rwred=0&wls=2&lka=0&lkt=0&TH=&mta=0&e=unNKnynFKQWXXIC524EWOSAwGAvHYdYZTQLFLsCpyt4hnbzmNS0M-IYk3IkWuNvGJqZL8SOXJ8VSATgLQDBaUA&A=1724666A068E01393AF164E3FFFFFFFF&mte=0; _U=1HdJXB6_VzbTclavcjSDn7Ik2-3natNWvAULQQeqMw72trqqBlfhzT1PnRlqIMb0zqKdySMpYv7kKOWP3zs7bxWnfI-p5sN8basPlCkmMRuHV85RK-nITb5vwvGHDXpi9rxmfKeRdJWittiTdoOthoO3efBzIg26hEiob6f2F5odhtjNj9XHK2_N38R3LGKlfznaTIpgNYP32NtaaoMPI-w; SRCHHPGUSR=SRCHLANG=en&PV=15.0.0&BRW=MW&BRH=MT&CW=376&CH=814&CH=814&SW=3440&SH=1440&DPR=2.5&UTC=120&DM=1&EXLTT=30&HV=1681308371&PRVCW=376&PRVCH=814&SCW=376&SCH=814&THEME=1&WTS=63816902938");
    client.DefaultRequestHeaders.Add("Referer",
        "https://edgeservices.bing.com/edgesvc/chat?clientscopes=chat,noheader&udsframed=1&form=SHORUN&shellsig=c28a41abd7f43af6fa4d676e351e8e86ac00aea2&setlang=en-US&darkschemeovr=1");
    client.DefaultRequestHeaders.Add("Referrer-Policy", "origin-when-cross-origin");
    // client.DefaultRequestHeaders.Add("x-forwarded-for", "1.1.1.1");


    HttpResponseMessage response =
        await client.GetAsync("https://edgeservices.bing.com/edgesvc/turing/conversation/create");
    string responseContent = await response.Content.ReadAsStringAsync();
    JObject jsonResponse = JObject.Parse(responseContent);
    conversationId = jsonResponse["conversationId"].ToString();
    conversationSignature = jsonResponse["conversationSignature"].ToString();
    clientId = jsonResponse["clientId"].ToString();
}


HubConnection hubConnection = new HubConnectionBuilder()
    .WithAutomaticReconnect()
    .ConfigureLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Error);
        logging.AddConsole();
    })
    .WithUrl("https://sydney.bing.com/sydney/ChatHub", HttpTransportType.WebSockets, options =>
    {
        options.UseDefaultCredentials = true;
        options.SkipNegotiation = true;
    })
    .Build();

await hubConnection.StartAsync();

hubConnection.On<object>("update", message =>
{
    //Console.WriteLine($"Received message: {message}" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);
});

Func<string> genRanHex = () =>
{
    Random random = new Random();
    string hexString = string.Format("{0:X8}", random.Next(int.MaxValue));
    return hexString;
};
string input = string.Empty;
bool isStartOfSession = true;

HttpResponseMessage jumboContent =
    await new HttpClient().GetAsync("https://www.jumbo.com/zoeken/?searchType=keyword&searchTerms=brood");


while (input != "exit")
{
    Console.WriteLine($"Me : ");
    input = Console.ReadLine();
    var message = new[]
    {
        new
        {
            source = "cib",
            optionsSets =
                new[]
                {
                    "nlu_direct_response_filter", "deepleo", "disable_emoji_spoken_text",
                    "responsible_ai_policy_235", "enablemm", "h3precise", "cachewriteext", "dlvpop",
                    "e2ecachewrite", "nodlcpcwrite", "nointernalsugg", "enuaug", "nourldedupe", "responseos",
                    "sportsansgnd", "dl_edge_prompt", "glprompt", "dv3sugg"
                },
            allowedMessageTypes =
                new[]
                {
                    "Chat", "InternalSearchQuery", "InternalSearchResult", "Disengaged", "InternalLoaderMessage",
                    "RenderCardRequest", "AdsQuery", "SemanticSerp", "GenerateContentQuery", "SearchQuery"
                },
            sliceIds =
                new[]
                {
                    "0404dlvpop_pc", "0404sydicnbs0", "330uaug", "403recansgnds0", "405suggpc", "406sportgnd",
                    "407dlforms0", "408nodedup", "afftoaltcf", "chk1cln", "nopreloadsstf", "perfimpcomb",
                    "sugdivdis", "sydnoinputt", "udscahrfoncf", "wpcssopt", "0329resp", "chatgptsugg"
                },
            verbosity = "verbose",
            traceId = genRanHex(),
            isStartOfSession = isStartOfSession,
            message =
                new
                {
                    locale = "en-US",
                    market = "en-US",
                    region = "NL",
                    location = "lat:47.639557;long:-122.128159;re=1000m;",
                    locationHints
                        = new[]
                        {
                            new
                            {
                                country = "Netherlands",
                                state = "North Brabant",
                                city = "Breda",
                                zipcode = "4814",
                                timezoneoffset = 1,
                                countryConfidence = 8,
                                cityConfidence = 8,
                                Center = new {Latitude = 51, Longitude = 4},
                                RegionType = 2,
                                SourceType = 1
                            }
                        },
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:sszzz", CultureInfo.InvariantCulture),
                    author = "user",
                    inputMethod = "Keyboard",
                    text = input,
                    messageType = "Chat"
                },
            conversationSignature,
            participant = new {id = clientId},
            conversationId
        }
    };
    await foreach (object response in hubConnection.StreamAsyncCore<object>("Chat", message))
    {
        Root? obj = JsonConvert.DeserializeObject<Root>(response.ToString());
        Console.WriteLine();
        Console.WriteLine($"Bing :");
        foreach (Message objMessage in obj.Messages)
        {
            Console.WriteLine(objMessage.Text);
        }

        Console.WriteLine();
    };
    isStartOfSession = false;
}

await hubConnection.StopAsync();
Console.Read();

namespace Request
{
    public record Center(
        [property: JsonProperty("Latitude")] int Latitude,
        [property: JsonProperty("Longitude")] int Longitude
    );

    public record LocationHint(
        [property: JsonProperty("country")] string Country,
        [property: JsonProperty("state")] string State,
        [property: JsonProperty("city")] string City,
        [property: JsonProperty("zipcode")] string Zipcode,
        [property: JsonProperty("timezoneoffset")]
        int Timezoneoffset,
        [property: JsonProperty("countryConfidence")]
        int CountryConfidence,
        [property: JsonProperty("cityConfidence")]
        int CityConfidence,
        [property: JsonProperty("Center")] Center Center,
        [property: JsonProperty("RegionType")] int RegionType,
        [property: JsonProperty("SourceType")] int SourceType
    );

    public record Message(
        [property: JsonProperty("locale")] string Locale,
        [property: JsonProperty("market")] string Market,
        [property: JsonProperty("region")] string Region,
        [property: JsonProperty("location")] string Location,
        [property: JsonProperty("locationHints")]
        IReadOnlyList<LocationHint> LocationHints,
        [property: JsonProperty("timestamp")] DateTime Timestamp,
        [property: JsonProperty("author")] string Author,
        [property: JsonProperty("inputMethod")]
        string InputMethod,
        [property: JsonProperty("text")] string Text,
        [property: JsonProperty("messageType")]
        string MessageType
    );

    public record Root(
        [property: JsonProperty("source")] string Source,
        [property: JsonProperty("optionsSets")]
        IReadOnlyList<string> OptionsSets,
        [property: JsonProperty("allowedMessageTypes")]
        IReadOnlyList<string> AllowedMessageTypes,
        [property: JsonProperty("sliceIds")] IReadOnlyList<string> SliceIds,
        [property: JsonProperty("verbosity")] string Verbosity,
        [property: JsonProperty("traceId")] string TraceId,
        [property: JsonProperty("isStartOfSession")]
        bool IsStartOfSession,
        [property: JsonProperty("message")] Message Message
    );
}


namespace Response
{
    public record AdaptiveCard(
        [property: JsonProperty("type")] string Type,
        [property: JsonProperty("version")] string Version,
        [property: JsonProperty("body")] IReadOnlyList<Body> Body
    );

    public record Body(
        [property: JsonProperty("type")] string Type,
        [property: JsonProperty("text")] string Text,
        [property: JsonProperty("wrap")] bool Wrap
    );

    public record Center(
        [property: JsonProperty("latitude")] int Latitude,
        [property: JsonProperty("longitude")] int Longitude,
        [property: JsonProperty("height")] object Height
    );

    public record ClassificationRanking(
        [property: JsonProperty("classification")]
        string Classification,
        [property: JsonProperty("score")] object Score
    );

    public record Feedback(
        [property: JsonProperty("tag")] object Tag,
        [property: JsonProperty("updatedOn")] object UpdatedOn,
        [property: JsonProperty("type")] string Type
    );

    public record From(
        [property: JsonProperty("id")] string Id,
        [property: JsonProperty("name")] object Name
    );

    public record LocationHint(
        [property: JsonProperty("country")] string Country,
        [property: JsonProperty("countryConfidence")]
        int CountryConfidence,
        [property: JsonProperty("state")] string State,
        [property: JsonProperty("city")] string City,
        [property: JsonProperty("cityConfidence")]
        int CityConfidence,
        [property: JsonProperty("zipCode")] string ZipCode,
        [property: JsonProperty("timeZoneOffset")]
        int TimeZoneOffset,
        [property: JsonProperty("sourceType")] int SourceType,
        [property: JsonProperty("center")] Center Center,
        [property: JsonProperty("regionType")] int RegionType
    );

    public record Message(
        [property: JsonProperty("text")] string Text,
        [property: JsonProperty("author")] string Author,
        [property: JsonProperty("from")] From From,
        [property: JsonProperty("createdAt")] DateTime CreatedAt,
        [property: JsonProperty("timestamp")] DateTime Timestamp,
        [property: JsonProperty("locale")] string Locale,
        [property: JsonProperty("market")] string Market,
        [property: JsonProperty("region")] string Region,
        [property: JsonProperty("location")] string Location,
        [property: JsonProperty("locationHints")]
        IReadOnlyList<LocationHint> LocationHints,
        [property: JsonProperty("messageId")] string MessageId,
        [property: JsonProperty("requestId")] string RequestId,
        [property: JsonProperty("nlu")] Nlu Nlu,
        [property: JsonProperty("offense")] string Offense,
        [property: JsonProperty("feedback")] Feedback Feedback,
        [property: JsonProperty("contentOrigin")]
        string ContentOrigin,
        [property: JsonProperty("privacy")] object Privacy,
        [property: JsonProperty("inputMethod")]
        string InputMethod,
        [property: JsonProperty("adaptiveCards")]
        IReadOnlyList<AdaptiveCard> AdaptiveCards,
        [property: JsonProperty("sourceAttributions")]
        IReadOnlyList<object> SourceAttributions,
        [property: JsonProperty("suggestedResponses")]
        IReadOnlyList<SuggestedResponse> SuggestedResponses,
        [property: JsonProperty("spokenText")] string SpokenText
    );

    public record Nlu(
        [property: JsonProperty("scoredClassification")]
        ScoredClassification ScoredClassification,
        [property: JsonProperty("classificationRanking")]
        IReadOnlyList<ClassificationRanking> ClassificationRanking,
        [property: JsonProperty("qualifyingClassifications")]
        object QualifyingClassifications,
        [property: JsonProperty("ood")] object Ood,
        [property: JsonProperty("metaData")] object MetaData,
        [property: JsonProperty("entities")] object Entities
    );

    public record Result(
        [property: JsonProperty("value")] string Value,
        [property: JsonProperty("message")] string Message,
        [property: JsonProperty("serviceVersion")]
        string ServiceVersion
    );

    public record Root(
        [property: JsonProperty("messages")] IReadOnlyList<Message> Messages,
        [property: JsonProperty("firstNewMessageIndex")]
        int FirstNewMessageIndex,
        [property: JsonProperty("conversationId")]
        string ConversationId,
        [property: JsonProperty("requestId")] string RequestId,
        [property: JsonProperty("conversationExpiryTime")]
        DateTime ConversationExpiryTime,
        [property: JsonProperty("shouldInitiateConversation")]
        bool ShouldInitiateConversation,
        [property: JsonProperty("telemetry")] Telemetry Telemetry,
        [property: JsonProperty("throttling")] Throttling Throttling,
        [property: JsonProperty("result")] Result Result
    );

    public record ScoredClassification(
        [property: JsonProperty("classification")]
        string Classification,
        [property: JsonProperty("score")] object Score
    );

    public record SuggestedResponse(
        [property: JsonProperty("text")] string Text,
        [property: JsonProperty("author")] string Author,
        [property: JsonProperty("createdAt")] DateTime CreatedAt,
        [property: JsonProperty("timestamp")] DateTime Timestamp,
        [property: JsonProperty("messageId")] string MessageId,
        [property: JsonProperty("messageType")]
        string MessageType,
        [property: JsonProperty("offense")] string Offense,
        [property: JsonProperty("feedback")] Feedback Feedback,
        [property: JsonProperty("contentOrigin")]
        string ContentOrigin,
        [property: JsonProperty("privacy")] object Privacy
    );

    public record Telemetry(
        [property: JsonProperty("metrics")] object Metrics,
        [property: JsonProperty("startTime")] DateTime StartTime
    );

    public record Throttling(
        [property: JsonProperty("maxNumUserMessagesInConversation")]
        int MaxNumUserMessagesInConversation,
        [property: JsonProperty("numUserMessagesInConversation")]
        int NumUserMessagesInConversation
    );
}
