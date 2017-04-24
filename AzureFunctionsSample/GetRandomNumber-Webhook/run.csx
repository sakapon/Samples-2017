#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;

static readonly Random random = new Random();

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    var n = random.Next(1, 10000);
    return req.CreateResponse(HttpStatusCode.OK, new {
        n
    });
}
