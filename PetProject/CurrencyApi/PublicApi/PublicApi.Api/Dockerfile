FROM mcr.microsoft.com/dotnet/sdk:7.0 AS prepare-restore
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.2
WORKDIR /src
COPY . .
RUN dotnet subset restore "PublicApi/PublicApi.Api/PublicApi.Api.csproj" --root-directory /src --output restore_subset/

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update && apt-get install -y curl
WORKDIR /src
COPY --from=prepare-restore /src/restore_subset/ .
RUN dotnet restore "PublicApi/PublicApi.Api/PublicApi.Api.csproj"

COPY ["PublicApi/*.Api", "PublicApi/PublicApi.Api/"]
COPY ["PublicApi/*.Application", "PublicApi/PublicApi.Application/"]
COPY ["PublicApi/*.Infrastructure", "PublicApi/PublicApi.Infrastructure/"]
COPY ["Shared/*.Application", "Shared/Shared.Application/"]
COPY ["Contracts", "Contracts/"]
COPY ["Domain", "Domain/"]
RUN dotnet publish "PublicApi/PublicApi.Api/PublicApi.Api.csproj" -c Release -o /app/publish --no-restore 

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5166
ENTRYPOINT ["dotnet", "PublicApi.Api.dll"]