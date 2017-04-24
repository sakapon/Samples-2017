using System.Net;

static readonly Random random = new Random();

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    var n = random.Next(1, 10000);
    return req.CreateResponse(HttpStatusCode.OK, n);
}
