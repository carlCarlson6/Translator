FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["src/TranslatorWebApp/TranslatorWebApp.csproj", "TranslatorWebApp/"]
RUN dotnet restore "TranslatorWebApp/TranslatorWebApp.csproj"
COPY . .
WORKDIR "/src/TranslatorWebApp"
RUN dotnet build "TranslatorWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TranslatorWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TranslatorWebApp.dll"]