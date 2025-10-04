/* ===========================
   DB: Reportes y Estadísticas =========================== */

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
   INSERTS: Reportes y Estadísticas (Ejemplos)
   =========================== */

INSERT INTO Reporte (Tipo, Contenido) VALUES
('Reporte de Matrícula',
 N'{
    "Periodo": "2025-1",
    "TotalEstudiantes": 350,
    "TotalCursos": 42,
    "CarrerasDestacadas": ["Ingeniería en Software", "Administración de Empresas"]
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
    "Curso": "Programación II",
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
        {"Nombre": "María López", "PromedioEvaluacion": 9.5}
    ]
 }'),

('Reporte de Facturación',
 N'{
    "Periodo": "2025-1",
    "FacturasEmitidas": 350,
    "FacturasPendientes": 40,
    "MontoBruto": 12000000.00,
    "Impuesto": 240000.00,
    "TotalNeto": 12240000.00
 }'),

('Reporte de Bitácoras',
 N'{
    "RangoFechas": "2025-10-01 al 2025-10-04",
    "TotalRegistros": 480,
    "OperacionesMasFrecuentes": ["Consulta", "Creación", "Actualización"]
 }'),

('Reporte de Sesiones',
 N'{
    "Periodo": "Últimas 24 horas",
    "SesionesActivas": 58,
    "UsuariosUnicos": 52,
    "PromedioDuracionMinutos": 14.3
 }'),

('Reporte de Carreras',
 N'{
    "Institucion": "Colegio Universitario de Cartago",
    "TotalCarreras": 12,
    "MasMatriculada": "Ingeniería en Software",
    "MenosMatriculada": "Diseño Publicitario"
 }'),

('Reporte de Sistema',
 N'{
    "FechaCorte": "2025-10-04T10:00:00",
    "ServiciosOperativos": 9,
    "ServiciosInactivos": 1,
    "UltimoBackup": "2025-10-03T23:30:00",
    "Servidor": "SRV-PROD-01"
 }');
