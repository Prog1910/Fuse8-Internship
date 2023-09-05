FROM mcr.microsoft.com/dotnet/sdk:7.0 AS prepare-restore
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.2
WORKDIR /src
COPY . .
RUN dotnet subset restore "Api/PublicApi/PublicApi.csproj" --root-directory /src --output restore_subset/

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update && apt-get install -y curl
WORKDIR /src
COPY --from=prepare-restore /src/restore_subset/ .
RUN dotnet restore "Api/PublicApi/PublicApi.csproj"

COPY ["Api/PublicApi", "Api/PublicApi/"]
COPY ["Api/*.Shared", "Api/Api.Shared/"]
COPY ["Infrastructure/*.Public", "Infrastructure/Infrastructure.Public/"]
COPY ["Infrastructure/*.Shared", "Infrastructure/Infrastructure.Shared/"]
COPY ["Application/*.Public", "Application/Application.Public/"]
COPY ["Application/*.Shared", "Application/Application.Shared/"]
COPY ["Contracts", "Contracts/"]
COPY ["Domain", "Domain/"]
RUN dotnet publish "Api/PublicApi/PublicApi.csproj" -c Release -o /app/publish --no-restore 

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "PublicApi.dll"]