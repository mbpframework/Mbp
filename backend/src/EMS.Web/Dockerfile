FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EMS.Web/EMS.Web.csproj", "EMS.Web/"]
COPY ["Mpb.Core/Mbp.Core.csproj", "Mpb.Core/"]
COPY ["EMS.Application/EMS.Application.csproj", "EMS.Application/"]
COPY ["EMS.Application.Contracts/EMS.Application.Contracts.csproj", "EMS.Application.Contracts/"]
COPY ["Mbp.Ddd.Application/Mbp.Ddd.Application.csproj", "Mbp.Ddd.Application/"]
COPY ["Mpb.AspNetCore/Mbp.AspNetCore.csproj", "Mpb.AspNetCore/"]
COPY ["EMS.DomainService/EMS.Domain.csproj", "EMS.DomainService/"]
COPY ["Mbp.Authentication.JwtBearer/Mbp.Authentication.JwtBearer.csproj", "Mbp.Authentication.JwtBearer/"]
COPY ["Mbp.EntityFrameworkCore/Mbp.EntityFrameworkCore.csproj", "Mbp.EntityFrameworkCore/"]
COPY ["EMS.EntityFrameworkCore/EMS.EntityFrameworkCore.csproj", "EMS.EntityFrameworkCore/"]
COPY ["Mbp.Authentication/Mbp.Authentication.csproj", "Mbp.Authentication/"]
COPY ["Mbp.LogDashboard/Mbp.LogDashboard.csproj", "Mbp.LogDashboard/"]
COPY ["Mbp.Swagger/Mbp.Swagger.csproj", "Mbp.Swagger/"]
RUN dotnet restore "EMS.Web/EMS.Web.csproj"
COPY . .
WORKDIR "/src/EMS.Web"
RUN dotnet build "EMS.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EMS.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EMS.Web.dll"]
