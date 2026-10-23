/* =====================================================================
   SE Lab 6 - SchoolDB (Exercise 2 + Exercise 3)
   Chạy file này trong SQL Server Management Studio (SSMS).
   ===================================================================== */

IF DB_ID(N'SchoolDB') IS NULL
    CREATE DATABASE SchoolDB;
GO

USE SchoolDB;
GO

/* Xóa bảng cũ (đúng thứ tự khóa ngoại) để script chạy lại được */
IF OBJECT_ID(N'dbo.Enrollments', N'U') IS NOT NULL DROP TABLE dbo.Enrollments;
IF OBJECT_ID(N'dbo.Courses', N'U')     IS NOT NULL DROP TABLE dbo.Courses;
IF OBJECT_ID(N'dbo.Students', N'U')    IS NOT NULL DROP TABLE dbo.Students;
IF OBJECT_ID(N'dbo.Instructors', N'U') IS NOT NULL DROP TABLE dbo.Instructors;
IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL DROP TABLE dbo.Departments;
IF OBJECT_ID(N'dbo.tblUser', N'U')     IS NOT NULL DROP TABLE dbo.tblUser;
GO

/* ---------- Exercise 2: các bảng nghiệp vụ ---------- */
CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(50)  NOT NULL,
    Budget       DECIMAL(18,2) NOT NULL,
    StartDate    DATE          NOT NULL
);

CREATE TABLE Instructors (
    InstructorID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName    NVARCHAR(50) NOT NULL,
    LastName     NVARCHAR(50) NOT NULL,
    HireDate     DATE         NOT NULL
);

CREATE TABLE Courses (
    CourseID     INT IDENTITY(1,1) PRIMARY KEY,
    Title        NVARCHAR(100) NOT NULL,
    Credits      INT           NOT NULL,
    DepartmentID INT           NULL FOREIGN KEY REFERENCES Departments(DepartmentID)
);

CREATE TABLE Students (
    StudentID      INT IDENTITY(1,1) PRIMARY KEY,
    FirstName      NVARCHAR(50) NOT NULL,
    LastName       NVARCHAR(50) NOT NULL,
    EnrollmentDate DATE         NOT NULL
);

CREATE TABLE Enrollments (
    EnrollmentID INT IDENTITY(1,1) PRIMARY KEY,
    CourseID     INT NOT NULL FOREIGN KEY REFERENCES Courses(CourseID),
    StudentID    INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID),
    Grade        DECIMAL(3,2) NULL
);
GO

/* ---------- Exercise 3: bảng tài khoản đăng nhập ----------
   Mật khẩu được lưu dưới dạng băm SHA-256 (VARBINARY), không lưu plain text. */
CREATE TABLE tblUser (
    UserID       INT IDENTITY(1,1) PRIMARY KEY,
    Username     NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,
    Email        NVARCHAR(100) NOT NULL,
    FullName     NVARCHAR(100) NOT NULL
);
GO

/* ---------- Dữ liệu mẫu ---------- */
INSERT INTO Departments (Name, Budget, StartDate) VALUES
    (N'Computer Science', 120000.00, '2023-01-01'),
    (N'Mathematics',       90000.00, '2023-01-01');

INSERT INTO Instructors (FirstName, LastName, HireDate) VALUES
    (N'John', N'Doe',   '2020-08-15'),
    (N'Mary', N'Jones', '2021-02-01');

INSERT INTO Courses (Title, Credits, DepartmentID) VALUES
    (N'Introduction to Programming', 3, 1),
    (N'Calculus I',                  4, 2),
    (N'Software Engineering',        3, 1);

INSERT INTO Students (FirstName, LastName, EnrollmentDate) VALUES
    (N'Jane',    N'Smith', '2023-09-01'),
    (N'Michael', N'Pham',  '2024-02-20');

INSERT INTO Enrollments (CourseID, StudentID, Grade) VALUES
    (1, 1, 3.50),
    (3, 1, 3.80),
    (2, 2, 2.90);

/* Tài khoản test:
     admin   / admin123
     teacher / teacher123  */
INSERT INTO tblUser (Username, PasswordHash, Email, FullName) VALUES
    (N'admin',   HASHBYTES('SHA2_256', N'admin123'),   N'admin@seuni.com',   N'Administrator'),
    (N'teacher', HASHBYTES('SHA2_256', N'teacher123'), N'teacher@seuni.com', N'Teacher');
GO
