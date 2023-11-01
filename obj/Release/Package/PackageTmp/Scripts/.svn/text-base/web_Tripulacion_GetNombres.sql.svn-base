use SIAC
go
CREATE PROCEDURE web_Tripulacion_GetNombres
(
	@IdPunto	int
)
AS
BEGIN
	Select	o.Id_Empleado		As	[Id_Operador],
			o.Nombre			As	[Operador],
			o.Clave_Empleado	As	[ClaveOperador],
			c.Id_Empleado		As	[Id_Cajero],
			c.Nombre			As	[Cajero],
			c.Clave_Empleado	As	[ClaveCajero]
	from	TV_Programacion	pr
	join	TV_Puntos		pu	on	pu.Id_Programacion	=	pr.Id_Programacion
	join	Cat_Empleados	o	on	o.Id_Empleado		=	pr.Operador
	join	Cat_Empleados	c	on	c.Id_Empleado		=	pr.Cajero
	where	pu.Id_Punto		=	@IdPunto
END