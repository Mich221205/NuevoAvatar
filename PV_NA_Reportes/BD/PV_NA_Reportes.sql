/* ===========================
   DB: Reportes y Estad�sticas =========================== */

IF DB_ID('PV_NA_Reportes') IS NULL
    CREATE DATABASE PV_NA_Reportes;
GO
USE PV_NA_Reportes;
GO

CREATE TABLE Reporte (
    ID_Reporte INT IDENTITY(1,1) PRIMARY KEY,
    Tipo VARCHAR(50) NOT NULL,
    Contenido NVARCHAR(MAX) NOT NULL
        CONSTRAINT CHK_Reporte_JSON CHECK (ISJSON(Contenido) = 1),
    FechaGeneracion DATETIME DEFAULT GETDATE()
);

/* ===========================
   INSERTS: Reportes y Estad�sticas (Ejemplos)
   =========================== */

INSERT INTO Reporte (Tipo, Contenido) VALUES
('Reporte de Matr�cula',
 N'{
    "Periodo": "2025-1",
    "TotalEstudiantes": 350,
    "TotalCursos": 42,
    "CarrerasDestacadas": ["Ingenier�a en Software", "Administraci�n de Empresas"]
 }'),

('Reporte de Pagos',
 N'{
    "Periodo": "2025-1",
    "FacturasGeneradas": 340,
    "PagosCompletados": 310,
    "MontoTotal": 10500000.50,
    "PorcentajeCobro": 91.17
 }'),

('Reporte de Notas',
 N'{
    "Curso": "Programaci�n II",
    "Grupo": "01",
    "PromedioGeneral": 84.6,
    "EstudiantesAprobados": 25,
    "EstudiantesReprobados": 5
 }'),

('Reporte de Asistencia',
 N'{
    "Curso": "Bases de Datos I",
    "Periodo": "2025-1",
    "TotalClases": 32,
    "PromedioAsistencia": 93.2
 }'),

('Reporte de Profesores',
 N'{
    "Periodo": "2025-1",
    "TotalProfesores": 45,
    "TopEvaluados": [
        {"Nombre": "Juan Castro", "PromedioEvaluacion": 9.8},
        {"Nombre": "Mar�a L�pez", "PromedioEvaluacion": 9.5}
    ]
 }'),

('Reporte de Facturaci�n',
 N'{
    "Periodo": "2025-1",
    "FacturasEmitidas": 350,
    "FacturasPendientes": 40,
    "MontoBruto": 12000000.00,
    "Impuesto": 240000.00,
    "TotalNeto": 12240000.00
 }'),

('Reporte de Bit�coras',
 N'{
    "RangoFechas": "2025-10-01 al 2025-10-04",
    "TotalRegistros": 480,
    "OperacionesMasFrecuentes": ["Consulta", "Creaci�n", "Actualizaci�n"]
 }'),

('Reporte de Sesiones',
 N'{
    "Periodo": "�ltimas 24 horas",
    "SesionesActivas": 58,
    "UsuariosUnicos": 52,
    "PromedioDuracionMinutos": 14.3
 }'),

('Reporte de Carreras',
 N'{
    "Institucion": "Colegio Universitario de Cartago",
    "TotalCarreras": 12,
    "MasMatriculada": "Ingenier�a en Software",
    "MenosMatriculada": "Dise�o Publicitario"
 }'),

('Reporte de Sistema',
 N'{
    "FechaCorte": "2025-10-04T10:00:00",
    "ServiciosOperativos": 9,
    "ServiciosInactivos": 1,
    "UltimoBackup": "2025-10-03T23:30:00",
    "Servidor": "SRV-PROD-01"
 }');
