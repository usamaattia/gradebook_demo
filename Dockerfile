# ---------- Build stage: test and publish the app ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy everything (bin, obj and .db files are skipped by .dockerignore)
COPY . .

# Run the tests first. If any test fails, the build stops here.
RUN dotnet test Gradebook.Tests

# Publish the web app in Release mode
RUN dotnet publish Gradebook/Gradebook.csproj -c Release -o /app/publish

# ---------- Final stage: a small image that only runs the app ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# Render sends traffic to port 10000
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Gradebook.dll"]
