using Microsoft.OpenApi.Models;

namespace MyFinance.Api.Helpers
{
    abstract class OpenApiServerBuilder
    {
        protected OpenApiServer OpenApiServer { get; private set; }
        public OpenApiServerBuilder()
        {
            OpenApiServer = new OpenApiServer();
        }

        public OpenApiServer GetOpenApiServer()
        {
            return OpenApiServer;
        }

        public abstract void SetDescription();
        public abstract void SetUrl();
    }

    class DevelopmentServerBuilder : OpenApiServerBuilder
    {
        private const string Description = "Localhost";
        private const string Url = "https://localhost:5001";
        public override void SetDescription()
        {
            OpenApiServer.Description = Description;
        }

        public override void SetUrl()
        {
            OpenApiServer.Url = Url;
        }
    }

    class ProductionServerBuilder : OpenApiServerBuilder
    {
        private const string Description = "Production server on AWS Lambda";
        private const string Url = "https://gk2ndif7w2.execute-api.us-east-1.amazonaws.com/Prod";
        public override void SetDescription()
        {
            OpenApiServer.Description = Description;
        }

        public override void SetUrl()
        {
            OpenApiServer.Url = Url;
        }
    }    

    class OpenApiServerProvider
    {
        private OpenApiServerBuilder _openApiServerBuilder;

        public void SetOpenApiServerBuilder(OpenApiServerBuilder builder)
        {
            _openApiServerBuilder = builder;
        }

        public OpenApiServer GetServer()
        {
            return _openApiServerBuilder.GetOpenApiServer();
        }

        public void ConstructOpenApiServer()
        {
            _openApiServerBuilder.SetDescription();
            _openApiServerBuilder.SetUrl();
        }
    }
}