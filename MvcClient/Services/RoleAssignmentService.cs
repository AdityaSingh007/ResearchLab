using MvcClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MvcClient.Services
{
    public class RoleAssignmentService
    {
        private RoleAssignmentModel _roleAssisgnmentModel;
        private HttpClient _httpClient = new HttpClient();
        public RoleAssignmentService(RoleAssignmentModel roleAssisgnmentModel)
        {
            _roleAssisgnmentModel = roleAssisgnmentModel;
        }

        public async Task<HttpResponseMessage> AssignRole(string accessToken)
        {
            var azureRoleManagementUri = BuildUriForRoleAssignment();
            var jsonContent = generateJsonRequestBodyContent();

            ConfigureClient(accessToken);

            var roleAssignmentResponse = await _httpClient.PutAsync(azureRoleManagementUri, jsonContent);
            var statusCode = roleAssignmentResponse.StatusCode;
            return roleAssignmentResponse;
        }

        private void ConfigureClient(string accessToken)
        {
            if(!string.IsNullOrEmpty(accessToken))
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        private StringContent generateJsonRequestBodyContent()
        {
            var jsonRequestBodyContent = JsonConvert.SerializeObject(new RoleAssignmentJsonRequestBody(
                new Properties
                (
                    _roleAssisgnmentModel.RoleId,
                    _roleAssisgnmentModel.SubscriptionId,
                    _roleAssisgnmentModel.ServicePrincipalObjectId)
                ));
            return new StringContent(jsonRequestBodyContent, Encoding.UTF8, "application/json");
        }

        private Uri BuildUriForRoleAssignment()
        {
            var uri = new StringBuilder();

            if (_roleAssisgnmentModel != null)
            {
                uri.Append("https://management.azure.com/subscriptions/")
                   .Append($"{_roleAssisgnmentModel.SubscriptionId}/")
                   .Append($"resourceGroups/{_roleAssisgnmentModel.ResourceGroupName}/")
                   .Append("providers/Microsoft.Storage/")
                   .Append($"storageAccounts/{_roleAssisgnmentModel.StorageAccountName}/")
                   .Append($"blobServices/default/containers/{_roleAssisgnmentModel.ContainerName}/")
                   .Append($"providers/Microsoft.Authorization/roleAssignments/{_roleAssisgnmentModel.RoleAssignmentName}")
                   .Append("?api-version=2019-04-01-preview");
            }

            return new Uri(uri.ToString());
        }
    }
}