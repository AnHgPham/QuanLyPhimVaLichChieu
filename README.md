# 🎬 Quản Lý Phim & Lịch Chiếu

Ứng dụng desktop Windows quản lý phim, phòng chiếu, suất chiếu và bán vé — xây dựng bằng **C# WinForms (.NET 6)** theo kiến trúc 3 lớp.

## 📸 Giao diện

Ứng dụng sử dụng giao diện **Dark Theme** hiện đại với:
- Sidebar điều hướng
- Dashboard tổng quan
- DataGridView theo phong cách tối

## 🏗️ Kiến trúc

```
QuanLyPhimVaLichChieu/
├── Models/          # Các lớp thực thể (Phim, PhongChieu, SuatChieu, Ve, ...)
├── DataAccess/      # Tầng truy cập dữ liệu (DAL)
├── BusinessLogic/   # Tầng xử lý nghiệp vụ (BLL)
├── Forms/           # Giao diện WinForms + UITheme
├── Database/        # Script tạo CSDL SQL Server
├── Program.cs       # Entry point
└── App.config       # Cấu hình connection string
```

## ⚙️ Yêu cầu

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) trở lên
- **SQL Server Express** (hoặc SQL Server bất kỳ)

## 🚀 Hướng dẫn chạy

### 1. Tạo cơ sở dữ liệu

```bash
sqlcmd -S localhost\SQLEXPRESS01 -i "Database/CreateDatabase.sql"
```

> ⚠️ Thay `SQLEXPRESS01` bằng tên instance SQL Server của bạn.

### 2. Cấu hình connection string

Mở `App.config` và chỉnh `Data Source` cho khớp:

```xml
<add name="QuanLyPhimDB" 
     connectionString="Data Source=localhost\SQLEXPRESS01;Initial Catalog=QuanLyPhimDB;Integrated Security=True;TrustServerCertificate=True;" 
     providerName="System.Data.SqlClient" />
```

### 3. Chạy ứng dụng

```bash
dotnet restore
dotnet run
```

Hoặc mở `QuanLyPhimVaLichChieu.sln` bằng **Visual Studio** và nhấn **F5**.

## 📋 Chức năng

| Chức năng | Mô tả |
|-----------|-------|
| **Quản lý phim** | Thêm, sửa, xóa, tìm kiếm phim theo tên và thể loại |
| **Phòng chiếu** | Quản lý phòng chiếu (2D, 3D, IMAX) và trạng thái |
| **Suất chiếu** | Tạo và quản lý lịch chiếu, lọc theo phim/ngày |
| **Bán vé** | Bán vé, chọn ghế, hủy vé |
| **Thống kê** | Báo cáo vé bán theo phim, tổng doanh thu |

## 🛠️ Công nghệ

- **C# / .NET 6** — WinForms
- **SQL Server** — Cơ sở dữ liệu
- **System.Data.SqlClient** — Kết nối database
- **Kiến trúc 3 lớp** — Models / DataAccess / BusinessLogic
