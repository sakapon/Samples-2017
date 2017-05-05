using System.Net;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    dynamic data = await req.Content.ReadAsAsync<object>();

    return data?.n == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, new { error = "Pass n property in the input object." })
        : req.CreateResponse(HttpStatusCode.OK, new { result = Factorize(data.n.ToString()) });
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
