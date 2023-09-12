FROM mcr.microsoft.com/dotnet/sdk:7.0 AS prepare-restore
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.2
WORKDIR /src
COPY . .
RUN dotnet subset restore "InternalApi/InternalApi.Api/InternalApi.Api.csproj" --root-directory /src --output restore_subset/

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update && apt-get install -y curl
WORKDIR /src
COPY --from=prepare-restore /src/restore_subset/ .
RUN dotnet restore "InternalApi/InternalApi.Api/InternalApi.Api.csproj"

COPY ["InternalApi/*.Api", "InternalApi/InternalApi.Api/"]
COPY ["InternalApi/*.Infrastructure", "InternalApi/InternalApi.Infrastructure/"]
COPY ["InternalApi/*.Application", "InternalApi/InternalApi.Application/"]
COPY ["InternalApi/*.Domain", "InternalApi/InternalApi.Domain/"]
COPY ["Shared/*.Application", "Shared/Shared.Application/"]
COPY ["Shared/*.Domain", "Shared/Shared.Domain/"]
COPY ["Contracts", "Contracts/"]
RUN dotnet publish "InternalApi/InternalApi.Api/InternalApi.Api.csproj" -c Release -o /app/publish --no-restore 

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "InternalApi.Api.dll"]