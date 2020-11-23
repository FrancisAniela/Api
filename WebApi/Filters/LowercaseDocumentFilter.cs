using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Filters
{
    public class LowercaseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //PATHS
            var paths = swaggerDoc.Paths;

            //Generate the new keys
            var newPaths = new Dictionary<string, OpenApiPathItem>();
            var removeKeys = new List<string>();
            foreach (var path in paths)
            {
                var newKey = ToLowerEachSegmentStart(path.Key);
                if (newKey != path.Key)
                {
                    removeKeys.Add(path.Key);
                    newPaths.Add(newKey, path.Value);
                }
            }

            //Add the new keys
            foreach (var path in newPaths)
            {
                swaggerDoc.Paths.Add(path.Key, path.Value);
            }

            //Remove the old keys
            foreach (var key in removeKeys)
            {
                swaggerDoc.Paths.Remove(key);
            }
        }

        private static string ToLowerEachSegmentStart(string route)
        {
            string[] allSegments = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var toLowerResult = new StringBuilder();
            foreach (var currentSegment in allSegments)
            {
                if (currentSegment.StartsWith("{") && currentSegment.Length > 2)
                {
                    toLowerResult.Append($"/{{{currentSegment[1].ToString().ToLowerInvariant()}{currentSegment.Substring(2)}");
                }
                else
                {
                    toLowerResult.Append($"/{currentSegment[0].ToString().ToLowerInvariant()}");
                    if (currentSegment.Length > 1)
                    {
                        toLowerResult.Append(currentSegment.Substring(1));
                    }
                }
            }

            return toLowerResult.ToString();
        }
    }
}
