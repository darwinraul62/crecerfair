using System;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Admin.Web.Services
{
    public static class APIUri
    {
        public static class Provider
        {
            public static string Get(QueryRequest request) 
            {
                return request.ToQueryString("/api/providers");
            }

            public static string Get(Guid id) => $"/api/providers/{id}";            
            public static string Insert() => "/api/providers";
            public static string Update() => "/api/providers";
            public static string GetResources(Guid providerId, string resourceTypeId = null) => $"/api/providers/{providerId}/resources?resourceTypeId={resourceTypeId}";
            public static string GetResource(Guid providerId, Guid resourceId) => $"/api/providers/{providerId}/resources/{resourceId}";
            public static string AddResource(Guid providerId) => $"/api/providers/{providerId}/resources";
            public static string UpdateResourcesPriority(Guid providerId, string type) => $"/api/providers/{providerId}/resources/priorities?resourceTypeId={type}";
            public static string UpdatePartial(Guid providerId, Guid resourceId) => $"/api/providers/{providerId}/resources/{resourceId}";
            public static string DeleteResource(Guid providerId, Guid resourceId) => $"/api/providers/{providerId}/resources/{resourceId}";
            public static string RemoveGeneralResource(Guid providerId, Guid resourceId) => $"/api/providers/{providerId}/resources?id={resourceId}";
            public static string GetUsers(Guid providerId) => $"/api/providers/{providerId}/users";
            public static string AddUser(Guid providerId) => $"/api/providers/{providerId}/users";
            public static string RemoveUser(Guid providerId, Guid userId) => $"/api/providers/{providerId}/users/{userId}";
            public static string GetProviderRelationByUser(Guid userId) => $"/api/users/{userId}/providersrelation";            
            public static string GetCalendar(Guid providerId) => $"/api/providers/{providerId}/calendar";
            public static string SetCalendar(Guid providerId) => $"/api/providers/{providerId}/calendar";
        
        }
        public static class ResourceType
        {                        
            public static string Get() => $"/api/resourceTypes";
            public static string Get(string id) => $"/api/resourceTypes/{id}";
        }

        public static class ProviderCategory
        {
            public static string Get(QueryRequest request) 
            {
                return request.ToQueryString("/api/categories");
            }

            public static string Get(Guid id) => $"/api/categories/{id}";            
            public static string Insert() => "/api/categories";
            public static string Update() => "/api/categories";
            public static string Delete(Guid id) => $"/api/categories/{id}";            
        }
        public static class Fair
        {
            public static string Get() => $"/api/fairs";
            public static string Update(Guid fairId) => $"/api/fairs/{fairId}";
            public static string GetStands(Guid fairId) => $"/api/fairs/{fairId}/stands";
            public static string UpdateStandPartial(Guid fairId, Guid standId) => $"/api/fairs/{fairId}/stands/{standId}";
            public static string GetStandsVisitCount(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId) 
                => $"/api/fairs/{fairId}/stands/visits/count?dateFrom={dateFrom.ToString("s")}&dateTo={dateTo.ToString("s")}&providerId={providerId}";

            public static string GetStandsCount(Guid fairId) 
                => $"/api/fairs/{fairId}/stands/count";

            public static string GetUserFairs(QueryRequest request) 
                => request.ToQueryString("/api/users");

            public static string GetProviderVisitCount(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId) 
                => $"/api/fairs/{fairId}/stands/visits/providers?dateFrom={dateFrom.ToString("s")}&dateTo={dateTo.ToString("s")}&providerId={providerId}";

            public static string GetPartnertStandVisitCount(Guid fairId, DateTime dateFrom, DateTime dateTo, QueryRequest queryRequest)
            {
                return queryRequest.ToQueryString($"/api/fairs/{fairId}/stands/visits/partnerts/resume?dateFrom={dateFrom.ToString("s")}&dateTo={dateTo.ToString("s")}");
            }                
        }

        public static class Partnert
        {
            public static string Get(QueryRequest queryRequest) => queryRequest.ToQueryString("/api/partnerts");
            public static string Import() => "/api/partnerts/import";
        }
    }
}