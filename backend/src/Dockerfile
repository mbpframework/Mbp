FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Medical.Ai.Mbdp.Web/Medical.Ai.Mbdp.Web.csproj", "Medical.Ai.Mbdp.Web/"]
COPY ["Mpb.Core/Mbp.Core.csproj", "Mpb.Core/"]
COPY ["Medical.Ai.Mbdp.Application/Medical.Ai.Mbdp.Application.csproj", "Medical.Ai.Mbdp.Application/"]
COPY ["Medical.Ai.Mbdp.Application.Contracts/Medical.Ai.Mbdp.Application.Contracts.csproj", "Medical.Ai.Mbdp.Application.Contracts/"]
COPY ["Mbp.Ddd.Application/Mbp.Ddd.Application.csproj", "Mbp.Ddd.Application/"]
COPY ["Mpb.AspNetCore/Mbp.AspNetCore.csproj", "Mpb.AspNetCore/"]
COPY ["Medical.Ai.Mbdp.DomainService/Medical.Ai.Mbdp.Domain.csproj", "Medical.Ai.Mbdp.DomainService/"]
COPY ["Mbp.Authentication.JwtBearer/Mbp.Authentication.JwtBearer.csproj", "Mbp.Authentication.JwtBearer/"]
COPY ["Mbp.EntityFrameworkCore/Mbp.EntityFrameworkCore.csproj", "Mbp.EntityFrameworkCore/"]
COPY ["Medical.Ai.Mbdp.EntityFrameworkCore/Medical.Ai.Mbdp.EntityFrameworkCore.csproj", "Medical.Ai.Mbdp.EntityFrameworkCore/"]
COPY ["Mbp.Authentication/Mbp.Authentication.csproj", "Mbp.Authentication/"]
COPY ["Mbp.LogDashboard/Mbp.LogDashboard.csproj", "Mbp.LogDashboard/"]
COPY ["Mbp.Swagger/Mbp.Swagger.csproj", "Mbp.Swagger/"]
RUN dotnet restore "Medical.Ai.Mbdp.Web/Medical.Ai.Mbdp.Web.csproj"
COPY . .
WORKDIR "/src/Medical.Ai.Mbdp.Web"
RUN dotnet build "Medical.Ai.Mbdp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Medical.Ai.Mbdp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Medical.Ai.Mbdp.Web.dll"]