#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app 
RUN apk add --no-cache icu-libs 
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src  
COPY ["HahnAPI/HahnAPI.csproj", "HahnAPI/"]
COPY ["HahnDomain/HahnDomain.csproj", "HahnDomain/"]
COPY ["HahnData/HahnData.csproj", "HahnData/"]
RUN dotnet restore "HahnAPI/HahnAPI.csproj"
COPY . .
WORKDIR "/src/HahnAPI"
RUN dotnet build "HahnAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HahnAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HahnAPI.dll"]