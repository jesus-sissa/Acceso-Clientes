use SIAC
go
CREATE PROCEDURE web_Servicios_Efectivo 293184
(
	@Id_Ficha	int
)
AS
BEGIN
	SELECT	d.Denominacion					As	[Denominacion],
			fe.Cantidad						As	[Cantidad],
			(fe.Cantidad * d.Denominacion)	As	[Importe],
			CASE d.Presentacion
			WHEN 'B'
			THEN 'BILLETE'
			WHEN 'M'
			THEN 'MONEDA'
			END								As	[Presentacion]
	FROM	Pro_FichasEfectivo	fe
	JOIN	Cat_Denominaciones	d	on	d.Id_Denominacion	=fe.Id_Denominacion
	WHERE	Id_Ficha	=	@Id_Ficha
END