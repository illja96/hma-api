using System.Collections.Generic;

namespace HMA.API.Options
{
    public class CorsOptions
    {
        public List<string> AllowedOrigins { get; set; }

        public List<string> AllowedMethods { get; set; }

        public List<string> AllowedHeaders { get; set; }
    }
}
