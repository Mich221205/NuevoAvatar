/* ===========================DB: Notificaciones=========================== */

IF DB_ID('PV_NA_Notificaciones') IS NULL
    CREATE DATABASE PV_NA_Notificaciones;
GO
USE PV_NA_Notificaciones;
GO

CREATE TABLE Notificacion (
    ID_Notificacion INT IDENTITY(1,1) PRIMARY KEY,
    Email_Destino VARCHAR(100) NOT NULL,
    Asunto VARCHAR(200) NOT NULL,
    Cuerpo VARCHAR(MAX) NOT NULL,
    FechaEnvio DATETIME DEFAULT GETDATE(),
    Estado VARCHAR(20) DEFAULT 'Enviado'
);

/* ===========================
   INSERTS: Notificaciones (Ejemplos)
   =========================== */

INSERT INTO Notificacion (Email_Destino, Asunto, Cuerpo, Estado) VALUES
('juan.perez@cuc.cr', 'Confirmación de matrícula', '<p>Estimado Juan Pérez, su matrícula ha sido registrada exitosamente.</p>', 'Enviado'),
('maria.lopez@cuc.ac.cr', 'Actualización de curso', '<p>Se ha modificado el horario del curso Programación V.</p>', 'Enviado'),
('admin.sistema@cuc.ac.cr', 'Bitácora del sistema', '<p>Se ha generado un nuevo registro en la bitácora del sistema.</p>', 'Enviado'),
('soporte@cuc.ac.cr', 'Error en servicio de login', '<p>Se detectó un intento fallido de autenticación.</p>', 'Error'),
('juan.perez@cuc.cr', 'Pago recibido', '<p>Su pago por ₡30,000 ha sido recibido correctamente. ¡Gracias!</p>', 'Enviado'),
('maria.lopez@cuc.ac.cr', 'Nueva notificación de notas', '<p>Se han publicado las notas del periodo actual.</p>', 'Enviado'),
('coordinacion@cuc.ac.cr', 'Reporte de matrícula', '<p>Se adjunta el reporte consolidado de matrículas del periodo 2025-2.</p>', 'Pendiente'),
('estudiantes@cuc.cr', 'Recordatorio de pago', '<p>Le recordamos que tiene una factura pendiente por cancelar antes del 10/10/2025.</p>', 'Enviado'),
('profesor.gomez@cuc.ac.cr', 'Asignación de grupo', '<p>Se le ha asignado el grupo 02 del curso Bases de Datos II.</p>', 'Enviado'),
('admin.sistema@cuc.ac.cr', 'Fallo técnico detectado', '<p>El servicio de notificaciones experimentó un error temporal.</p>', 'Error');
