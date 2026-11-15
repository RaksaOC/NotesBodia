# ---- build ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY backend.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /out --no-restore /p:UseAppHost=false

# ---- run: small Alpine ASP.NET runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_GCHeapHardLimit=0x8000000 \
    DOTNET_GCgen0size=0x1000000
# required at runtime:
#   ConnectionStrings__DefaultConnection=Host=postgres;Database=notesbodia;Username=...;Password=...
#   Jwt__Key=<long random secret>
COPY --from=build /out ./
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "backend.dll"]
