use SIAC
go 
ALTER PROCEDURE [dbo].[web_Login_validar]
(
	@Usuario	varchar(10)
)
AS
BEGIN
	declare	@Id_Sucursal	int

	SELECT	@Id_Sucursal	=	cl.Id_Sucursal
	FROM	Cli_Usuarios	cu
	JOIN	Cat_Clientes	cl	on	cl.Id_Cliente	=	cu.Id_Cliente
	WHERE	Nombre_Sesion	=	@Usuario

	SELECT	Id_Usuario,
			Id_Cliente,
			Id_ClienteP,
			Nombre,
			Password,
			Fecha_Expira,
			Tipo,
			Validar_IP,
			Direccion_IP,
			Fecha_Registro,
			@Id_Sucursal	As	[Id_Sucursal],
			Status
	FROM	Cli_Usuarios
	where	Nombre_Sesion	=	@Usuario
END