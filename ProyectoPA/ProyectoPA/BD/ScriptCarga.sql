USE FIDE_DB;
GO

INSERT INTO FIDE_CATEGORIA_TB (NOMBRE, DESCRIPCION, ID_ESTADO)
VALUES (N'Conmemorativas', N'Placas conmemorativas y de reconocimiento', 1);

DECLARE @ID_CATEGORIA INT = SCOPE_IDENTITY();

INSERT INTO FIDE_PLACA_TB (NOMBRE, DESCRIPCION, MATERIAL, TAMANO, PRECIO, IMAGEN_NOMBRE, DISPONIBLE, ID_CATEGORIA, ID_ESTADO)
VALUES
(N'Placa Reconocimiento', N'Placa para logros y trayectorias.', N'Acrilico', NULL, 8500.00,  N'/assets/img/placas/placa1.jpeg', 1, @ID_CATEGORIA, 1),
(N'Placa Conmemorativa',  N'Placa para honrar la memoria.',      N'Madera',   NULL, 12000.00, N'/assets/img/placas/placa2.jpg',  1, @ID_CATEGORIA, 1),
(N'Placa Mascota',        N'Placa personalizada para mascotas.', N'Acrilico', NULL, 6000.00,  N'/assets/img/placas/placa3.jpeg', 1, @ID_CATEGORIA, 1);
GO

DELETE FROM FIDE_PLACA_TB