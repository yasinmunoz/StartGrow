--SET IDENTITY_INSERT [dbo].[TiposInversiones] ON
--INSERT INTO [dbo].[TiposInversiones] ([TiposInversionesId], [Nombre]) VALUES (1, N'Crownfunding')
--INSERT INTO [dbo].[TiposInversiones] ([TiposInversionesId], [Nombre]) VALUES (2, N'Business Angels')
--INSERT INTO [dbo].[TiposInversiones] ([TiposInversionesId], [Nombre]) VALUES (3, N'Venture Capital')
--SET IDENTITY_INSERT [dbo].[TiposInversiones] OFF

--SET IDENTITY_INSERT [dbo].[Rating] ON
--INSERT INTO [dbo].[Rating] ([RatingId], [Nombre]) VALUES (1, N'A')
--INSERT INTO [dbo].[Rating] ([RatingId], [Nombre]) VALUES (2, N'B')
--INSERT INTO [dbo].[Rating] ([RatingId], [Nombre]) VALUES (3, N'C')
--SET IDENTITY_INSERT [dbo].[Rating] OFF

--SET IDENTITY_INSERT [dbo].[Areas] ON
--INSERT INTO [dbo].[Areas] ([AreasId], [Nombre]) VALUES (1, N'Salud')
--INSERT INTO [dbo].[Areas] ([AreasId], [Nombre]) VALUES (2, N'Pinturas')
--INSERT INTO [dbo].[Areas] ([AreasId], [Nombre]) VALUES (3, N'Drogería')
--INSERT INTO [dbo].[Areas] ([AreasId], [Nombre]) VALUES (4, N'Tecnología')
--SET IDENTITY_INSERT [dbo].[Areas] OFF

--SET IDENTITY_INSERT [dbo].[Proyecto] ON
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (2, N'2020-01-01 00:00:00', 1000, 10, 10, N'Salud y mejoría', 0, 12, 10, 1)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (4, N'2019-03-01 00:00:00', 444, 44, 4, N'Mecánica', 0, 12, 14, 2)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (6, N'2021-04-01 00:00:00', 33, 32, 3, N'Educación para adultos', 0, 12, 43, 3)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (7, N'2019-05-11 00:00:00', 45, 4, 45, N'Pinturas Jafep', 0, 12, 55, 2)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (8, N'2019-11-27 00:00:00', 66, 12, 60, N'Pinturas ADORAL', 0, 5, 77, 1)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (9, N'2020-12-23 00:00:00', 70, 5, 200, N'Soluciones Informáticas', 0, 3, 27, 3)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (10, N'2019-03-13 00:00:00', 60, 8, 25, N'Limpiezas Luján', 0, 23, 39, 3)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (11, N'2020-03-24 00:00:00', 32, 3, 59, N'Salud y Mejorías', 0, 7, 87, 1)
--INSERT INTO [dbo].[Proyecto] ([ProyectoId], [FechaExpiracion], [Importe], [Interes], [MinInversion], [Nombre], [NumInversores], [Plazo], [Progreso], [RatingId]) VALUES (12, N'2019-08-02 00:00:00', 23, 4, 77, N'Erson Informática', 0, 5, 69, 2)
--SET IDENTITY_INSERT [dbo].[Proyecto] OFF

--SET IDENTITY_INSERT [dbo].[ProyectoAreas] ON
--INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (3, 1, 2)
--INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (9, 1, 11)
--INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (11, 2, 7)
--INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (12, 2, 8)
--INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (13, 3, 10)
--INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (14, 4, 9)
--INSERT INTO [dbo].[ProyectoAreas] ([ProyectoAreasId], [AreasId], [ProyectoId]) VALUES (15, 4, 12)
--SET IDENTITY_INSERT [dbo].[ProyectoAreas] OFF

SET IDENTITY_INSERT [dbo].[Inversion] ON
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (5, 10, N'En_curso', 11, N'1', N'1', 2, 1, 111)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (10, 15, N'Finalizado', 22, N'1', N'1', 2, 1, 11)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (12, 33, N'Recaudacion', 33, N'1', N'1', 2, 1, 44)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (14, 31, N'Finalizado', 5, N'1', N'1', 4, 2, 35)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (15, 11, N'En_curso', 22, N'1', N'1', 6, 3, 33)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (18, 2, N'En_curso', 20, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 2, 2, 750)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (20, 5, N'En_curso', 31, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 7, 1, 5503)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (26, 6, N'En curso', 22, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 8, 2, 2234)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (27, 12, N'Recaudacion', 4, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 9, 3, 7348)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (29, 13, N'Finalizado', 15, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 8, 2, 2341)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (30, 3, N'Finalizado', 6, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 10, 1, 1134)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (31, 6, N'En_curso', 7, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 11, 1, 998)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (32, 12, N'Finalizado', 5, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 7, 3, 5503)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (34, 9, N'En_curso', 18, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 11, 2, 998)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (35, 2, N'Finalizado', 6, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 8, 1, 2341)
INSERT INTO [dbo].[Inversion] ([InversionId], [Cuota], [EstadosInversiones], [Intereses], [InversorId], [InversorId1], [ProyectoId], [TipoInversionesId], [Total]) VALUES (37, 4, N'En_curso', 7, N'c8f3e769-0948-494b-b670-454bd4635408', N'c8f3e769-0948-494b-b670-454bd4635408', 12, 3, 7534)
SET IDENTITY_INSERT [dbo].[Inversion] OFF

SET IDENTITY_INSERT [dbo].[Monedero] ON
INSERT INTO [dbo].[Monedero] ([MonederoId], [Dinero], [InversorId]) VALUES (1, CAST(2579.40 AS Decimal(18, 2)), N'c8f3e769-0948-494b-b670-454bd4635408')
SET IDENTITY_INSERT [dbo].[Monedero] OFF



