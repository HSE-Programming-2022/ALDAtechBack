
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore "./AldaTech-api.csproj"
RUN dotnet publish "./AldaTech-api.csproj" -c release -o /app --no-restore



FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /apps
COPY --from=build /app/ .
EXPOSE 80
EXPOSE 443
EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "AldaTech-api.dll"]
# ENTRYPOINT ["dotnet", "run"]

