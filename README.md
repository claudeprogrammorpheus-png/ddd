# SE Lab 6 – ASP.NET MVC (.NET Framework)

Bài nộp gồm 4 bài tập:

| Bài | Nội dung | Vị trí trong project |
|-----|----------|----------------------|
| 1 (Ex 1) | Repo GitHub, commit và push | Thư mục gốc (`.git`, `.gitignore`) |
| 2 (Ex 2) | CRUD: Students, Courses, Instructors, Enrollments, Departments | `Controllers/*Controller.cs`, `Models/`, `Views/<Entity>/` |
| 3 (Ex 3) | Bảng `tblUser`, form đăng nhập là màn hình đầu tiên | `Controllers/AccountController.cs`, `Views/Account/`, `Filters/LoginRequiredAttribute.cs` |
| 4 (Ex 4) | Báo cáo RDLC theo khóa học, xem trước và xuất PDF | `Controllers/ReportController.cs`, `Reports/StudentReport.rdlc`, `Views/Report/` |

Cấu trúc:

```
SE_Lab6_Submission/
├── database/SchoolDB.sql            ← tạo CSDL, bảng, dữ liệu mẫu, tblUser
├── ASPNETWebAppMVCStudentApp/       ← mã nguồn project ASP.NET MVC (.NET Framework 4.8)
│   ├── App_Start/, Filters/, Helpers/, Models/, Controllers/, Views/, Reports/
│   ├── Global.asax.cs, Web.config, packages.config
└── README.md
```

## Yêu cầu môi trường

- Windows, Visual Studio 2019 hoặc 2022 (workload **ASP.NET and web development**, có **.NET Framework 4.8 development tools**)
- SQL Server (Express là đủ) và SQL Server Management Studio (SSMS)
- Git for Windows

## Các bước chạy

### 1. Tạo CSDL

1. Mở SSMS, kết nối tới server của bạn.
2. Mở `database/SchoolDB.sql` rồi nhấn **Execute** (F5).
3. Kiểm tra: database `SchoolDB` có các bảng `Departments`, `Instructors`, `Courses`, `Students`, `Enrollments`, `tblUser`.

Tài khoản test: `admin / admin123` hoặc `teacher / teacher123`.

### 2. Tạo project trong Visual Studio và thêm mã nguồn

1. **File → New → Project → ASP.NET Web Application (.NET Framework)**, đặt tên `ASPNETWebAppMVCStudentApp`, chọn template **MVC**, nhấn **Create**.
2. Mở **Manage NuGet Packages** và cài:
   - `EntityFramework` (6.4.4)
   - `Microsoft.ReportingServices.ReportViewerControl.WebForms` (dùng cho Exercise 4)
3. Copy các thư mục và file trong `ASPNETWebAppMVCStudentApp/` vào project vừa tạo, ghi đè khi được hỏi:
   - `App_Start/`, `Filters/`, `Helpers/`, `Models/`, `Controllers/`, `Views/`, `Reports/`
   - `Global.asax.cs`
   - `Web.config` (sau khi copy, mở lại và đảm bảo `<connectionStrings>` có `SchoolDBEntities`)
4. Trong project, nhấp chuột phải **Reports → Add → Existing Item** nếu `StudentReport.rdlc` chưa hiện, rồi chọn **Properties → Copy to Output Directory: Copy if newer** (Build Action để mặc định là `Content`).
5. Trong `Web.config`, sửa `data source=.` thành tên SQL Server của bạn nếu khác.
6. Với mỗi file `.cs` và `.cshtml` vừa copy, Visual Studio sẽ tự thêm vào project. Nếu thấy lỗi `The type or namespace name 'Microsoft' ...` ở `ReportController.cs`, kiểm tra lại đã cài gói ReportViewer ở bước 2.
7. **Build → Rebuild Solution**, không được có lỗi.

### 3. Chạy ứng dụng

1. Nhấn **F5**. Trình duyệt mở tại trang **Login** (màn hình đầu tiên).
2. Đăng nhập `admin / admin123` → chuyển đến danh sách sinh viên.
3. Dùng menu trên cùng để kiểm tra CRUD cho từng bảng.
4. Vào **Student Report**, chọn khóa học, nhấn **Print Preview** (xem PDF trên trình duyệt) hoặc **Export to PDF** (tải về).
5. Để kiểm tra Exercise 3: nhập sai mật khẩu → chọn **Reset password** → nhập đúng username + email (`admin@seuni.com`) và mật khẩu mới → đăng nhập lại.

### 4. Đưa lên GitHub (Exercise 1)

Mở **Git Changes** trong Visual Studio:

1. **Initialize Repository**.
2. Trên GitHub, tạo repo `ASPNETWebAppMVCStudentApp`, **không** tick README, `.gitignore` hay license.
3. Trong Git Changes, **Settings → Repository Settings → Remotes → Add**, tên `origin`, dán URL repo.
4. Stage tất cả (+), commit với message `Initial commit of ASP.NET MVC Student App`, rồi **Push**.

Lưu ý: thêm `.gitignore` chuẩn của Visual Studio (chọn **Add → New Item → Visual C# → .gitignore**) để không đưa thư mục `bin/`, `obj/`, `packages/` lên GitHub.

## Ghi chú kỹ thuật

- `Database.SetInitializer(null)` trong `Global.asax.cs`: EF không tự tạo/sửa bảng, CSDL lấy từ `SchoolDB.sql`.
- Mật khẩu trong `tblUser` lưu dạng băm SHA-256 (`VARBINARY(64)`). C# băm bằng `Encoding.Unicode` để khớp với `HASHBYTES('SHA2_256', N'...')` của SQL Server (`Helpers/PasswordHelper.cs`).
- Các action có `[AllowAnonymous]` (Login, ResetPassword) không cần đăng nhập. Mọi trang khác được bảo vệ bởi `LoginRequiredAttribute` qua `Session["UserID"]`.
- Model được viết theo kiểu Database First bằng code (class + `DbContext`), tương đương file `.edmx` do wizard sinh ra. Nếu giảng viên yêu cầu đúng wizard EDMX, dùng **Add → ADO.NET Entity Data Model → EF Designer from database** với 6 bảng trong `SchoolDB`; tên context là `SchoolDBEntities`.
- Layout dùng Bootstrap và jQuery qua CDN, nên không cần cấu hình bundle.
