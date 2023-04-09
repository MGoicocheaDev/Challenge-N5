using Nest;
using web_api_lib_data.Models;

namespace web_api_permision.ElkConfiguration
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var config =  configuration.GetSection("ELKConfiguration").Get<ELKConfiguration>();
            var url = config.Uri;
            var defaultIndex = config.index;

            //var settings = new ConnectionSettings(new Uri(url)).BasicAuthentication(userName, pass)
            var settings = new ConnectionSettings(new Uri(url))
                .PrettyJson()
                .DefaultIndex(defaultIndex);

            //AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        //private static void AddDefaultMappings(ConnectionSettings settings)
        //{
        //    settings
        //        .DefaultMappingFor<Permission>(m => m
        //            .PropertyName( x => x.Id,"Id")
        //            .PropertyName(x => x.Id, "Id")
        //            .PropertyName(x => x.Id, "Id")
        //            .PropertyName(x => x.Id, "Id")
        //        );
        //}

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Permission>(x => x.AutoMap())
            );
        }
    }

}
