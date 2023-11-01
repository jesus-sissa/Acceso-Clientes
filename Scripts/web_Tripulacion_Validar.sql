use SIAC
go
CREATE PROCEDURE web_Tripulacion_Validar
(
	@IdCliente	int
)
AS
BEGIN
	select	pu.Id_Punto,
			convert(varchar(10),pr.Fecha,103)	As	[Fecha],
			u.Descripcion						As	[Unidad],
			o.Nombre_Comercial					As	[Origen],
			d.Nombre_Comercial					As	[Destino]
	from	TV_Programacion	pr
	join	TV_Puntos		pu	on	pu.Id_Programacion	= pr.Id_Programacion
	join	Cat_Unidades	u	on	u.Id_Unidad			= pr.Id_Unidad
	join	Cat_Clientes	o	on	o.Id_Cliente		= pu.Cliente_Origen
	join	Cat_Clientes	d	on	d.Id_Cliente		= pu.Cliente_Destino
	where	(pu.Cliente_Origen	=	@IdCliente
	or		pu.Cliente_Origen	=	@IdCliente)
	and		CONVERT(varchar(10),pr.Fecha,102) = CONVERT(varchar(10),GETDATE(),102)
END