
namespace WebApi.GraphAndFileHandler
{

        public class TokenResponse
        {
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public int ext_expires_in { get; set; }
            public string access_token { get; set; }
        }
        public class GraphAuthentificator
        {
            private string url;
            public GraphAuthentificator()
            {
                this.url = "https://login.microsoft.com/c6852595-366b-4047-873e-354632ca8c49/oauth2/v2.0/token";
            }

            public async Task<string> GetQualtechApiToken()
            {
                string retToken = "";
                HttpClient client = new HttpClient();
                using (client)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {

                        var tokenRequest = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", "a6ad3cce-7987-4c17-8c3a-e1c37d795bdf"),
                        new KeyValuePair<string, string>("client_secret", "7no8Q~4kvx55Ugb3cRqKKiSyqcBjRYC6OFOuEbUq"),
                        new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),

                     });



                        HttpResponseMessage resp = await client.PostAsync(url, tokenRequest);

                        if (resp.IsSuccessStatusCode)
                        {
                            Console.WriteLine("ok");
                            if (resp.Content != null)
                            {
                                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(
                                    await resp.Content.ReadAsStringAsync()
                                );
                                retToken = tokenResponse.access_token;
                                //Console.WriteLine(tokenResponse.access_token);
                            }
                            else
                            {
                                //Console.WriteLine("Response content is null.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Erreur get bearer (client secret might be expired): " + resp.StatusCode + "   ");
                        }
                    }

                    return retToken;
                }




           }
        }
}
