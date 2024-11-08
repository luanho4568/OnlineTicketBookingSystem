# Sử dụng image .NET SDK làm base image để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Đặt thư mục làm việc bên trong container
WORKDIR /app

# Sao chép file csproj vào container
COPY ["OnlineTicketBookingSystem/OnlineTicketBookingSystem/OnlineTicketBookingSystem.csproj", "OnlineTicketBookingSystem/"]

# Restore các dependencies của dự án
RUN dotnet restore "OnlineTicketBookingSystem/OnlineTicketBookingSystem/OnlineTicketBookingSystem.csproj"

# Sao chép toàn bộ mã nguồn vào container
COPY . .

# Build ứng dụng
RUN dotnet publish "OnlineTicketBookingSystem/OnlineTicketBookingSystem/OnlineTicketBookingSystem.csproj" -c Release -o /app/publish

# Tạo image runtime cho ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Đặt thư mục làm việc trong container
WORKDIR /app

# Sao chép các file đã build từ container trước
COPY --from=build /app/publish .

# Cung cấp cổng để ứng dụng có thể tiếp nhận request
EXPOSE 80

# Chạy ứng dụng khi container được khởi động
ENTRYPOINT ["dotnet", "OnlineTicketBookingSystem.dll"]
