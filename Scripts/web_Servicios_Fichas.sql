use SIAC
go
CREATE PROCEDURE web_Servicios_Fichas
(
	@Id_Servicio	int
)
AS
BEGIN
	SELECT	f.Id_Ficha,
			f.Numero_Ficha		As	[Ficha],
			m.Nombre			As	[Moneda],
			CASE f.Tipo_Ficha	
			WHEN 'N'
			THEN 'NORMAL'
			WHEN 'P'
			THEN 'PARCIAL'
			END					As	[Tipo],
			f.Importe_Efectivo	As	[Efectivo],
			f.Importe_Cheques	As	[Cheques],
			f.Importe_Otros		As	[Otros],
			f.Dif_Efectivo		As	[Dif. Efectivo],
			f.Dif_Cheques		As	[Dif. Cheques],
			f.Dif_Otros			As	[Dif. Otros]
	FROM	Pro_Fichas	f
	JOIN	Cat_Monedas	m	on	m.Id_Moneda	=	f.Id_Moneda
	WHERE	f.Id_Servicio	=	@Id_Servicio
END