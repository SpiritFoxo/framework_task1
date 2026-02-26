# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file first → enables layer caching for restore
COPY Pr1.MinWebService.csproj .
RUN dotnet restore Pr1.MinWebService.csproj

# Copy everything else
COPY . .

# Build
RUN dotnet build Pr1.MinWebService.csproj -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish Pr1.MinWebService.csproj -c Release -o /app/publish --no-restore

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:54254
COPY --from=publish /app/publish .
EXPOSE 54254
ENTRYPOINT ["dotnet", "Pr1.MinWebService.dll"]