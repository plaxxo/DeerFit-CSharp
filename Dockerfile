# ── Build Stage ──
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DeerFit.Core/DeerFit.Core.csproj DeerFit.Core/
COPY DeerFit.Web/DeerFit.Web.csproj   DeerFit.Web/
RUN dotnet restore DeerFit.Web/DeerFit.Web.csproj

COPY . .
RUN dotnet publish DeerFit.Web/DeerFit.Web.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Runtime Stage ──
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "DeerFit.Web.dll"]