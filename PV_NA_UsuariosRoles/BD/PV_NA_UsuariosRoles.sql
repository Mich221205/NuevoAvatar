/* ===========================
   DB: Usuarios y Roles
   =========================== */

IF DB_ID('PV_NA_UsuariosRoles') IS NULL
    CREATE DATABASE PV_NA_UsuariosRoles;
GO
USE PV_NA_UsuariosRoles;
GO

CREATE TABLE Rol (
    ID_Rol INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE,
    CONSTRAINT CHK_Rol_Nombre CHECK (Nombre NOT LIKE '%[^A-Za-z ]%')
);

CREATE TABLE Usuario (
    ID_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Tipo_Identificacion VARCHAR(20) NOT NULL,
    Identificacion VARCHAR(20) NOT NULL UNIQUE,
    Nombre VARCHAR(150) NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    ID_Rol INT NOT NULL,
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (ID_Rol) REFERENCES Rol(ID_Rol),
    CONSTRAINT CHK_Email CHECK (Email LIKE '%cuc.cr' OR Email LIKE '%cuc.ac.cr')
);

CREATE TABLE Parametro (
    ID_Parametro VARCHAR(10) PRIMARY KEY,
    Valor VARCHAR(500) NOT NULL
);

CREATE TABLE Modulo (
    ID_Modulo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Sesion (
    ID_Sesion INT IDENTITY(1,1) PRIMARY KEY,
    ID_Usuario INT NOT NULL,
    Token_JWT VARCHAR(500) NOT NULL,
    Refresh_Token VARCHAR(500) NOT NULL,
    Expira DATETIME NOT NULL,
    CONSTRAINT FK_Sesion_Usuario FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario)
);

/* ===========================
   INSERTS: Usuarios y Roles (Ejemplos)
   =========================== */

-- ===========================
-- ROLES
-- ===========================
INSERT INTO Rol (Nombre) VALUES
('Estudiante'),
('Profesor'),
('Administrador'),
('Coordinador'),
('Soporte'),
('Secretar�a'),
('Finanzas'),
('Invitado'),
('Auditor'),
('SuperAdmin');

-- ===========================
-- USUARIOS
-- ===========================
INSERT INTO Usuario (Email, Tipo_Identificacion, Identificacion, Nombre, Contrasena, ID_Rol) VALUES
('juan.perez@cuc.cr', 'C�dula', '123456789', 'Juan P�rez', 'HASH123', 1),
('maria.lopez@cuc.ac.cr', 'C�dula', '987654321', 'Mar�a L�pez', 'HASH456', 2),
('admin.general@cuc.ac.cr', 'Pasaporte', 'A112233', 'Admin General', 'HASH789', 3),
('carlos.gomez@cuc.cr', 'C�dula', '112233445', 'Carlos G�mez', 'HASH321', 1),
('ana.morales@cuc.ac.cr', 'C�dula', '223344556', 'Ana Morales', 'HASH654', 2),
('luis.rodriguez@cuc.ac.cr', 'C�dula', '334455667', 'Luis Rodr�guez', 'HASH987', 4),
('soporte@cuc.ac.cr', 'C�dula', '445566778', 'Soporte T�cnico', 'HASH741', 5),
('secretaria@cuc.ac.cr', 'C�dula', '556677889', 'Secretar�a Acad�mica', 'HASH852', 6),
('finanzas@cuc.ac.cr', 'C�dula', '667788990', 'Oficina de Finanzas', 'HASH963', 7),
('auditor@cuc.ac.cr', 'C�dula', '778899001', 'Auditor Interno', 'HASH159', 9);

-- ===========================
-- PAR�METROS DEL SISTEMA
-- ===========================
INSERT INTO Parametro (ID_Parametro, Valor) VALUES
('TOKENEXP', '5'), -- minutos de expiraci�n
('REFEXP', '15'),  -- minutos de expiraci�n del refresh token
('MAILDOM', 'cuc.ac.cr'),
('STUDDOM', 'cuc.cr'),
('MAXLOGIN', '3'),
('HASHALG', 'bcrypt'),
('LOGLEVEL', 'INFO'),
('MAXBITA', '5000'),
('MAILSRV', 'smtp.cuc.ac.cr'),
('MAILUSER', 'notificaciones@cuc.ac.cr');

-- ===========================
-- M�DULOS DISPONIBLES
-- ===========================
INSERT INTO Modulo (Nombre) VALUES
('Usuarios y Roles'),
('Acad�mico'),
('Matr�cula'),
('Pagos'),
('Notificaciones'),
('Bit�cora'),
('Cat�logos'),
('Seguridad'),
('Integraci�n'),
('Reportes');

-- ===========================
-- SESIONES (EJEMPLOS DE LOGIN)
-- ===========================
INSERT INTO Sesion (ID_Usuario, Token_JWT, Refresh_Token, Expira) VALUES
(1, 'JWT_TOKEN_ABC123', 'REF_TOKEN_XYZ456', DATEADD(MINUTE, 5, GETDATE())),
(2, 'JWT_TOKEN_DEF234', 'REF_TOKEN_UVW567', DATEADD(MINUTE, 5, GETDATE())),
(3, 'JWT_TOKEN_GHI345', 'REF_TOKEN_RST678', DATEADD(MINUTE, 5, GETDATE())),
(4, 'JWT_TOKEN_JKL456', 'REF_TOKEN_OPQ789', DATEADD(MINUTE, 5, GETDATE())),
(5, 'JWT_TOKEN_MNO567', 'REF_TOKEN_LMN890', DATEADD(MINUTE, 5, GETDATE())),
(6, 'JWT_TOKEN_PQR678', 'REF_TOKEN_IJK901', DATEADD(MINUTE, 5, GETDATE())),
(7, 'JWT_TOKEN_STU789', 'REF_TOKEN_FGH012', DATEADD(MINUTE, 5, GETDATE())),
(8, 'JWT_TOKEN_VWX890', 'REF_TOKEN_DEF123', DATEADD(MINUTE, 5, GETDATE())),
(9, 'JWT_TOKEN_YZA901', 'REF_TOKEN_ABC234', DATEADD(MINUTE, 5, GETDATE())),
(10, 'JWT_TOKEN_BCD012', 'REF_TOKEN_ZYX345', DATEADD(MINUTE, 5, GETDATE()));
