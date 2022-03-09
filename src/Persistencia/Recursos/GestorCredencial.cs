using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace EDM.RFLocal.Interceptador.Repositorio.Recursos
{
    public static class GestorCredencial
    {
        public static string GetSecret(string NomeSecret, string stringConexao, IConfiguration _configuration, IAmazonSecretsManager _secretsManager)
        {
            string connectionString = _configuration.GetConnectionString(stringConexao);
            string secretName = _configuration[NomeSecret];

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT"
            };

            var response = _secretsManager.GetSecretValueAsync(request).Result;

            if (string.IsNullOrEmpty(response.SecretString))
                throw new Exception("Falha na recuperação do secret AWS : Secret Nulo");

            var secretObj = JsonConvert.DeserializeObject<Secret>(response.SecretString);
            connectionString = connectionString.Replace("SecretUser", secretObj.Username);
            connectionString = connectionString.Replace("SecretPass", secretObj.Password);

            return connectionString;

        }

        internal class Secret
        {
            [JsonProperty(PropertyName = "username")]
            public string Username { get; set; }
            [JsonProperty(PropertyName = "password")]
            public string Password { get; set; }

        }
    }

}
