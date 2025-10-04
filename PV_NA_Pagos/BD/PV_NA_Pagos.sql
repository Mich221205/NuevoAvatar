/* ===========================
   DB: Pagos   =========================== */

IF DB_ID('PV_NA_Pagos') IS NULL
    CREATE DATABASE PV_NA_Pagos;
GO
USE PV_NA_Pagos;
GO

CREATE TABLE Factura (
    ID_Factura INT IDENTITY(1,1) PRIMARY KEY,
    ID_Estudiante INT NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Pendiente',
    Fecha DATETIME DEFAULT GETDATE()
);

CREATE TABLE DetalleFactura (
    ID_Detalle INT IDENTITY(1,1) PRIMARY KEY,
    ID_Factura INT NOT NULL,
    Descripcion VARCHAR(200) DEFAULT 'Servicios estudiantiles',
    CONSTRAINT FK_DetalleFactura FOREIGN KEY (ID_Factura) REFERENCES Factura(ID_Factura)
);

CREATE TABLE Pago (
    ID_Pago INT IDENTITY(1,1) PRIMARY KEY,
    ID_Factura INT NOT NULL,
    FechaPago DATETIME DEFAULT GETDATE(),
    Monto DECIMAL(10,2) NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Aplicado',
    CONSTRAINT FK_Pago_Factura FOREIGN KEY (ID_Factura) REFERENCES Factura(ID_Factura)
);

/* ===========================
   INSERTS: Pagos y Facturación (Ejemplos)
   =========================== */

-- ===========================
-- FACTURAS
-- ===========================
INSERT INTO Factura (ID_Estudiante, Monto, Impuesto, Estado) VALUES
(1, 30000.00, 600.00, 'Pendiente'),
(2, 30000.00, 600.00, 'Pendiente'),
(3, 60000.00, 1200.00, 'Pendiente'),
(4, 30000.00, 600.00, 'Pagada'),
(5, 30000.00, 600.00, 'Pendiente'),
(6, 90000.00, 1800.00, 'Pendiente'),
(7, 30000.00, 600.00, 'Pagada'),
(8, 60000.00, 1200.00, 'Pendiente'),
(9, 30000.00, 600.00, 'Pendiente'),
(10, 30000.00, 600.00, 'Pendiente');


-- ===========================
-- DETALLE DE FACTURAS
-- ===========================
INSERT INTO DetalleFactura (ID_Factura, Descripcion) VALUES
(1, 'Servicios estudiantiles - Curso Programación V'),
(2, 'Servicios estudiantiles - Curso Matemática General'),
(3, 'Servicios estudiantiles - Curso Física I y II'),
(4, 'Servicios estudiantiles - Curso Bases de Datos'),
(5, 'Servicios estudiantiles - Curso Inglés Técnico'),
(6, 'Servicios estudiantiles - Curso Proyecto Integrador'),
(7, 'Servicios estudiantiles - Curso Ética Profesional'),
(8, 'Servicios estudiantiles - Curso Arquitectura de Software'),
(9, 'Servicios estudiantiles - Curso Redes y Comunicaciones'),
(10, 'Servicios estudiantiles - Curso Ingeniería de Software');


-- ===========================
-- PAGOS
-- ===========================
INSERT INTO Pago (ID_Factura, Monto, Estado) VALUES
(4, 30600.00, 'Aplicado'),
(7, 30600.00, 'Aplicado'),
(1, 0.00, 'Pendiente'),
(2, 0.00, 'Pendiente'),
(3, 0.00, 'Pendiente'),
(5, 0.00, 'Pendiente'),
(6, 0.00, 'Pendiente'),
(8, 0.00, 'Pendiente'),
(9, 0.00, 'Pendiente'),
(10, 0.00, 'Pendiente');
