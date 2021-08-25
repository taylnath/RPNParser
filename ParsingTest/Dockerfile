FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ParsingTest/ParsingTest.fsproj", "ParsingTest/"]
RUN dotnet restore "ParsingTest/ParsingTest.fsproj"
COPY . .
WORKDIR "/src/ParsingTest"
RUN dotnet build "ParsingTest.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParsingTest.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParsingTest.dll"]
