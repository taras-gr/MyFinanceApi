FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["source/MyFinance.Api/MyFinance.Api.csproj", "source/MyFinance.Api/"]
COPY ["source/MyFinance.Services/MyFinance.Services.csproj", "source/MyFinance.Services/"]
COPY ["source/MyFinance.Domain/MyFinance.Domain.csproj", "source/MyFinance.Domain/"]
COPY ["source/MyFinance.Repositories/MyFinance.Repositories.csproj", "source/MyFinance.Repositories/"]
COPY ["tests/MyFinance.Api.Tests/MyFinance.Api.Tests.csproj", "tests/MyFinance.Api.Tests/"]
COPY ["tests/MyFinance.Repositories.Tests/MyFinance.Repositories.Tests.csproj", "tests/MyFinance.Repositories.Tests/"]

RUN dotnet restore "source/MyFinance.Api/MyFinance.Api.csproj"
COPY . .
WORKDIR "/src/source/MyFinance.Api"
RUN dotnet build "MyFinance.Api.csproj" -c Release -o /app/build

RUN dotnet test

FROM build AS publish
RUN dotnet publish "MyFinance.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFinance.Api.dll"]