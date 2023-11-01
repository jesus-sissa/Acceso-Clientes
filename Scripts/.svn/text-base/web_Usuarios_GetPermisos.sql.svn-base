CREATE PROCEDURE web_Usuarios_GetMenus
(
	@IdUsuario	int
)
AS
BEGIN
	Select	m.Id_Menu,
			m.Descripcion,
			m.Enlace
	From	Cli_Menus		m
	JOIN	Cli_Opciones	o	on	o.Id_Menu		=	m.Id_Menu
								and	o.Status		=	'A'
	JOIN	Cli_Permisos	p	on	p.Id_Opcion		=	o.Id_Opcion
	where	p.Id_Usuario	=	@IdUsuario
	and		m.Status		=	'A'
	order by
			m.Orden
END
GO
CREATE PROCEDURE web_Usuarios_GetOpciones
(
	@IdUsuario	int
)
AS
BEGIN
	Select	o.Id_Menu,
			o.Descripcion,
			o.Enlace
	From	Cli_Opciones	o	
	JOIN	Cli_Permisos	p	on	p.Id_Opcion		=	o.Id_Opcion
	where	p.Id_Usuario	=	@IdUsuario
	and		o.Status		=	'A'
	order by
			o.Orden
END