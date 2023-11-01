use SIAC
go
CREATE PROCEDURE web_Dotaciones_Dotaciones
(
	@FechaDesde		DateTime,
	@FechaHasta		DateTime,
	@Id_Moneda		int			=	0,
	@Status			varchar(2)	=	''
)
AS
BEGIN
	SELECT	r.Numero_Remision							As	[Remision],
			convert(varchar(10),d.Fecha_Captura,103)	As	[Fecha],
			m.Nombre									As	[Moneda],
			d.Importe									As	[Importe],
			d.Cantidad_Envases							As	[Envases],
			CASE d.Status
			WHEN 'EN' THEN 'ENTREGADO'
			WHEN 'DE' THEN 'EN TRANSITO'
			WHEN 'CA' THEN 'CANCELADO'
			ELSE 'PENDIENTE' END						As	[Status]
	FROM	pro_Dotaciones	d
	JOIN	Cat_Remisiones	r	on	r.Id_Remision	=	d.Id_Remision
	JOIN	Cat_Monedas		m	on	m.Id_Moneda		=	d.Id_Moneda
	WHERE	((d.Status = @Status AND @Status in ('EN','DE','CA'))
	OR		(d.Status not in ('EN','DE','CA') and @Status = 'PE'))
	AND		(d.Id_Moneda = @Id_Moneda OR @Id_Moneda = 0)
	AND			(convert(varchar(10),d.Fecha_Captura,102)
	BETWEEN		convert(varchar(10),@FechaDesde,102)
	AND			convert(varchar(10),@FechaHasta,102))
END