using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Parse query parameters
    var q = req.GetQueryNameValuePairs().ToArray();
    var n = q.FirstOrDefault(p => string.Compare(p.Key, "n", true) == 0).Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    n = data?.n?.ToString() ?? n;

    return n == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Pass n property on the query string or in the request body.")
        : req.CreateResponse(HttpStatusCode.OK, Factorize(n));
}

public static string Factorize(string text)
{
    int i;
    if (!int.TryParse(text, out i)) return "Send an integer.";

    var factorized = Factorize2(Math.Abs(i));
    var factorizedString = string.Join(" ・ ", factorized.Select(p => $"{p.Key}{(p.Value == 1 ? "" : $"^{p.Value}")}"));
    return $"{i} = {(i >= 0 ? "" : "-")}{factorizedString}";
}

public static Dictionary<int, int> Factorize2(int x)
{
    if (x <= 1) return new Dictionary<int, int> { { x, 1 } };

    var d = new Dictionary<int, int>();

    for (var i = 2; ; i++)
    {
        while (x % i == 0)
        {
            if (d.ContainsKey(i))
                d[i]++;
            else
                d[i] = 1;
            x /= i;

            if (x == 1)
                return d;
        }
    }
}
