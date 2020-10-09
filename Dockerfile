FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MyFinance.Api/MyFinance.Api.csproj", "MyFinance.Api/"]
COPY ["MyFinance.Services/MyFinance.Services.csproj", "MyFinance.Services/"]
COPY ["MyFinance.Domain/MyFinance.Domain.csproj", "MyFinance.Domain/"]
COPY ["MyFinance.Repositories/MyFinance.Repositories.csproj", "MyFinance.Repositories/"]
COPY ["MyFinance.Api.Tests/MyFinance.Api.Tests.csproj", "MyFinance.Api.Tests/"]
COPY ["MyFinance.Repositories.Tests/MyFinance.Repositories.Tests.csproj", "MyFinance.Repositories.Tests/"]

RUN dotnet restore "MyFinance.Api/MyFinance.Api.csproj"
COPY . .
WORKDIR "/src/MyFinance.Api"
RUN dotnet build "MyFinance.Api.csproj" -c Release -o /app/build

RUN dotnet test

FROM build AS publish
RUN dotnet publish "MyFinance.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFinance.Api.dll"]