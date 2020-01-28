SET IDENTITY_INSERT [dbo].[Areas] ON
INSERT INTO [dbo].[Areas] ([AreasId], [Nombre]) VALUES (1, N'Salud')
SET IDENTITY_INSERT [dbo].[Areas] OFF

SET IDENTITY_INSERT [dbo].[Rating] ON
INSERT INTO [dbo].[Rating] ([RatingId], [Nombre]) VALUES (1, N'A')
SET IDENTITY_INSERT [dbo].[Rating] OFF

SET IDENTITY_INSERT [dbo].[Proyecto] ON
INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (2, N'2020-01-01 00:00:00', 1000, 10, 10, N'Salud y mejoría', 0, 12, 10, 1)
SET IDENTITY_INSERT [dbo].[Proyecto] OFF

SET IDENTITY_INSERT [dbo].[ProyectoAreas] ON
INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (3, 1, 2)
SET IDENTITY_INSERT [dbo].[ProyectoAreas] OFF

