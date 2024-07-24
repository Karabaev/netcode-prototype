FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Motk.Matchmaking/Motk.Matchmaking.csproj", "Motk.Matchmaking/"]
RUN dotnet restore "Motk.Matchmaking/Motk.Matchmaking.csproj"
COPY . .
WORKDIR "/src/Motk.Matchmaking"
RUN dotnet build "Motk.Matchmaking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Motk.Matchmaking.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Motk.Matchmaking.dll"]
