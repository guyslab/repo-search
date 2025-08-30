FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TibaRepoSearch.AnalysisFetcher/TibaRepoSearch.AnalysisFetcher.csproj", "TibaRepoSearch.AnalysisFetcher/"]
COPY ["TibaRepoSearch.Contract/TibaRepoSearch.Contract.csproj", "TibaRepoSearch.Contract/"]
RUN dotnet restore "./TibaRepoSearch.AnalysisFetcher/TibaRepoSearch.AnalysisFetcher.csproj"
COPY . .
WORKDIR "/src/TibaRepoSearch.AnalysisFetcher"
RUN dotnet build "./TibaRepoSearch.AnalysisFetcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./TibaRepoSearch.AnalysisFetcher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TibaRepoSearch.AnalysisFetcher.dll"]