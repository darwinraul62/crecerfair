using System;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Web.Services
{
    internal static class APIUri
    {
        public static class Partnert
        {
            public static string Create() => "/api/partnerts";
            public static string Update(Guid id) => $"/api/partnerts/{id}";
            public static string GetByIdentification(string identification) => $"/api/partnerts/byidentification?identification={identification}";
            public static string Delete(Guid partnertId) => "/api/partnerts/{partnertId}";
            public static string AllowRegister(string identification) => $"/api/partnerts/allowregister/{identification}";
        }

        public static class Fair
        {
            public static string Get() => $"/api/fairs";
            public static string GetStands(Guid fairId) => $"/api/fairs/{fairId}/stands";
            public static string GetStandsById(Guid fairId, Guid standId) => $"/api/fairs/{fairId}/stands/{standId}";
            public static string GetStandsUsers(Guid fairId) => $"/api/fairs/{fairId}/stands/users";
            public static string RegisterStandVisit(Guid fairId, Guid standId) => $"/api/fairs/{fairId}/stands/{standId}/visits";                    
        }

        public static class StandModel
        {
            public static string Get(string modelCode) => $"/api/standmodels/{modelCode}";            
        }

        public static class Provider
        {
            public static string Get(QueryRequest request)
            {
                return request.ToQueryString("/api/providers");
            }

            public static string Get(Guid id) => $"/api/providers/{id}";
            public static string GetResources(Guid providerId, string resourceTypeId = null) 
                => $"/api/providers/{providerId}/resources?resourceTypeId={resourceTypeId}";
            public static string GetCalendar(Guid providerId) => $"/api/providers/{providerId}/calendar";
            public static string GetUsers(Guid providerId) => $"/api/providers/{providerId}/users";
        }
    }
}
