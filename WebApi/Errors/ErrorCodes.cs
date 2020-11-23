using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebApi.Errors
{
    public static class ErrorCodes
    {
        public const int Error106ResourceNotFound = 106;
        public const int Error107ImageNotFound = 107;
        public const int Error108FileNotFound = 108;
        public const int Error109ResourceExists = 109;

        public const int Error121InvalidArgument = 121;
        public const int Error122InvalidJson = 122;
        public const int Error123InvalidQueryParameter = 123;
        public const int Error124ValidationError = 124;

        public const int Error130Unauthorized = 130;
        public const int Error131InvalidHeader = 131;

        public const int Error199UnknownError = 199;

        public static ReadOnlyDictionary<int, string> ErrorsDescription = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>() {
            { Error106ResourceNotFound, "Resource not found" },
            { Error107ImageNotFound, "Image not Found" },
            { Error108FileNotFound, "File not found"},
            { Error109ResourceExists, "Resource already exists" },
            { Error121InvalidArgument, "Invalid argument - An invalid value was specified on the URI"},
            { Error122InvalidJson, "Invalid json parameter value"},
            { Error123InvalidQueryParameter, "Invalid query parameter value "},
            { Error124ValidationError, "Validation error"},
            { Error130Unauthorized, "Unauthorized request"},
            { Error131InvalidHeader, "Invalid Header"},
            { Error199UnknownError, "Internal server error"},
        });
    }
}
