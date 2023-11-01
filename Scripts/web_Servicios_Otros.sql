use SIAC
go
CREATE PROCEDURE web_Servicios_Otros
(
	@Id_Ficha	int
)
AS
BEGIN
	SELECT	od.Descripcion		As	[Documento],
			fo.Importe			As	[Importe],
			fo.Observaciones	As	[Observaciones]
	FROM	pro_FichasOtros		fo
	join	Cat_ODoctos			od	on	od.Id_Odocto	=	fo.Id_Odocto
	WHERE	fo.Id_Ficha	=	@Id_Ficha
END